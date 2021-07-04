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
    public class AwardPointsController : ODataController
    {
        private readonly kbdataContext _context;

        public AwardPointsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /AwardPoints
        [EnableQuery]
        public IQueryable<AwardPoint> Get()
        {
            return _context.AwardPoints;
        }

        /// GET: /AwardPoints(:id)
        [EnableQuery]
        public SingleResult<AwardPoint> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.AwardPoints.Where(p => p.ID == key));
        }

        // POST: /AwardPoints
        public async Task<IActionResult> Post([FromBody] AwardPoint point)
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

            // Update db
            _context.AwardPoints.Add(point);
            await _context.SaveChangesAsync();

            return Created(point);
        }

        // UPDATE: /AwardPoints
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] AwardPoint update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.ID)
            {
                return BadRequest("Key is not matched");
            }

            // Check item need be updated
            var dbentry = await _context.AwardPoints.SingleOrDefaultAsync(x => x.ID == key);
            if (dbentry == null)
            {
                return NotFound();
            }

            dbentry.UpdateData(update);

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
            var point = await _context.AwardPoints.FindAsync(key);
            if (point == null)
            {
                return NotFound();
            }

            if (point.MatchedRuleID.HasValue)
            {
                return BadRequest("Point is auto-generated one, cannot delete");
            }

            _context.AwardPoints.Remove(point);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
