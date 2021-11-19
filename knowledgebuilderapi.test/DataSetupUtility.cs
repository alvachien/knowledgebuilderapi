using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using knowledgebuilderapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace knowledgebuilderapi.test
{
    public sealed class DataSetupUtility
    {
        #region Create tables and Views
        public static void CreateDatabaseTables(DatabaseFacade database)
        {
            database.ExecuteSqlRaw(@"CREATE TABLE KnowledgeItem (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                ContentType SMALLINT       NULL,
                Title       NVARCHAR(50)  NOT NULL,
                Content     TEXT NOT NULL,
                CreatedAt   DATETIME    NULL   DEFAULT CURRENT_DATE,
                ModifiedAt  DATETIME    NULL   DEFAULT CURRENT_DATE )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE ExerciseItem (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                KnowledgeItem     INT       NULL,
                ExerciseType      SMALLINT  NOT NULL,
                Content           TEXT NOT NULL,
                CreatedAt   DATETIME    NULL   DEFAULT CURRENT_DATE,
                ModifiedAt  DATETIME    NULL   DEFAULT CURRENT_DATE,    
                CONSTRAINT FK_EXECITEM_KITEM FOREIGN KEY (KnowledgeItem) REFERENCES KnowledgeItem (ID) ON DELETE SET NULL )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE ExerciseItemAnswer (
                ItemID      INTERGER PRIMARY KEY,
                Content     TEXT NOT NULL,
                CreatedAt   DATETIME    NULL   DEFAULT CURRENT_DATE,
                ModifiedAt  DATETIME    NULL   DEFAULT CURRENT_DATE,    
                CONSTRAINT FK_EXECAWR_EXECITEM FOREIGN KEY (ItemID) REFERENCES ExerciseItem (ID) ON DELETE CASCADE ON UPDATE CASCADE )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE KnowledgeTag (
                Tag   NVARCHAR (20) NOT NULL,
                RefID INT           NOT NULL,
                PRIMARY KEY (Tag, RefID),
                CONSTRAINT FK_KNOWLEDGETAG_ID FOREIGN KEY (RefID) REFERENCES KnowledgeItem ([ID]) ON DELETE CASCADE ON UPDATE CASCADE )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE ExerciseTag (
                Tag   NVARCHAR (20) NOT NULL,
                RefID INT           NOT NULL,
                PRIMARY KEY (Tag, RefID),
                CONSTRAINT FK_KNOWLEDGETAG_ID FOREIGN KEY (RefID) REFERENCES ExerciseItem ([ID]) ON DELETE CASCADE ON UPDATE CASCADE )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE AwardRule (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
	            RuleType	SMALLINT 		NOT NULL,
	            TargetUser	NVARCHAR(50)	NOT NULL,
	            DESP		NVARCHAR(50)	NOT NULL,
	            ValidFrom	DATETIME        NULL DEFAULT CURRENT_DATE,
	            ValidTo		DATETIME        NULL DEFAULT CURRENT_DATE,
	            CountOfFactLow	INT			NULL,
                CountOfFactHigh	INT			NULL,
	            DoneOfFact	BIT				NULL,
	            TimeStart	DECIMAL			NULL,
	            TimeEnd		DECIMAL 		NULL,
	            DaysFrom	INT				NULL,
	            DaysTo		INT				NULL,
	            Point		INT				NOT NULL )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE DailyTrace (
	            TargetUser	NVARCHAR(50)	NOT NULL,
	            RecordDate	DATE			NOT NULL,
	            SchoolWorkTime	DECIMAL		NULL,
	            GoToBedTime		DECIMAL		NULL,
	            HomeWorkCount		SMALLINT	NULL,
	            BodyExerciseCount	SMALLINT	NULL,
	            ErrorsCollection	BIT			NULL,
	            HandWriting		BIT			NULL,
	            CleanDesk			BIT			NULL,
	            HouseKeepingCount	SMALLINT	NULL,
	            PoliteBehavior	SMALLINT	NULL,
	            COMMENT			NVARCHAR(50)	NULL,
	            PRIMARY KEY (TargetUser, RecordDate) )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE AwardPoint (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
	            TargetUser	NVARCHAR(50)	NOT NULL,
	            RecordDate	DATE			NOT NULL,
	            MatchedRuleID	INT			NULL,
	            Point			INT			NOT NULL,
                CountOfDay      INT         NULL,
	            COMMENT		NVARCHAR(50)	NULL )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE InvitedUser (
	            UserID  NVARCHAR(50) NOT NULL,
	            InvitationCode NVARCHAR(20) NOT NULL,
	            UserName NVARCHAR(50) NOT NULL,
	            DisplayAs NVARCHAR(50) NOT NULL,
	            Deleted BIT NULL,
	            CreatedAt	DATE NULL DEFAULT CURRENT_DATE,
	            LastLoginAt DATE NULL DEFAULT CURRENT_DATE,
	            PRIMARY KEY (UserID),
	            CONSTRAINT UX_INVITEDUSERS_CODE UNIQUE(InvitationCode),
	            CONSTRAINT UX_INVITEDUSERS_DISPLAYAS UNIQUE(DisplayAs) )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE AwardUser (
	            TargetUser  NVARCHAR(50) NOT NULL,
	            Supervisor NVARCHAR(50) NOT NULL,
	            PRIMARY KEY (TargetUser, Supervisor) )"
            );

            // Added on 2021.11.06
            database.ExecuteSqlRaw(@"CREATE TABLE UserHabit (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Category   SMALLINT  NOT NULL DEFAULT 0,
                Name       NVARCHAR(50)  NOT NULL,
                TargetUser NVARCHAR(50) NOT NULL,

                Frequency  SMALLINT  NOT NULL DEFAULT 0,
                CompleteCategory  SMALLINT NOT NULL DEFAULT 0,
                CompleteCondition  INT  NOT NULL  DEFAULT 1,

                StartDate     INT NULL,

                Comment    NVARCHAR(50)  NULL,
                ValidFrom  DATE   NULL   DEFAULT CURRENT_DATE,
                ValidTo    DATE   NULL   DEFAULT CURRENT_DATE,

	            CONSTRAINT FK_USERHABIT_USER FOREIGN KEY (TargetUser) REFERENCES InvitedUser (UserID) ON DELETE CASCADE ON UPDATE CASCADE )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE UserHabitRule (
                HabitID                 INT NOT NULL,
                RuleID                  INT NOT NULL,
                ContinuousRecordFrom    INT NULL,
                ContinuousRecordTo      INT NULL,
                Point                   INT NOT NULL,
                PRIMARY KEY (HabitID, RuleID),
	            CONSTRAINT FK_USERHABITRULE_HABIT FOREIGN KEY (HabitID) REFERENCES UserHabit (ID) ON DELETE CASCADE ON UPDATE CASCADE	)"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE UserHabitRecord (
                HabitID         INT            NOT NULL,
	            RecordDate	    DATE		   NOT NULL DEFAULT CURRENT_DATE,
                SubID           INT            NOT NULL DEFAULT 1,
                CompleteFact    INT            NULL,
                RuleID          INT            NULL,
                ContinuousCount INT            NOT NULL DEFAULT 1,
                Comment         NVARCHAR(50)   NULL,
                PRIMARY KEY (HabitID, RecordDate, SubID),
	            CONSTRAINT FK_USERHABITRECORD_HABIT FOREIGN KEY (HabitID) REFERENCES UserHabit (ID) ON DELETE CASCADE ON UPDATE CASCADE )"
            );
        }

        public static void CreateDatabaseViews(DatabaseFacade database)
        {
            database.ExecuteSqlRaw(@"CREATE VIEW Tag
	            AS 
	            SELECT Tag, 1 as RefType, RefID FROM KnowledgeTag
	            UNION ALL
	            SELECT Tag, 2 as RefType, RefID FROM ExerciseTag");

            database.ExecuteSqlRaw(@"CREATE VIEW TagCountByRefType
	            AS
	            SELECT 1 as RefType, count(*) as TagCount FROM KnowledgeTag
	            UNION ALL
	            SELECT 2 as RefType, count(*) as TagCount FROM ExerciseTag");

            database.ExecuteSqlRaw(@"CREATE VIEW TagCount
	            AS
	            SELECT Tag, RefType, count(*) as Count
	            FROM Tag
	            GROUP BY Tag, RefType");

            database.ExecuteSqlRaw(@"CREATE VIEW OverviewInfo
	            AS 
	            SELECT 1 AS RefType, count(*) AS cnt FROM KnowledgeItem
 	            UNION ALL
	            SELECT 2 AS RefType, count(*) AS cnt FROM ExerciseItem");
        }
        #endregion

        internal static void DeleteKnowledgeItem(kbdataContext context, int kid)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM KnowledgeItem WHERE ID = " + kid.ToString());
        }

        internal static void DeleteAwardData(kbdataContext context)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM AwardRule WHERE ID > 0 ");
            context.Database.ExecuteSqlRaw("DELETE FROM AwardPoint WHERE ID > 0 ");
            context.Database.ExecuteSqlRaw("DELETE FROM DailyTrace WHERE TargetUser IS NOT NULL");
        }

        internal static void ClearUserHabitData(kbdataContext context, Int32 habitID)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM UserHabit WHERE ID = " + habitID);
        }

        internal static void CreateInviteUser(kbdataContext context, String supervisor, String testUser)
        {
            // Add Invited User
            //
            InvitedUser usr = new InvitedUser();
            usr.DisplayAs = supervisor;
            usr.InvitationCode = supervisor;
            usr.UserID = supervisor;
            usr.UserName = supervisor;
            context.InvitedUsers.Add(usr);

            usr = new InvitedUser();
            usr.DisplayAs = testUser;
            usr.InvitationCode = testUser;
            usr.UserID = testUser;
            usr.UserName = testUser;
            context.InvitedUsers.Add(usr);

            AwardUser aus = new AwardUser();
            aus.Supervisor = supervisor;
            aus.TargetUser = testUser;
            context.AwardUsers.Add(aus);
        }

        internal static void DeleteInviteUser(kbdataContext context, String supervisor, String testUser)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM InvitedUser WHERE UserID = '" + supervisor + "'");
            context.Database.ExecuteSqlRaw("DELETE FROM InvitedUser WHERE UserID = '" + testUser + "'");
            context.Database.ExecuteSqlRaw("DELETE FROM AwardUser WHERE Supervisor = '" + supervisor + "' AND TargetUser = '" + testUser + "'");
        }

        internal static void DeleteUserHabit(kbdataContext context, int habitid)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM UserHabit WHERE ID = " + habitid.ToString());
        }
    }
}

