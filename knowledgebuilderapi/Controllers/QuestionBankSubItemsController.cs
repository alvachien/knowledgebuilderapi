using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using knowledgebuilderapi.Models;

namespace knowledgebuilderapi.Controllers
{
    public class QuestionBankSubItemsController : ODataController
    {
        private readonly kbdataContext _context;

        public QuestionBankSubItemsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /QuestionBankSubItem
        /// <summary>
        /// Adds support for getting knowledges, for example:
        /// 
        /// GET /QuestionBankSubItem
        /// GET /QuestionBankSubItem?$filter=Name eq 'Windows 95'
        /// GET /QuestionBankSubItem?
        /// 
        /// <remarks>
        [EnableQuery]
        public IQueryable<QuestionBankSubItem> Get()
        {
            return _context.QuestionBankSubItems;
        }
    }
}

