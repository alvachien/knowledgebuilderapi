using knowledgebuilderapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace knowledgebuilderapi.test.unittest.Models
{
    [Collection("KBAPI_UnitTests#2")]
    public class UserHabitTest
    {
        [Fact]
        public void TestCase_UserHabitPoint()
        {
            var point = new UserHabitPoint();
            point.TargetUser = "Test";
            point.Comment = "Test";
            point.ID = 1;
            point.Point = 100;
            point.RecordDate = DateTime.Now;
            Assert.NotNull(point);
        }
    }
}
