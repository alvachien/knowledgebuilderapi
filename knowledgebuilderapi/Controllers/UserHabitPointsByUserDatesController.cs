using System.Linq;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using System;

namespace knowledgebuilderapi.Controllers
{
    //[Authorize]
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
            return this._context.UserHabitPointsByUserDates;
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

            String user = (String)parameters["User"];
            DateTime rdt = (DateTime)parameters["StartedDate"];

            var point = (from usrpoint in this._context.UserHabitPointsByUserDates
                         where usrpoint.TargetUser == user && usrpoint.RecordDate < rdt
                         select usrpoint.Point).Sum();

            return Ok(point);
        }
    }
}
