using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using knowledgebuilderapi.Models;

namespace knowledgebuilderapi.Controllers
{
    public class ExerciseItemAnswersController : ODataController
    {
        private readonly kbdataContext _context;

        public ExerciseItemAnswersController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /Knowledges
        /// <summary>
        /// Adds support for getting knowledges, for example:
        /// 
        /// GET /Knowledges
        /// GET /Knowledges?$filter=Name eq 'Windows 95'
        /// GET /Knowledges?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<ExerciseItemAnswer> Get()
        {
            return _context.ExerciseItemAnswers;
        }
    }
}

