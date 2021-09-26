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
    public class ExerciseItemUserScoresController : ODataController
    {
        private readonly kbdataContext _context;

        public ExerciseItemUserScoresController(kbdataContext context)
        {
            _context = context;
        }

        // GET: /ExerciseItemUserScores
        [EnableQuery]
        public IQueryable<ExerciseItemUserScore> Get()
        {
            return _context.ExerciseItemUserScores;
        }

        // GET: /ExerciseItemUserScores(:id)
        [EnableQuery]
        public SingleResult<ExerciseItemUserScore> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.ExerciseItemUserScores.Where(p => p.ID == key));
        }

        public async Task<IActionResult> Post([FromBody] ExerciseItemUserScore score)
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

            if (score.TakenDate == null)
                score.TakenDate = DateTime.Now;

            var scorecnt = (from dbscore in this._context.ExerciseItemUserScores
                          where dbscore.User == score.User && dbscore.RefID == score.RefID
                            && dbscore.TakenDate.Value.Date == score.TakenDate.Value.Date
                          select dbscore).Count();
            if (scorecnt > 0)
                return BadRequest("Same record exists");

            _context.ExerciseItemUserScores.Add(score);
            await _context.SaveChangesAsync();

            return Created(score);
        }

        // PUT: /ExerciseItemUserScores(5)
        /// <summary>
        /// Support for updating Knowledge items
        /// </summary>
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] ExerciseItemUserScore update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.ID)
            {
                return BadRequest();
            }

            var coll = await _context.ExerciseItemUserScores
                    .SingleOrDefaultAsync(x => x.ID == key);
            if (coll == null)
            {
                return NotFound();
            }
            coll.UpdateData(update);

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

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var coll = await _context.ExerciseItemUserScores.FindAsync(key);
            if (coll == null)
            {
                return NotFound();
            }

            _context.ExerciseItemUserScores.Remove(coll);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }

        [HttpPost]
        public IActionResult LatestUserScore([FromBody] ODataActionParameters parameters)
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

            String user = (String)parameters["User"];
            int refid = (int)parameters["RefID"];

            var scores = (from score in this._context.ExerciseItemUserScores
                          where score.User == user && score.RefID == refid
                          orderby score.TakenDate descending
                          select score).ToList();
            if (scores.Count <= 0)
                return NoContent();

            return Ok(scores[0]);
        }
    }
}
