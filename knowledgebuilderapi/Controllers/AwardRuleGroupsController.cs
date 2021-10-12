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
using Microsoft.AspNetCore.Authorization;

namespace knowledgebuilderapi.Controllers
{
    [Authorize]
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

            // Check there is no overlap
            var grpList = (from grp in _context.AwardRuleGroups
                           where grp.TargetUser == item.TargetUser
                             && grp.RuleType == item.RuleType
                           // orderby grp.ValidTo descending
                           select grp).ToList<AwardRuleGroup>();
            grpList.Add(item);
            grpList.Sort((a, b) => a.ValidTo.CompareTo(b.ValidTo));

            DateTime? dtCurBgn = null, dtCurEnd = null;
            foreach(var grp in grpList)
            {
                if (dtCurBgn == null)
                {
                    dtCurBgn = grp.ValidFrom;
                    dtCurEnd = grp.ValidTo;
                }
                else
                {
                    if ((grp.ValidTo >= dtCurBgn && grp.ValidFrom <= dtCurBgn)
                        || (grp.ValidTo >= dtCurEnd && grp.ValidFrom <= dtCurEnd))
                        return BadRequest("Invalid time");
                }
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
            // Check rule is inusing
            var dbgrp = await _context.AwardRuleGroups
                .Include(i => i.Rules)
                .SingleOrDefaultAsync(x => x.ID == key);
            if (dbgrp== null)
            {
                return NotFound();
            }

            var ruleUsedCnt = (from rule in dbgrp.Rules
                               join pnt in _context.AwardPoints
                               on rule.ID equals pnt.MatchedRuleID
                               select pnt).Count();
            if (ruleUsedCnt > 0)
                return BadRequest("Rule group is inuse");

            _context.AwardRuleGroups.Remove(dbgrp);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
