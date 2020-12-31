-- T-SQL based
-- Tested on SQL Server Express/SQL Server 2016


CREATE TABLE [dbo].[KnowledgeItem] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [ContentType] SMALLINT       NULL,
    [Title]       NVARCHAR (50)  NOT NULL,
    [Content]     NVARCHAR (MAX) NOT NULL,
    [Tags]        NCHAR (100)    NULL,
    [CreatedAt]   DATETIME       DEFAULT (getdate()) NULL,
    [ModifiedAt]  DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[ExerciseItem] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [KnowledgeItem] INT            NULL,
    [ParentID]      INT            NULL,
    [ExerciseType]  SMALLINT       NOT NULL,
    [Content]       NVARCHAR (MAX) NOT NULL,
    [CreatedAt]     DATETIME       DEFAULT (getdate()) NULL,
    [ModifiedAt]    DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_EXECITEM_KITEM] FOREIGN KEY ([KnowledgeItem]) REFERENCES [dbo].[KnowledgeItem] ([ID]) ON DELETE SET NULL
);

CREATE TABLE [dbo].[ExerciseItemAnswer] (
    [ItemID]     INT            NOT NULL,
    [Content]    NVARCHAR (MAX) NOT NULL,
    [CreatedAt]  DATETIME       DEFAULT (getdate()) NULL,
    [ModifiedAt] DATETIME       DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([ItemID] ASC),
    CONSTRAINT [FK_EXECAWR_EXECITEM] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[ExerciseItem] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [dbo].[KnowledgeTag] (
    [Tag]   NVARCHAR (20) NOT NULL,
    [RefID] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Tag], [RefID]),
    CONSTRAINT [FK_KNOWLEDGETAG_ID] FOREIGN KEY ([RefID]) REFERENCES [dbo].[KnowledgeItem] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE [dbo].[ExerciseTag] (
    [Tag]   NVARCHAR (20) NOT NULL,
    [RefID] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Tag], [RefID]),
    CONSTRAINT [FK_EXERCISETAG_ID] FOREIGN KEY ([RefID]) REFERENCES [dbo].[ExerciseItem] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);