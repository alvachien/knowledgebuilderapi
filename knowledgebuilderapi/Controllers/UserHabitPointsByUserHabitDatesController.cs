using System.Linq;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using knowledgebuilderapi.Models;

namespace knowledgebuilderapi.Controllers
{
    //[Authorize]
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
            return this._context.UserHabitPointsByUserHabitDates;
        }
    }
}
