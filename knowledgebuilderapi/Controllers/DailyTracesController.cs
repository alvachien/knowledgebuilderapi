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
    public class DailyTracesController : ODataController
    {
        private readonly kbdataContext _context;

        public DailyTracesController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /DailyTraces
        [EnableQuery]
        public IQueryable<DailyTrace> Get()
        {
            return _context.DailyTraces;
        }

        /// GET: /DailyTraces(:id)
        [EnableQuery]
        //[HttpGet("DailyTraces(TargetUser={tagetUsr},RecordDate={dtRecord})")]
        public SingleResult<DailyTrace> Get([FromODataUri] String keyTargetUser, [FromODataUri] DateTime keyRecordDate)
        {
            return SingleResult.Create(_context.DailyTraces.Where(p => p.TargetUser == keyTargetUser && p.RecordDate.Date == keyRecordDate.Date));
        }

        // POST: /DailyTraces
        [HttpPut]
        public async Task<IActionResult> Post([FromBody] DailyTrace dt)
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

                return BadRequest("Invalid model state");
            }

            var cnt = _context.DailyTraces.Where(p => p.TargetUser == dt.TargetUser && p.RecordDate.Date > dt.RecordDate.Date).Count();
            if (cnt > 0)
                return BadRequest("Cannot insert a trace on past");

            // Calculate the points
            List<AwardPoint> points = CalculatePoints(dt);

            // Update db
            _context.DailyTraces.Add(dt);
            if (points.Count > 0)
                _context.AwardPoints.AddRange(points);

            await _context.SaveChangesAsync();

            return Created(dt);
        }

        // PUT: /DailyTraces/5
        public async Task<IActionResult> Put([FromODataUri] String keyTargetUser, [FromODataUri] DateTime keyRecordDate, [FromBody] DailyTrace update)
        {
            return BadRequest();
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (update.TargetUser != keyTargetUser || update.RecordDate.Date != keyRecordDate.Date)
            //{
            //    return BadRequest("Key is not matched");
            //}

            //var existdbentry = await _context.DailyTraces
            //        .SingleOrDefaultAsync(x => x.RecordDate.Date == keyRecordDate.Date && x.TargetUser == keyTargetUser);
            //if (existdbentry == null)
            //{
            //    return NotFound();
            //}

            //existdbentry.UpdateData(update);

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    throw;
            //}

            //return Updated(update);
        }

        //[HttpDelete("DailyTraces(TargetUser={tagetUsr},RecordDate={dtRecord})")]
        public async Task<IActionResult> Delete([FromODataUri] String keyTargetUser, [FromODataUri] DateTime keyRecordDate)
        {
            var dt = await _context.DailyTraces.FindAsync(keyTargetUser, keyRecordDate);
            if (dt == null)
            {
                return NotFound();
            }

            _context.DailyTraces.Remove(dt);
            var points = _context.AwardPoints.Where(p => p.RecordDate.Date == keyRecordDate.Date && p.TargetUser == keyTargetUser && p.MatchedRuleID.HasValue);
            if (points.Count() > 0)
                _context.AwardPoints.RemoveRange(points);
            await _context.SaveChangesAsync();

            return StatusCode(204); // HttpStatusCode.NoContent
        }

        [HttpPost]
        public async Task<IActionResult> SimulatePoints([FromBody] ODataActionParameters parameters)
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

            DailyTrace dt = (DailyTrace)parameters["dt"];

            List<AwardPoint> points = CalculatePoints(dt);

            return Ok(points);
        }

        public List<AwardPoint> CalculatePoints(DailyTrace dt)
        {
            List<AwardPoint> points = new List<AwardPoint>();

            // Calculate the points
            // Step 1. Get all related rules and previous result.
            var prvdate = dt.RecordDate.Subtract(new TimeSpan(1, 0, 0, 0)).Date;
            var prvresults = this._context.AwardPoints.Where(p => p.TargetUser == dt.TargetUser && p.RecordDate.Date == prvdate).ToList<AwardPoint>();

            List<AwardRuleType> listTypes = new List<AwardRuleType>();
            if (dt.GoToBedTime.HasValue)
            {
                listTypes.Add(AwardRuleType.GoToBedTime);
            }
            if (dt.SchoolWorkTime.HasValue)
            {
                listTypes.Add(AwardRuleType.SchoolWorkTime);
            }
            if (dt.HandWriting.HasValue)
            {
                listTypes.Add(AwardRuleType.HandWritingHabit);
            }
            if (dt.HomeWorkCount.HasValue)
            {
                listTypes.Add(AwardRuleType.HomeWorkCount);
            }
            if (dt.HouseKeepingCount.HasValue)
            {
                listTypes.Add(AwardRuleType.HouseKeepingCount);
            }
            if (dt.PoliteBehavior.HasValue)
            {
                listTypes.Add(AwardRuleType.PoliteBehavior);
            }
            if (dt.ErrorsCollection.HasValue)
            {
                listTypes.Add(AwardRuleType.ErrorCollectionHabit);
            }
            if (dt.BodyExerciseCount.HasValue)
            {
                listTypes.Add(AwardRuleType.BodyExerciseCount);
            }
            if (dt.CleanDesk.HasValue)
            {
                listTypes.Add(AwardRuleType.CleanDeakHabit);
            }

            var allgrps = (from rtype in listTypes
                            join grp in _context.AwardRuleGroups
                           on new { RuleType = rtype, TargetUser = dt.TargetUser, IsValid = true } equals new { grp.RuleType, grp.TargetUser, IsValid = grp.ValidFrom.Date <= dt.RecordDate.Date && grp.ValidTo.Date >= dt.RecordDate.Date }
                           select grp).ToList<AwardRuleGroup>();

            // Step 2. Calculate the points per rule
            if (dt.GoToBedTime.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.GoToBedTime);
                var rules = (from grp in allgrps
                                join rul in _context.AwardRules
                                on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.GoToBedTime
                             select rul).ToList<AwardRule>();
                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.TimeStart <= dt.GoToBedTime.Value && p.TimeEnd >= dt.GoToBedTime.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new AwardPoint();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }
            if (dt.SchoolWorkTime.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.SchoolWorkTime);
                var rules = (from grp in allgrps
                             join rul in _context.AwardRules
                             on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.SchoolWorkTime
                             select rul).ToList<AwardRule>();
                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.TimeStart <= dt.SchoolWorkTime.Value && p.TimeEnd >= dt.SchoolWorkTime.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }
            if (dt.HandWriting.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.HandWritingHabit);
                var rules = (from grp in allgrps
                             join rul in _context.AwardRules
                             on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.HandWritingHabit
                             select rul).ToList<AwardRule>();

                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.DoneOfFact.Value == dt.HandWriting.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }
            if (dt.HomeWorkCount.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.HomeWorkCount);
                var rules = (from grp in allgrps
                             join rul in _context.AwardRules
                             on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.HomeWorkCount
                             select rul).ToList<AwardRule>();

                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.CountOfFactLow.Value <= dt.HomeWorkCount.Value && p.CountOfFactHigh.Value >= dt.HomeWorkCount.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }
            if (dt.HouseKeepingCount.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.HouseKeepingCount);
                var rules = (from grp in allgrps
                             join rul in _context.AwardRules
                             on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.HouseKeepingCount
                             select rul).ToList<AwardRule>();

                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.CountOfFactLow.Value <= dt.HouseKeepingCount.Value && p.CountOfFactHigh.Value >= dt.HouseKeepingCount.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }
            if (dt.PoliteBehavior.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.PoliteBehavior);
                var rules = (from grp in allgrps
                             join rul in _context.AwardRules
                             on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.PoliteBehavior
                             select rul).ToList<AwardRule>();

                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.CountOfFactLow.Value <= dt.PoliteBehavior.Value && p.CountOfFactHigh.Value >= dt.PoliteBehavior.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }
            if (dt.ErrorsCollection.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.ErrorCollectionHabit);
                var rules = (from grp in allgrps
                             join rul in _context.AwardRules
                             on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.ErrorCollectionHabit
                             select rul).ToList<AwardRule>();

                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.DoneOfFact.Value == dt.ErrorsCollection.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }
            if (dt.BodyExerciseCount.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.BodyExerciseCount);
                var rules = (from grp in allgrps
                             join rul in _context.AwardRules
                             on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.BodyExerciseCount
                             select rul).ToList<AwardRule>();

                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.CountOfFactLow.Value <= dt.BodyExerciseCount.Value && p.CountOfFactHigh.Value >= dt.BodyExerciseCount.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }
            if (dt.CleanDesk.HasValue)
            {
                // var rules = allrules.FindAll(p => p.RuleType == AwardRuleType.CleanDeakHabit);
                var rules = (from grp in allgrps
                             join rul in _context.AwardRules
                             on grp.ID equals rul.GroupID
                             where grp.RuleType == AwardRuleType.CleanDeakHabit
                             select rul).ToList<AwardRule>();

                if (rules.Count > 0)
                {
                    var matchedrules = rules.FindAll(p => p.DoneOfFact.Value == dt.CleanDesk.Value);
                    if (matchedrules.Count > 0)
                    {
                        // Shall be matched
                        AwardPoint pnt = new();

                        var countOfDays = 0;

                        // Appear in yesterday?
                        if (prvresults.Count > 0)
                        {
                            foreach (var rule in matchedrules)
                            {
                                var pntYesterday = prvresults.FirstOrDefault(p => p.MatchedRuleID != null && p.MatchedRuleID == rule.ID);
                                if (pntYesterday != null)
                                {
                                    if (pntYesterday.CountOfDay.HasValue)
                                        countOfDays = pntYesterday.CountOfDay.Value;
                                    break;
                                }
                            }
                        }
                        countOfDays++;

                        foreach (var rule in matchedrules)
                        {
                            if (rule.DaysFrom <= countOfDays && rule.DaysTo >= countOfDays)
                            {
                                pnt.CountOfDay = countOfDays;
                                pnt.MatchedRuleID = rule.ID;
                                pnt.RecordDate = dt.RecordDate.Date;
                                pnt.TargetUser = dt.TargetUser;
                                pnt.Point = rule.Point;
                                points.Add(pnt);
                            }
                        }
                    }
                }
            }

            return points;
        }
    }
}

