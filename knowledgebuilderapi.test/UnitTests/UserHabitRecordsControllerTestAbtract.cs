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
        //private List<Int32> objectsCreated = new List<Int32>();
        protected const string test_manager = "TestManager_1";
        protected const string test_user1 = "Tester_1";

        public UserHabitRecordsControllerTestAbtract(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture;

            this.createUser();
        }

        public void Dispose()
        {
            this.clearUser();
        }

        private void createUser()
        {
            // Create invite user
            var context = this.fixture.GetCurrentDataContext();

            DataSetupUtility.CreateInviteUser(context, test_manager, test_user1);
            context.SaveChanges();
        }

        private void clearUser()
        {
            var context = this.fixture.GetCurrentDataContext();
            DataSetupUtility.DeleteInviteUser(context, test_manager, test_user1);
            context.SaveChanges();
            context.Dispose();
        }
    }
}
