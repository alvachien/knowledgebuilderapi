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
            return this._context.UserHabitPointReports;
        }
    }
}
