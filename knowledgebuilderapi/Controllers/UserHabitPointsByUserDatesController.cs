using System.Linq;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using knowledgebuilderapi.Models;

namespace knowledgebuilderapi.Controllers
{
    //[Authorize]
    public class UserHabitPointsByUserDatesController : ODataController
    {
        private readonly kbdataContext _context;

        public UserHabitPointsByUserDatesController(kbdataContext context)
        {
            _context = context;
        }

        [EnableQuery]
        public IQueryable<UserHabitPointsByUserDate> Get()
        {
            return this._context.UserHabitPointsByUserDates;
        }
    }
}
