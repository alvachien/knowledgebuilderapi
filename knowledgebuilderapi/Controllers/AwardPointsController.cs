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

        /// GET: /AwardPoints
        [EnableQuery]
        public IQueryable<AwardPoint> Get()
        {
            return _context.AwardPoints;
        }

        /// GET: /AwardPoints(:id)
        [EnableQuery]
        public SingleResult<AwardPoint> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.AwardPoints.Where(p => p.ID == key));
        }
    }
}
