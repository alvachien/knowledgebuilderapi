using System.Linq;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using System;
using Microsoft.AspNetCore.Authorization;

namespace knowledgebuilderapi.Controllers
{
    [Authorize]
    public class UserHabitPointsByUserDatesController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitPointsByUserDatesController(kbdataContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IQueryable<UserHabitPointsByUserDate> Get()
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            return from point in _context.UserHabitPointsByUserDates
                   join auser in _context.AwardUsers
                    on point.TargetUser equals auser.TargetUser
                   where auser.Supervisor == usrId
                   select point;
        }

        [HttpPost]
        public IActionResult GetOpeningPoint([FromBody] ODataActionParameters parameters)
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

            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            String user = (String)parameters["User"];
            Int32 daysBackTo = (Int32)parameters["DaysBackTo"];
            DateTime dt = DateTime.Now;
            TimeSpan ts = new TimeSpan(daysBackTo, 0, 0, 0);
            dt = dt.Subtract(ts);

            var rst = (from au in _context.AwardUsers
                       where au.TargetUser == user
                         && au.Supervisor == usrId
                       select au).Count();
            if (rst != 1)
                throw new Exception("Invalid user data");

            var point = (from usrpoint in this._context.UserHabitPointsByUserDates
                         where usrpoint.TargetUser == user && usrpoint.RecordDate < dt
                         select usrpoint.Point).Sum();

            return Ok(point);
        }
    }
}
