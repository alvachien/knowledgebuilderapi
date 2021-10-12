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
    public class TagCountsController : ODataController
    {
        private readonly kbdataContext _context;

        public TagCountsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /TagCounts
        /// <summary>
        /// Adds support for getting tags
        /// <remarks>
        [EnableQuery]
        public IQueryable<TagCount> Get()
        {
            return _context.TagCounts;
        }
    }
}
