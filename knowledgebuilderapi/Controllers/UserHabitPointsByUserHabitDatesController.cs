using System.Linq;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using knowledgebuilderapi.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace knowledgebuilderapi.Controllers
{
    [Authorize]
    public class UserHabitPointsByUserHabitDatesController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitPointsByUserHabitDatesController(kbdataContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IQueryable<UserHabitPointsByUserHabitDate> Get()
        {
            String usrId = ControllerUtil.GetUserID(this);
            if (String.IsNullOrEmpty(usrId))
                throw new Exception("Failed ID");

            return from point in _context.UserHabitPointsByUserHabitDates
                   join au in _context.AwardUsers
                       on new { point.TargetUser } equals new { au.TargetUser }
                   where au.Supervisor == usrId
                   select point;
        }
    }
}
