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
    public class UserHabitsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /UserHabits
        /// <summary>
        /// Adds support for getting User Habits, for example:
        /// 
        /// GET /UserHabits
        /// GET /UserHabits?$filter=Name eq 'Windows 95'
        /// GET /UserHabits?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<UserHabit> Get()
        {
            return _context.UserHabits;
        }

        /// GET: /UserHabits(:id)
        /// <summary>
        /// Adds support for getting an user habit by key, for example:
        /// 
        /// GET /ExerciseItem(1)
        /// </summary>
        /// <param name="key">The key of the user habit</param>
        /// <returns>The user habit</returns>
        [EnableQuery]
        public SingleResult<UserHabit> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.UserHabits.Where(p => p.ID == key));
        }

        // POST: /UserHabits
        /// <summary>
        /// Support for creating user habit
        /// </summary>
        public async Task<IActionResult> Post([FromBody] UserHabit habit)
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

            // Check 1. Validity
            if (habit.ValidTo <= habit.ValidFrom)
                return BadRequest("Invalid Validity");
            switch (habit.Frequency)
            {
                case HabitFrequency.Weekly:
                    if (!habit.StartDate.HasValue)
                        return BadRequest("Invalid start date");
                    else
                    {
                        try
                        {
                            DayOfWeek dow = (DayOfWeek)habit.StartDate.Value;
                        }
                        catch(Exception exp)
                        {
                            return BadRequest("Invalid start date");
                        }

                        switch (habit.CompleteCategory)
                        {
                            case HabitCompleteCategory.NumberOfCount:
                                if (habit.CompleteCondition <= 0)
                                    return BadRequest("Invalid done criteria");
                                break;

                            case HabitCompleteCategory.NumberOfTimes:
                            default:
                                if (habit.CompleteCondition> 7 || habit.CompleteCondition <= 0)
                                    return BadRequest("Invalid done criteria");
                                break;
                        }
                    }
                    break;

                case HabitFrequency.Monthly:
                    if (!habit.StartDate.HasValue)
                        return BadRequest("Invalid start date");
                    else
                    {
                        if (habit.StartDate.Value > 28 || habit.StartDate < 1)
                            return BadRequest("Invalid start date");

                        switch (habit.CompleteCategory)
                        {
                            case HabitCompleteCategory.NumberOfCount:
                                if (habit.CompleteCondition <= 0)
                                    return BadRequest("Invalid done criteria");
                                break;

                            case HabitCompleteCategory.NumberOfTimes:
                            default:
                                if (habit.CompleteCondition > 28 || habit.CompleteCondition <= 0)
                                    return BadRequest("Invalid done criteria");
                                break;
                        }
                    }
                    break;

                case HabitFrequency.Daily:
                default:
                    if (habit.StartDate.HasValue)
                        return BadRequest("Invalid start date");
                    switch (habit.CompleteCategory)
                    {
                        case HabitCompleteCategory.NumberOfCount:
                            if (habit.CompleteCondition <= 0)
                                return BadRequest("Invalid done criteria");
                            break;

                        case HabitCompleteCategory.NumberOfTimes:
                        default:
                            if (habit.CompleteCondition != 1)
                                return BadRequest("Invalid done criteria");
                            break;
                    }
                    break;
            }

            // Update db
            _context.UserHabits.Add(habit);
            await _context.SaveChangesAsync();

            return Created(habit);
        }
    }
}
