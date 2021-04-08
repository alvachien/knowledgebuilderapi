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

namespace knowledgebuilderapi.Controllers
{
    public class OverviewInfosController : ODataController
    {
        private readonly kbdataContext _context;

        public OverviewInfosController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /OverviewInfos
        /// <summary>
        /// Adds support for getting overview info
        /// <remarks>
        [EnableQuery]
        public IQueryable<OverviewInfo> Get()
        {
            return _context.OverviewInfos;
        }
    }
}
