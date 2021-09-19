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
    public class UserCollectionItemsController : ODataController
    {
        private readonly kbdataContext _context;

        public UserCollectionItemsController(kbdataContext context)
        {
            _context = context;
        }

        // GET: /UserCollectionItems
        [EnableQuery]
        public IQueryable<UserCollectionItem> Get()
        {
            return _context.UserCollectionItems;
        }

        // GET: /UserCollectionItems(:id)
        [EnableQuery]
        public SingleResult<UserCollectionItem> Get([FromODataUri] int key)
        {
            return SingleResult.Create(_context.UserCollectionItems.Where(p => p.ID == key));
        }
    }
}
