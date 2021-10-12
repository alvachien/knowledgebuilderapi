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
