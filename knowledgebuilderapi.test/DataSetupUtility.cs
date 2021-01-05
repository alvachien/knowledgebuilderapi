using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}

