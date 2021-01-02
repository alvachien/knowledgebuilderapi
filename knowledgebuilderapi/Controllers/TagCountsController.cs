using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace knowledgebuilderapi.Controllers
{
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
