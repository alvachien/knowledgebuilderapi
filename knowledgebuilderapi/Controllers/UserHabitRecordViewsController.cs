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
    public class UserHabitRecordViewsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitRecordViewsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /UserHabitRecordViews
        /// <summary>
        /// Adds support for getting User Habit Rules, for example:
        /// 
        /// GET /UserHabitRecordViews
        /// GET /UserHabitRecordViews?$filter=Name eq 'Windows 95'
        /// GET /UserHabitRecordViews?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<UserHabitRecordView> Get()
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            var resultInterms = from record in _context.UserHabitRecords
                          join habit in _context.UserHabits
                            on record.HabitID equals habit.ID
                          join auser in _context.AwardUsers
                            on habit.TargetUser equals auser.TargetUser
                          where auser.TargetUser != null
                          select new
                          {
                              HabitID = record.HabitID,
                              Comment = record.Comment,
                              CompleteFact = record.CompleteFact,
                              RecordDate = record.RecordDate,
                              ContinuousCount = record.ContinuousCount,
                              RuleID = record.RuleID,
                              SubID = record.SubID,
                              TargetUser = habit.TargetUser,
                              HabitName = habit.Name,
                              HabitValidFrom = habit.ValidFrom,
                              HabitValidTo = habit.ValidTo
                          };

            var results = from intrst in resultInterms
                          join rule in _context.UserHabitRules
                            on new { HabitID = intrst.HabitID, RuleID = intrst.RuleID.GetValueOrDefault() } equals new { HabitID = rule.HabitID, RuleID = rule.RuleID }
                          into ps 
                          from p in ps.DefaultIfEmpty()
                          select new UserHabitRecordView
                          {
                              HabitID = intrst.HabitID,
                              Comment = intrst.Comment,
                              CompleteFact = intrst.CompleteFact,
                              RecordDate = intrst.RecordDate,
                              ContinuousCount = intrst.ContinuousCount,
                              RuleID = intrst.RuleID,
                              SubID = intrst.SubID,
                              TargetUser = intrst.TargetUser,
                              HabitName = intrst.HabitName,
                              HabitValidFrom = intrst.HabitValidFrom,
                              HabitValidTo = intrst.HabitValidTo,
                              RuleDaysFrom = p.ContinuousRecordFrom,
                              RuleDaysTo = p.ContinuousRecordTo,
                              RulePoint = p.Point,
                          };

            return results;
        }
    }
}
