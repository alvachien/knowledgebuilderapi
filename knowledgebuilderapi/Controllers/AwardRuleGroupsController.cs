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
    public class AwardRuleGroupsController : ODataController
    {
        private readonly kbdataContext _context;

        public AwardRuleGroupsController(kbdataContext context)
        {
            _context = context;
        }

        // GET: /AwardRules
        [EnableQuery]
        public IQueryable<AwardRuleGroup> Get()
        {
            return _context.AwardRuleGroups;
        }

        // GET: /AwardRules(:id)
        [EnableQuery]
        public SingleResult<AwardRuleGroup> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.AwardRuleGroups.Where(p => p.ID == key));
        }

        // POST: /AwardUserGroups
        /// <summary>
        /// Support for creating user group
        /// </summary>
        public async Task<IActionResult> Post([FromBody] AwardRuleGroup item)
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

            _context.AwardRuleGroups.Add(item);
            await _context.SaveChangesAsync();

            return Created(item);
        }

        // DELETE: /AwardRuleGroup(5)
        /// <summary>
        /// Support for deleting award user gorup
        /// </summary>
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var coll = await _context.AwardRuleGroups.FindAsync(key);
            if (coll == null)
            {
                return NotFound();
            }

            _context.AwardRuleGroups.Remove(coll);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
