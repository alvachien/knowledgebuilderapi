using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using knowledgebuilderapi.Models;

namespace knowledgebuilderapi.Controllers
{
    public class ExerciseItemsController : ODataController
    {
        private readonly kbdataContext _context;

        public ExerciseItemsController(kbdataContext context)
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
        public IQueryable<ExerciseItem> Get()
        {
            return _context.ExerciseItems;
        }
    }
}
