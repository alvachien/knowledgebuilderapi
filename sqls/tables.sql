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

-- Award Rules
CREATE TABLE [dbo].[AwardRule] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [RuleType]    SMALLINT      NOT NULL,
    [TargetUser]  NVARCHAR (50) NOT NULL,
    [DESP]        NVARCHAR (50) NOT NULL,
    [ValidFrom]   DATETIME      DEFAULT (getdate()) NULL,
    [ValidTo]     DATETIME      DEFAULT (getdate()) NULL,
    [CountOfFactLow] INT           NULL,
	[CountOfFactHigh] INT           NULL,
    [DoneOfFact]  BIT           NULL,
    [TimeStart]   DECIMAL (18)  NULL,
    [TimeEnd]     DECIMAL (18)  NULL,
    [DaysFrom]    INT           NULL,
    [DaysTo]      INT           NULL,
    [Point]       INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE DailyTrace (
	[TargetUser]	NVARCHAR(50)	NOT NULL,
	[RecordDate]	DATE			NOT NULL,
	[SchoolWorkTime]	DECIMAL		NULL,
	[GoToBedTime]		DECIMAL		NULL,
	[HomeWorkCount]		SMALLINT	NULL,
	[BodyExerciseCount]	SMALLINT	NULL,
	[ErrorsCollection]	BIT			NULL,
	[HandWriting]		BIT			NULL,
	[CleanDesk]			BIT			NULL,
	[HouseKeepingCount]	SMALLINT	NULL,
	[PoliteBehavior]	SMALLINT	NULL,
	[COMMENT]			NVARCHAR(50)	NULL,
	PRIMARY KEY CLUSTERED ([TargetUser] ASC, [RecordDate] ASC)
);

CREATE TABLE AwardPoint (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
	[TargetUser]	NVARCHAR(50)	NOT NULL,
	[RecordDate]	DATE			NOT NULL,
	[MatchedRuleID]	INT				NULL,
	[CountOfDay]	INT				NULL,
	[Point]			INT				NOT NULL,
	[COMMENT]		NVARCHAR(50)	NULL,
	PRIMARY KEY CLUSTERED(ID ASC)
);


