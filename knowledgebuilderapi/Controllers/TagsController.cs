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
    public class TagsController : ODataController
    {
        private readonly kbdataContext _context;

        public TagsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /Tags
        /// <summary>
        /// Adds support for getting tags
        /// <remarks>
        [EnableQuery]
        public IQueryable<Tag> Get()
        {
            return _context.Tags;
        }
    }
}
