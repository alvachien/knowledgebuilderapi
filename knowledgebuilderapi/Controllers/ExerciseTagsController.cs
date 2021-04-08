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
    public class ExerciseTagsController : ODataController
    {
        private readonly kbdataContext _context;

        public ExerciseTagsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /ExerciseTags
        /// <summary>
        /// Adds support for getting tags
        /// <remarks>
        [EnableQuery]
        public IQueryable<ExerciseTag> Get()
        {
            return _context.ExerciseTags;
        }
    }
}
