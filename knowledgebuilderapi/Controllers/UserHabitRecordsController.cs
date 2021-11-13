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
                    dtbgn = HabitWeeklyTrace.getDBSelectionDate((DayOfWeek)habits[0].StartDate, record.RecordDate);
                    break;

                case HabitFrequency.Monthly:
                    dtbgn = HabitMonthlyTrace.getDBSelectionDate(habits[0].StartDate.Value, record.RecordDate);
                    break;

                case HabitFrequency.Daily:
                default:
                    dtbgn = record.RecordDate - TimeSpan.FromDays(1);
                    break;
            }

            var checkrecord = (from dbrecord in _context.UserHabitRecords where dbrecord.RecordDate >= record.RecordDate && dbrecord.HabitID == record.HabitID
                               select dbrecord).Count();
            if (checkrecord > 0)
                return BadRequest("Record in the past!");

            // Find out all rules
            var rules = (from rule in this._context.UserHabitRules where rule.HabitID == record.HabitID 
                         orderby rule.ContinuousRecordFrom 
                         ascending select rule).ToList<UserHabitRule>();
            if (rules.Count > 0) { }

            // Find related records
            var oldrecords = (from dbrecord in this._context.UserHabitRecords where dbrecord.RecordDate >= dtbgn && dbrecord.RecordDate < record.RecordDate
                           select dbrecord).ToList<UserHabitRecord>();

            record.ContinuousCount = 1; // Default is 1

            // Now calculate the rule and the points
            switch (habits[0].Frequency)
            {
                case HabitFrequency.Weekly:
                    HabitWeeklyTrace firstWeek = new HabitWeeklyTrace();
                    HabitWeeklyTrace secondWeek = new HabitWeeklyTrace();
                    HabitWeeklyTrace.analyzeUserRecord(oldrecords, dtbgn, firstWeek, secondWeek);
                    // First week
                    int? firstweekrule = firstWeek.getRuleID();
                    int firstweekcontcnt = 0;
                    //int? firstweekFact = firstWeek.getCompleteFact();
                    if (firstweekrule.HasValue)
                        firstweekcontcnt = firstWeek.getRuleContinuousCount().GetValueOrDefault();                    

                    // Second week
                    int? secondweekrule = secondWeek.getRuleID();
                    if (firstweekrule.HasValue)
                    {
                        // Start since last week
                        switch (habits[0].CompleteCategory)
                        {
                            case HabitCompleteCategory.NumberOfCount:
                                break;

                            case HabitCompleteCategory.NumberOfTimes:
                            default:
                                {
                                    int nexistcnt = secondWeek.getNumberOfTimes();
                                    if (secondweekrule.HasValue)
                                    {
                                        // Already has rule assigned, move the rule ID to new created one
                                        var existRecord = secondWeek.getRecordWithRule();
                                        var existDBRecord = _context.UserHabitRecords
                                            .SingleOrDefaultAsync(x => x.HabitID == existRecord.HabitID && x.RecordDate == existRecord.RecordDate && x.SubID == existRecord.SubID);

                                        record.RuleID = existDBRecord.Result.RuleID;
                                        record.ContinuousCount = existDBRecord.Result.ContinuousCount;

                                        existDBRecord.Result.RuleID = null;
                                        existDBRecord.Result.ContinuousCount = 0;
                                    }
                                    else
                                    {
                                        if (nexistcnt + 1 == habits[0].CompleteCondition)
                                        {
                                            // Workout the rule then
                                            var ncontcnt = firstweekcontcnt + 1;
                                            var ridx = rules.FindIndex(ruleitem => ncontcnt >= ruleitem.ContinuousRecordFrom && ruleitem.ContinuousRecordTo > ncontcnt );
                                            if (ridx != -1)
                                            {
                                                record.ContinuousCount = ncontcnt;
                                                record.RuleID = rules[ridx].RuleID;
                                            }                                                
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        // New start in this week
                        switch(habits[0].CompleteCategory)
                        {
                            case HabitCompleteCategory.NumberOfCount:
                                break;

                            case HabitCompleteCategory.NumberOfTimes:
                            default:
                                {
                                    int nexistcnt = secondWeek.getNumberOfTimes();
                                    if (secondweekrule.HasValue)
                                    {
                                        // Already has rule assigned, move the rule ID to new created one
                                        var existRecord = secondWeek.getRecordWithRule();
                                        var existDBRecord = _context.UserHabitRecords
                                            .SingleOrDefaultAsync(x => x.HabitID == existRecord.HabitID && x.RecordDate == existRecord.RecordDate && x.SubID == existRecord.SubID);

                                        record.RuleID = existDBRecord.Result.RuleID;
                                        record.ContinuousCount = existDBRecord.Result.ContinuousCount;

                                        existDBRecord.Result.RuleID = null;
                                        existDBRecord.Result.ContinuousCount = 0;
                                    }
                                    else
                                    {
                                        if (nexistcnt + 1 == habits[0].CompleteCondition)
                                        {
                                            // Workout the rule then
                                            var ridx = rules.FindIndex(ruleitem => record.ContinuousCount >= ruleitem.ContinuousRecordFrom && record.ContinuousCount < ruleitem.ContinuousRecordTo );
                                            if (ridx != -1)
                                                record.RuleID = rules[ridx].RuleID;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;

                case HabitFrequency.Monthly:
                    HabitMonthlyTrace firstMonth = new HabitMonthlyTrace();
                    HabitMonthlyTrace secondMonth = new HabitMonthlyTrace();
                    HabitMonthlyTrace.analyzeUserRecord(oldrecords, dtbgn, firstMonth, secondMonth);
                    // First month
                    // Second month
                    break;

                case HabitFrequency.Daily:
                default:
                    if (oldrecords.Count > 0)
                    {
                        // Yesterday
                        if (oldrecords[0].RuleID.HasValue)
                        {
                            // Rule assigned
                            var oldruleidx = rules.FindIndex(ruleitem => ruleitem.RuleID == oldrecords[0].RuleID);
                            if (oldruleidx != -1)
                            {
                                record.ContinuousCount = oldrecords[0].ContinuousCount + 1;

                                var ridx = rules.FindIndex(ruleitem => ruleitem.ContinuousRecordFrom >= record.ContinuousCount);
                                if (ridx != -1)
                                    record.RuleID = rules[ridx].RuleID;
                            }
                        }
                        else
                        {
                            // No rule assigned yesterday, new activity
                            var ridx = rules.FindIndex(ruleitem => ruleitem.ContinuousRecordFrom == 1);
                            if (ridx != -1)
                                record.RuleID = rules[ridx].RuleID;
                        }
                    } 
                    else
                    {
                        // New activity
                        var ridx = rules.FindIndex(ruleitem => ruleitem.ContinuousRecordFrom == 1);
                        if (ridx != -1)
                            record.RuleID = rules[ridx].RuleID;
                    }
                    break;
            }

            // Update db
            _context.UserHabitRecords.Add(record);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception exp)
            {
                System.Console.WriteLine(exp.Message);
            }

            return Created(record);
        }
    }
}
