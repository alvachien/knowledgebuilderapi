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
    public class AwardPointReportsController : ODataController
    {
        private readonly kbdataContext _context;

        public AwardPointReportsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /AwardPointReports
        [EnableQuery]
        public IQueryable<AwardPointReport> Get()
        {
            return _context.AwardPointReports;
        }
    }
}
