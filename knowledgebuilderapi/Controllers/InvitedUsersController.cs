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
    public class InvitedUsersController : ODataController
    {
        private readonly kbdataContext _context;

        public InvitedUsersController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /AwardPoints
        //[EnableQuery]
        //public IQueryable<InvitedUser> Get()
        //{
        //    return _context.InvitedUsers;
        //}

        [HttpPost]
        public IActionResult ValidInvitationCode([FromBody] ODataActionParameters parameters)
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

                return BadRequest(ModelState);
            }

            String code = (String)parameters["InvitationCode"];

            var user = this._context.InvitedUsers.FirstOrDefault(p => p.InvitationCode == code);

            if (user == null)
                return NotFound();

            user.LastLoginAt = DateTime.Now;
            _context.SaveChanges();

            return Ok(user);
        }
    }
}
