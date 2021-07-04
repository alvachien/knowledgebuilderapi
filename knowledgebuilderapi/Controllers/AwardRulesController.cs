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
    public class AwardRulesController : ODataController
    {
        private readonly kbdataContext _context;

        public AwardRulesController(kbdataContext context)
        {
            _context = context;
        }

        // GET: /AwardRules
        [EnableQuery]
        public IQueryable<AwardRule> Get()
        {
            return _context.AwardRules;
        }

        // GET: /AwardRules(:id)
        [EnableQuery]
        public SingleResult<AwardRule> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.AwardRules.Where(p => p.ID == key));
        }

        // POST: /AwardRules
        public async Task<IActionResult> Post([FromBody] AwardRule rule)
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
            _context.AwardRules.Add(rule);
            await _context.SaveChangesAsync();

            return Created(rule);
        }

        // UPDATE: /AwardRules
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] AwardRule update)
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
            var dbentry = await _context.AwardRules.SingleOrDefaultAsync(x => x.ID == key);
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
            var rule = await _context.AwardRules.FindAsync(key);
            if (rule == null)
            {
                return NotFound();
            }

            // Check whether the rule is inuse or not
            var usedcnt = _context.AwardPoints.Where(p => p.MatchedRuleID == rule.ID).Count();
            if (usedcnt > 0)
                return BadRequest("Rule still in use");

            _context.AwardRules.Remove(rule);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
