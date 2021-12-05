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
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            return from habit in _context.UserHabits
                   join auser in _context.AwardUsers
                    on habit.TargetUser equals auser.TargetUser
                   where auser.Supervisor == usrId
                   select habit;
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
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            return SingleResult.Create(from habit in _context.UserHabits
                                       join auser in _context.AwardUsers
                                        on habit.TargetUser equals auser.TargetUser
                                       where auser.Supervisor == usrId && habit.ID == key
                                       select habit);
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

            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            // Check 0. User
            var ucnt = _context.AwardUsers.Where(p => p.TargetUser == habit.TargetUser && p.Supervisor == usrId).Count();
            if (ucnt != 1)
                return BadRequest("Invalid user");

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
                            if (!Enum.IsDefined(typeof(DayOfWeek), dow))
                                throw new Exception("Invalid start date");
                        }
                        catch(Exception exp)
                        {
                            return BadRequest(exp.Message);
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

            // Check 2. Rules
            if (habit.Rules.Count < 0)
                return BadRequest("Habit must define with rules");
            // habit.Rules.Sort(prop => prop.);

            int i = 1;
            foreach(var rule in habit.Rules)
            {
                rule.RuleID = i++;
                rule.CurrentHabit = habit;
            }

            // Update db
            _context.UserHabits.Add(habit);
            await _context.SaveChangesAsync();

            return Created(habit);
        }

        public async Task<ActionResult> Delete([FromODataUri]int key)
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            // Check whether the habit already in using
            var recordcnt = _context.UserHabitRecords.Where(p => p.HabitID == key).Count();
            if (recordcnt > 0)
            {
                return BadRequest("Habit is in using");
            }

            var habititem = await _context.UserHabits.FindAsync(key);
            if (habititem == null)
            {
                return NotFound();
            }

            var ucnt = _context.AwardUsers.Where(p => p.TargetUser == habititem.TargetUser && p.Supervisor == usrId).Count();
            if (ucnt != 1)
                return BadRequest("Invalid user");

            _context.UserHabits.Remove(habititem);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }
    }
}
