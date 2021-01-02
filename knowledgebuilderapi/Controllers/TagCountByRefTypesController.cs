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
    public class TagCountByRefTypesController : ODataController
    {
        private readonly kbdataContext _context;

        public TagCountByRefTypesController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /TagCountBys
        /// <summary>
        /// Adds support for getting tags
        /// <remarks>
        [EnableQuery]
        public IQueryable<Tag> Get()
        {
            return _context.TagCountByRefTypes;
        }
    }
}
