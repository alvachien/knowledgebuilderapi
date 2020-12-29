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
