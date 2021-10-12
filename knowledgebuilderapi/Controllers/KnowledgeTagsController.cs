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
    public class KnowledgeTagsController : ODataController
    {
        private readonly kbdataContext _context;

        public KnowledgeTagsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /KnowledgeTags
        /// <summary>
        /// Adds support for getting knowledge tags
        /// <remarks>
        [EnableQuery]
        public IQueryable<KnowledgeTag> Get()
        {
            return _context.KnowledgeTags;
        }
    }
}
