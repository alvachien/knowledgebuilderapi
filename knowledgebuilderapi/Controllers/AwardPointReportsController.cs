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
    public class AwardPointReportsController : ODataController
    {
        private readonly kbdataContext _context;

        public AwardPointReportsController(kbdataContext context)
        {
            _context = context;
        }

        /// GET: /AwardPointReports
        [EnableQuery]
        public IQueryable<AwardPointReport> Get()
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            //return _context.AwardPointReports.Where(p => p.TargetUser ;
            return from au in _context.AwardUsers
                      join ap in _context.AwardPointReports
                      on au.TargetUser equals ap.TargetUser
                      where au.Supervisor == usrId
                      select ap;
        }
    }
}
