using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using knowledgebuilderapi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace knowledgebuilderapi.test.common
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

            database.ExecuteSqlRaw(@"CREATE VIEW ExerciseItemWithTagView AS 
	            SELECT a.*, b.Tags
	            from ExerciseItem as a
	            left outer join ( select RefID, STRING_AGG(Tag, ',') as Tags from ExerciseTag GROUP BY RefID ) as b
	            on a.ID = b.RefID");

            database.ExecuteSqlRaw(@"CREATE VIEW KnowledgeItemWithTagView AS 
	            SELECT a.ID, a.Content, a.ContentType, a.CreatedAt, a.ModifiedAt, a.Title, b.Tags
	            FROM KnowledgeItem as a
	            LEFT OUTER JOIN ( select RefID, STRING_AGG(Tag, ',') as Tags from KnowledgeTag GROUP BY RefID ) as b
	            on a.ID = b.RefID");

            database.ExecuteSqlRaw(@"CREATE VIEW AwardPointReport AS 
	            WITH records AS ( select TargetUser, RecordDate, SUM(Point) as Point 
			            from AwardPoint group by TargetUser, RecordDate )
	            select TargetUser, RecordDate, Point, SUM(Point) OVER ( PARTITION BY TargetUser ORDER BY RecordDate ASC  ) as AggPoint
	             from records");

            database.ExecuteSqlRaw(@"CREATE VIEW AwardUserView AS 
	            SELECT a.TargetUser, b.UserName, b.DisplayAs, a.Supervisor
		            FROM AwardUser AS a
		            INNER JOIN InvitedUser AS b
		            ON a.TargetUser = b.UserID");

            database.ExecuteSqlRaw(@"CREATE VIEW HabitUserDatePointReport AS 
	            SELECT c.TargetUser as TargetUser, a.RecordDate as RecordDate, SUM( b.Point ) as Point
			            FROM UserHabitRecord as a
				            INNER JOIN UserHabit as c
					            ON a.HabitID = c.ID
				            LEFT OUTER JOIN UserHabitRule as b
					            ON c.ID = b.HabitID and a.RuleID = b.RuleID
			            WHERE b.RuleID IS NOT NULL
			            GROUP BY c.TargetUser, a.RecordDate");

            database.ExecuteSqlRaw(@"CREATE VIEW HabitUserHabitDatePointReport AS 
		        SELECT c.TargetUser, a.HabitID, a.RecordDate, SUM( b.Point ) as Point
			        FROM UserHabitRecord as a
				        INNER JOIN UserHabit as c
					        ON a.HabitID = c.ID
				        LEFT OUTER JOIN UserHabitRule as b
					        ON c.ID = b.HabitID and a.RuleID = b.RuleID
			        WHERE b.RuleID IS NOT NULL
			        GROUP BY c.TargetUser, a.HabitID, a.RecordDate ");

            database.ExecuteSqlRaw(@"CREATE VIEW UserHabitPointReport AS
	            SELECT TargetUser, RecordDate, SUM( Point ) as Point
		            FROM UserHabitPoint
		            GROUP BY TargetUser, RecordDate ");

            database.ExecuteSqlRaw(@"CREATE VIEW UserHabitRecordView AS 
	            SELECT a.*, b.Name as HabitName, b.ValidFrom as HabitValidFrom, b.ValidTo as HabitValidTo,
	            b.TargetUser, c.ContinuousRecordFrom as RuleDaysFrom, c.ContinuousRecordTo as RuleDaysTo,
	            c.Point as RulePoint
	            from UserHabitRecord as a
	            inner join UserHabit as b
		            on a.HabitID = b.ID
	            left outer join UserHabitRule as c
		            on a.HabitID = c.HabitID
			            and a.RuleID = c.RuleID");
        }
        #endregion

        #region Constants of initialized data
        public const string UserA = "UserA";
        public const string UserB = "UserB";
        public const int Knowledge1ID = 1;
        public const int Knowledge2ID = 2;
        public const string Tag1 = "Tag1";
        public const string Tag2 = "Tag2";
        public const int Exercise1ID = 1;
        public const int Exercise2ID = 2;
        #endregion

        #region Initialize test data
        public static void InitalizeTestData(kbdataContext context)
        {
            #region Invite users
            context.InvitedUsers.Add(new InvitedUser
            {
                DisplayAs = "User A",
                InvitationCode = "User A Invit",
                UserID = UserA,
                UserName = "User A",
            });

            context.InvitedUsers.Add(new InvitedUser
            {
                DisplayAs = "User B",
                InvitationCode = "User B Invit",
                UserID = UserB,
                UserName = "User B",
            });
            #endregion

            #region Award users
            context.AwardUsers.Add(new AwardUser
            {
                Supervisor = UserA,
                TargetUser = UserB,
            });
            #endregion

            #region Knowledge items
            context.KnowledgeItems.Add(new KnowledgeItem
            {
                ID = Knowledge1ID,
                Category = KnowledgeItemCategory.Concept,
                Title = "Knowledge 1",
                Content = "Knowledge 1 Content"
            });
            context.KnowledgeItems.Add(new KnowledgeItem
            {
                ID = Knowledge2ID,
                Category = KnowledgeItemCategory.Concept,
                Title = "Knowledge 2",
                Content = "Knowledge 2 Content"
            });
            context.KnowledgeTags.Add(new KnowledgeTag
            {
                RefID = Knowledge1ID,
                TagTerm = Tag1
            });
            context.KnowledgeTags.Add(new KnowledgeTag
            {
                RefID = Knowledge2ID,
                TagTerm = Tag2
            });
            #endregion

            #region Exercise items
            context.ExerciseItems.Add(new ExerciseItem
            {
                ID = Exercise1ID,
                ExerciseType = ExerciseItemType.Question,
                Content = "Exercise 1"
            });
            context.ExerciseItems.Add(new ExerciseItem
            {
                ID = Exercise2ID,
                ExerciseType = ExerciseItemType.Question,
                Content = "Exercise 2"
            });
            context.ExerciseItemAnswers.Add(new ExerciseItemAnswer
            {
                ID = Exercise1ID,
                Content = "Answer for Exercise 1"
            });
            context.ExerciseItemAnswers.Add(new ExerciseItemAnswer
            {
                ID = Exercise2ID,
                Content = "Answer for Exercise 2"
            });
            context.ExerciseTags.Add(new ExerciseTag
            {
                RefID = Exercise1ID,
                TagTerm = Tag1
            });
            context.ExerciseTags.Add(new ExerciseTag
            {
                RefID = Exercise1ID,
                TagTerm = Tag2
            });
            context.ExerciseTags.Add(new ExerciseTag
            {
                RefID = Exercise2ID,
                TagTerm = Tag1
            });
            #endregion

            context.SaveChanges();
        }
        #endregion

        public static ClaimsPrincipal GetClaimForUser(String usr)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usr),
                new Claim(ClaimTypes.NameIdentifier, usr),
            }, "mock"));
        }

        public static void DeleteKnowledgeItem(kbdataContext context, int kid)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM KnowledgeItem WHERE ID = " + kid.ToString());
        }

        public static void DeleteExerciseItem(kbdataContext context, int eid)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM ExerciseItem WHERE ID = " + eid.ToString());
        }

        public static void DeleteAwardData(kbdataContext context)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM AwardRule WHERE ID > 0 ");
            context.Database.ExecuteSqlRaw("DELETE FROM AwardPoint WHERE ID > 0 ");
            context.Database.ExecuteSqlRaw("DELETE FROM DailyTrace WHERE TargetUser IS NOT NULL");
        }

        public static void ClearUserHabitData(kbdataContext context, Int32 habitID)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM UserHabit WHERE ID = " + habitID);
        }

        //public static void CreateInviteUser(kbdataContext context, String supervisor, String testUser)
        //{
        //    // Add Invited User
        //    //
        //    InvitedUser usr = new InvitedUser();
        //    usr.DisplayAs = supervisor;
        //    usr.InvitationCode = supervisor;
        //    usr.UserID = supervisor;
        //    usr.UserName = supervisor;
        //    context.InvitedUsers.Add(usr);

        //    usr = new InvitedUser();
        //    usr.DisplayAs = testUser;
        //    usr.InvitationCode = testUser;
        //    usr.UserID = testUser;
        //    usr.UserName = testUser;
        //    context.InvitedUsers.Add(usr);

        //    AwardUser aus = new AwardUser();
        //    aus.Supervisor = supervisor;
        //    aus.TargetUser = testUser;
        //    context.AwardUsers.Add(aus);
        //}

        //public static void DeleteInviteUser(kbdataContext context, String supervisor, String testUser)
        //{
        //    context.Database.ExecuteSqlRaw("DELETE FROM InvitedUser WHERE UserID = '" + supervisor + "'");
        //    context.Database.ExecuteSqlRaw("DELETE FROM InvitedUser WHERE UserID = '" + testUser + "'");
        //    context.Database.ExecuteSqlRaw("DELETE FROM AwardUser WHERE Supervisor = '" + supervisor + "' AND TargetUser = '" + testUser + "'");
        //}

        public static void DeleteUserHabit(kbdataContext context, int habitid)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM UserHabit WHERE ID = " + habitid.ToString());
        }
    }
}

