using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Deltas;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace knowledgebuilderapi.Controllers
{
    public class DailyTracesController : ODataController
    {
        private readonly kbdataContext _context;

        public DailyTracesController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /DailyTraces
        [EnableQuery]
        public IQueryable<DailyTrace> Get()
        {
            return _context.DailyTraces;
        }

        /// GET: /DailyTraces(:id)
        [EnableQuery]
        //[HttpGet("DailyTraces(TargetUser={tagetUsr},RecordDate={dtRecord})")]
        public SingleResult<DailyTrace> Get([FromODataUri] String keyTargetUser, [FromODataUri] DateTime keyRecordDate)
        {
            return SingleResult.Create(_context.DailyTraces.Where(p => p.TargetUser == keyTargetUser && p.RecordDate.Date == keyRecordDate.Date));
        }

        // POST: /DailyTraces
        [HttpPut]
        public async Task<IActionResult> Post([FromBody] DailyTrace dt)
        {
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var err in value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(err.Exception?.Message);
                    }
                }

                return BadRequest();
            }

            // Calculate the points
            // Step 1. Get all related rules and previous result.
            var allrules = this._context.AwardRules.Where(p => p.TargetUser == dt.TargetUser);
            var prvdate = dt.RecordDate.Subtract(new TimeSpan(1, 0, 0, 0)).Date;
            var prvresults = this._context.AwardPoints.Where(p => p.TargetUser == dt.TargetUser && p.RecordDate.Date == prvdate);
            List<AwardRuleType> listTypes = new List<AwardRuleType>();
            if (dt.GoToBedTime.HasValue) {
                listTypes.Add(AwardRuleType.GoToBedTime);
            }
            else if(dt.SchoolWorkTime.HasValue)
            {
                listTypes.Add(AwardRuleType.SchoolWorkTime);
            }
            else if (dt.HandWriting.HasValue)
            {
                listTypes.Add(AwardRuleType.HandWritingHabit);
            }
            else if(dt.HomeWorkCount.HasValue)
            {
                listTypes.Add(AwardRuleType.HomeWorkCount);
            }
            else if(dt.HouseKeepingCount.HasValue) 
            {
                listTypes.Add(AwardRuleType.HouseKeepingCount);
            }
            else if(dt.PoliteBehavior.HasValue)
            {
                listTypes.Add(AwardRuleType.PoliteBehavior);
            }
            else if(dt.ErrorsCollection.HasValue)
            {
                listTypes.Add(AwardRuleType.ErrorCollectionHabit);
            }
            else if(dt.BodyExerciseCount.HasValue)
            {
                listTypes.Add(AwardRuleType.BodyExerciseCount);
            }
            else if(dt.CleanDesk.HasValue)
            {
                listTypes.Add(AwardRuleType.CleanDeakHabit);
            }

            // Update db
            _context.DailyTraces.Add(dt);
            await _context.SaveChangesAsync();

            return Created(dt);
        }

        // PUT: /DailyTraces/5
        public async Task<IActionResult> Put([FromODataUri] String keyTargetUser, [FromODataUri] DateTime keyRecordDate, [FromBody] DailyTrace update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (update.TargetUser != keyTargetUser || update.RecordDate.Date != keyRecordDate.Date)
            {
                return BadRequest("Key is not matched");
            }

            var existdbentry = await _context.DailyTraces
                    .SingleOrDefaultAsync(x => x.RecordDate.Date == keyRecordDate.Date && x.TargetUser == keyTargetUser);
            if (existdbentry == null)
            {
                return NotFound();
            }

            existdbentry.UpdateData(update);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Updated(update);
        }

        //[HttpDelete("DailyTraces(TargetUser={tagetUsr},RecordDate={dtRecord})")]
        public async Task<IActionResult> Delete([FromODataUri] String keyTargetUser, [FromODataUri] DateTime keyRecordDate)
        {
            var dt = await _context.DailyTraces.FindAsync(keyTargetUser, keyRecordDate);
            if (dt == null)
            {
                return NotFound();
            }

            _context.DailyTraces.Remove(dt);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
