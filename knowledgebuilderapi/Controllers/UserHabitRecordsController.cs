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
    public class UserHabitRecordsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitRecordsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /UserHabitRecords
        /// <summary>
        /// Adds support for getting User Habit Rules, for example:
        /// 
        /// GET /UserHabitRecords
        /// GET /UserHabitRecords?$filter=Name eq 'Windows 95'
        /// GET /UserHabitRecords?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<UserHabitRecord> Get()
        {
            return _context.UserHabitRecords;
        }

        // POST: /UserHabitRecords
        /// <summary>
        /// Support for creating user habit record
        /// </summary>
        public async Task<IActionResult> Post([FromBody] UserHabitRecord record)
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

            // Find out the matched rule
            var habits = (from habit in this._context.UserHabits where habit.ID == record.HabitID select habit).ToList<UserHabit>();
            if (habits.Count != 1)
                return BadRequest("Invalid Habit");
            // Date range for checking
            if (record.RecordDate >= habits[0].ValidTo || record.RecordDate < habits[0].ValidFrom)
                return BadRequest("Invalid time");

            DateTime dtbgn = record.RecordDate;
            switch(habits[0].Frequency)
            {
                case HabitFrequency.Weekly:
                    break;

                case HabitFrequency.Monthly:
                    break;

                case HabitFrequency.Daily:
                default:
                    break;
            }

            // Find out the rules
            var rules = (from rule in this._context.UserHabitRules where rule.HabitID == record.HabitID select rule).ToList<UserHabitRule>();
            if (rules.Count > 0)
            {
            }

            // Update db
            _context.UserHabitRecords.Add(record);
            await _context.SaveChangesAsync();

            return Created(record);
        }
    }
}
