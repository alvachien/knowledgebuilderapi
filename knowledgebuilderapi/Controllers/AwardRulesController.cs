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
    public class AwardRulesController : ODataController
    {
        private readonly kbdataContext _context;

        public AwardRulesController(kbdataContext context)
        {
            _context = context;
        }

        // GET: /AwardRules
        [EnableQuery]
        public IQueryable<AwardRule> Get()
        {
            return _context.AwardRules;
        }

        // GET: /AwardRules(:id)
        [EnableQuery]
        public SingleResult<AwardRule> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.AwardRules.Where(p => p.ID == key));
        }
    }
}
