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
    public class UserHabitPointsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitPointsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /UserHabitPoints
        /// <summary>
        /// Adds support for getting User Habit Points, for example:
        /// 
        /// GET /UserHabitPoints
        /// GET /UserHabitPoints?$filter=Name eq 'Windows 95'
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<UserHabitPoint> Get()
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            return from point in _context.UserHabitPoints
                    join au in _context.AwardUsers
                        on new { point.TargetUser } equals new { au.TargetUser }
                    where au.Supervisor == usrId
                    select point;
        }

        /// GET: /UserHabitPoints(:id)
        [EnableQuery]
        public SingleResult<UserHabitPoint> Get([FromODataUri] int key)
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            return SingleResult.Create(from point in _context.UserHabitPoints
                                       join au in _context.AwardUsers
                                           on new { point.TargetUser } equals new { au.TargetUser }
                                       where au.Supervisor == usrId && point.ID == key
                                       select point );
        }

        // POST: /UserHabitPoints
        /// <summary>
        /// Support for creating user habit record
        /// </summary>
        public async Task<IActionResult> Post([FromBody] UserHabitPoint point)
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
            var rst = (from au in _context.AwardUsers
                       where au.TargetUser == point.TargetUser
                         && au.Supervisor == usrId
                       select au).Count();
            if (rst != 1)
                throw new Exception("Invalid user data");

            // Update db
            _context.UserHabitPoints.Add(point);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message);
                throw;
            }

            return Created(point);
        }

        // DELETE: UserHabitPoints(id)
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var point = await _context.UserHabitPoints.FindAsync(key);
            if (point == null)
            {
                return NotFound();
            }

            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");
            var rst = (from au in _context.AwardUsers
                       where au.TargetUser == point.TargetUser
                         && au.Supervisor == usrId
                       select au).Count();
            if (rst != 1)
                throw new Exception("Invalid user data");

            _context.UserHabitPoints.Remove(point);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
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
            Int32 daysBackTo = (Int32)parameters["DaysBackTo"];
            DateTime dt = DateTime.Now;
            TimeSpan ts = new TimeSpan(daysBackTo, 0, 0, 0);
            dt = dt.Subtract(ts);

            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");
            var rst = (from au in _context.AwardUsers
                       where au.TargetUser == user
                         && au.Supervisor == usrId
                       select au).Count();
            if (rst != 1)
                throw new Exception("Invalid user data");


            var point = (from usrpoint in this._context.UserHabitPoints
                          where usrpoint.TargetUser == user && usrpoint.RecordDate < dt                          
                          select usrpoint.Point).Sum();

            return Ok(point);
        }
    }
}
