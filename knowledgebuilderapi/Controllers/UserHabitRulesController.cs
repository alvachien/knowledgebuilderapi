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
    public class UserHabitRulesController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitRulesController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /UserHabitRules
        /// <summary>
        /// Adds support for getting User Habit Rules, for example:
        /// 
        /// GET /UserHabitRules
        /// GET /UserHabitRules?$filter=Name eq 'Windows 95'
        /// GET /UserHabitRules?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<UserHabitRule> Get()
        {
            return _context.UserHabitRules;
        }
    }
}
