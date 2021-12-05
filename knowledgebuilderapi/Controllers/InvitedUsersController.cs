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
    public class InvitedUsersController : ODataController
    {
        private readonly kbdataContext _context;

        public InvitedUsersController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /InvitedUsers
        [EnableQuery]
        public IQueryable<InvitedUser> Get()
        {
            String usrName = String.Empty;
            try
            {
                usrName = ControllerUtil.GetUserID(this);
                if (String.IsNullOrEmpty(usrName))
                    throw new UnauthorizedAccessException();
            }
            catch
            {
                throw new UnauthorizedAccessException();
            }

            return _context.InvitedUsers.Where(p => p.UserID == usrName);
        }

        //[HttpPost]
        //public IActionResult ValidInvitationCode([FromBody] ODataActionParameters parameters)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        foreach (var value in ModelState.Values)
        //        {
        //            foreach (var err in value.Errors)
        //            {
        //                System.Diagnostics.Debug.WriteLine(err.Exception?.Message);
        //            }
        //        }

        //        return BadRequest(ModelState);
        //    }

        //    String code = (String)parameters["InvitationCode"];

        //    var user = this._context.InvitedUsers.FirstOrDefault(p => p.InvitationCode == code);

        //    if (user == null)
        //        return NotFound();

        //    user.LastLoginAt = DateTime.Now;
        //    _context.SaveChanges();

        //    return Ok(user);
        //}
    }
}
