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
            return _context.UserHabitPoints;
        }

        ///// GET: /AwardPoints(:id)
        //[EnableQuery]
        //public SingleResult<UserHabitPoint> Get([FromODataUri] int key)
        //{
        //    String usrId = ControllerUtil.GetUserID(this);
        //    if (String.IsNullOrEmpty(usrId))
        //        throw new Exception("Failed ID");

        //    return SingleResult.Create(from au in _context.AwardUsers
        //                               join ap in _context.AwardPoints
        //                               on au.TargetUser equals ap.TargetUser
        //                               where au.Supervisor == usrId
        //                                && ap.ID == key
        //                               select ap);
        //}

        // POST: /UserHabitRecords
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

        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var point = await _context.UserHabitPoints.FindAsync(key);
            if (point == null)
            {
                return NotFound();
            }

            //String usrId = ControllerUtil.GetUserID(this);
            //if (String.IsNullOrEmpty(usrId))
            //    throw new Exception("Failed ID");
            //var rst = (from au in _context.AwardUsers
            //           where au.TargetUser == point.TargetUser
            //             && au.Supervisor == usrId
            //           select au).Count();
            //if (rst != 1)
            //    throw new Exception("Invalid user data");

            _context.UserHabitPoints.Remove(point);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
