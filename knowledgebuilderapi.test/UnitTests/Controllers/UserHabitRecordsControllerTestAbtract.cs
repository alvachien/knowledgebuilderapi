using knowledgebuilderapi.test.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace knowledgebuilderapi.test.unittest
{
    public class UserHabitRecordsControllerTestAbtract : IDisposable
    {
        protected SqliteDatabaseFixture fixture = null;

        public UserHabitRecordsControllerTestAbtract(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        public void Dispose()
        {
        }
    }
}
