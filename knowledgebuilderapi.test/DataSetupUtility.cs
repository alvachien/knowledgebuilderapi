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
                Tags        NVARCHAR(100)    NULL,
                CreatedAt   DATETIME    NULL   DEFAULT CURRENT_DATE,
                ModifiedAt  DATETIME    NULL   DEFAULT CURRENT_DATE )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE QuestionBankItem (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                KnowledgeItem     INT            NULL,
                ParentID          INT            NULL,
                QBType            INT            NOT NULL,
                Content           TEXT NOT NULL,
                CreatedAt   DATETIME    NULL   DEFAULT CURRENT_DATE,
                ModifiedAt  DATETIME    NULL   DEFAULT CURRENT_DATE,    
                CONSTRAINT FK_QBITEM_KITEM FOREIGN KEY (KnowledgeItem) REFERENCES KnowledgeItem (ID) ON DELETE SET NULL )"
            );

            database.ExecuteSqlRaw(@"CREATE TABLE QuestionBankSubItem (
                ItemID            INT            NOT NULL,
                SubID             NVARCHAR(20)   NOT NULL,
                Content           TEXT NOT NULL,
                CreatedAt   DATETIME    NULL   DEFAULT CURRENT_DATE,
                ModifiedAt  DATETIME    NULL   DEFAULT CURRENT_DATE,    
                PRIMARY KEY (ItemID, SubID),
                CONSTRAINT FK_QBSUBITEM_QBITEM FOREIGN KEY (ItemID) REFERENCES QuestionBankItem (ID) ON DELETE CASCADE ON UPDATE CASCADE )"
            );
        }

        public static void CreateDatabaseViews(DatabaseFacade database)
        {
            // Nothing
        }
        #endregion

        internal static void DeleteKnowledgeItem(kbdataContext context, int kid)
        {
            context.Database.ExecuteSqlRaw("DELETE FROM KnowledgeItem WHERE ID = " + kid.ToString());

        }
    }
}

