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
    public class DailyTracesController : ODataController
    {
        private readonly kbdataContext _context;

        public DailyTracesController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /DailyTraces
        [EnableQuery]
        public IQueryable<DailyTrace> Get()
        {
            return _context.DailyTraces;
        }

        /// GET: /DailyTraces(:id)
        /// <summary>
        /// Adds support for getting a exercise by key, for example:
        /// 
        /// GET /ExerciseItem(1)
        /// </summary>
        /// <param name="key">The key of the exercise item</param>
        /// <returns>The exercise item</returns>
        //[EnableQuery]
        //public SingleResult<DailyTrace> Get([FromODataUri] int key)
        //{
        //    return SingleResult.Create(_context.DailyTraces.Where(p => p.ID == key));
        //}
    }
}
