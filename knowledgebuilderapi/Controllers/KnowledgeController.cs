using System;
using Microsoft.AspNet.OData;
using Microsoft.EntityFrameworkCore;
using knowledgebuilderapi.Models;
using System.Linq;

namespace knowledgebuilderapi.Controllers {
    public class KnowledgeController : ODataController {
        private readonly kbdataContext _context;

        public KnowledgeController(kbdataContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IQueryable<Knowledge> Get()
        {
            return _context.Knowledges;
        }
    }
}