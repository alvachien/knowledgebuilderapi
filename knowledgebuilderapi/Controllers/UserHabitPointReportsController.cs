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
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace knowledgebuilderapi.Controllers
{
    [Authorize]
    public class UserHabitPointReportsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitPointReportsController(kbdataContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IQueryable<UserHabitPointReport> Get()
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            return from report in this._context.UserHabitPointReports
                   join auser in _context.AwardUsers
                    on report.TargetUser equals auser.TargetUser
                   where auser.Supervisor == usrId // || auser.TargetUser == usrId
                   select report;
        }
    }
}
