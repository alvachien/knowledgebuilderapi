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
    public class AwardPointsController : ODataController
    {
        private readonly kbdataContext _context;

        public AwardPointsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /ExerciseItems
        /// <summary>
        /// Adds support for getting knowledges, for example:
        /// 
        /// GET /ExerciseItems
        /// GET /ExerciseItems?$filter=Name eq 'Windows 95'
        /// GET /ExerciseItems?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<AwardPoint> Get()
        {
            return _context.AwardPoints;
        }

        /// GET: /ExerciseItem(:id)
        /// <summary>
        /// Adds support for getting a exercise by key, for example:
        /// 
        /// GET /ExerciseItem(1)
        /// </summary>
        /// <param name="key">The key of the exercise item</param>
        /// <returns>The exercise item</returns>
        [EnableQuery]
        public SingleResult<AwardPoint> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.AwardPoints.Where(p => p.ID == key));
        }
    }
}
