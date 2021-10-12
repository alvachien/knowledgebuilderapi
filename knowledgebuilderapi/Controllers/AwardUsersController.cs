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
    public class AwardUsersController : ODataController
    {
        private readonly kbdataContext _context;

        public AwardUsersController(kbdataContext context)
        {
            _context = context;
        }

        // GET: /AwardUsers
        [EnableQuery]
        public IQueryable<AwardUser> Get()
        {
            return _context.AwardUsers;
        }

        //// [EnableQuery]
        //public SingleResult<AwardUser> Get([FromODataUri] String keyTargetUser, String keySupervior)
        //{
        //    return SingleResult.Create(_context.AwardUsers.Where(p => p.TargetUser == keyTargetUser && p.Supervisor == keySupervior));
        //}

        // POST: /AwardUsers
        /// <summary>
        /// Support for creating award user
        /// </summary>
        public async Task<IActionResult> Post([FromODataBody] AwardUser item)
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

            _context.AwardUsers.Add(item);
            await _context.SaveChangesAsync();

            return Created(item);
        }

        // DELETE: /AwardUsers(5)
        /// <summary>
        /// Support for deleting award user gorup
        /// </summary>
        public async Task<IActionResult> Delete([FromODataUri] String keyTargetUser, [FromODataUri] String keySupervior)
        {
            var usr = await _context.AwardUsers.FindAsync(keyTargetUser, keySupervior);
            if (usr == null)
            {
                return NotFound();
            }

            _context.AwardUsers.Remove(usr);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
