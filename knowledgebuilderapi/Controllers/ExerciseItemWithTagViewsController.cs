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
    public class ExerciseItemWithTagViewsController : ODataController
    {
        private readonly kbdataContext _context;

        public ExerciseItemWithTagViewsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /ExerciseItemWithTagViews
        /// <summary>
        /// Adds support for getting tags
        /// <remarks>
        [EnableQuery]
        public IQueryable<ExerciseItemWithTagView> Get()
        {
            return _context.ExerciseItemWithTagViews;
        }
    }
}
