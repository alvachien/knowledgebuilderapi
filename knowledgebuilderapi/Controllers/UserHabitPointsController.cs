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
    }
}
