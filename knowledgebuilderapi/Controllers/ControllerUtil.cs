using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace knowledgebuilderapi.Controllers
{
    public class ControllerUtil
    {
        internal static String GetUserID(ControllerBase ctrl)
        {
            return ctrl.User?.Identity?.Name;
            //return ctrl.User != null ? ctrl.User.FindFirst(ClaimTypes.NameIdentifier)?.Value : "";
        }
    }
}
