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
/* CREATE TABLE [dbo].[AwardRule] (
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
); */

CREATE TABLE DailyTrace (
	[TargetUser]	NVARCHAR(50)	NOT NULL,
	[RecordDate]	DATE			NOT NULL,
	[SchoolWorkTime]	DECIMAL (6, 2)	NULL,
	[GoToBedTime]		DECIMAL (6, 2)	NULL,
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

CREATE TABLE UserCollection (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
	[User]			NVARCHAR(50)	NOT NULL,
	[Name]			NVARCHAR(50)	NOT NULL,
	[COMMENT]		NVARCHAR(50)	NULL,
    [CreatedAt]     DATETIME       DEFAULT (getdate()) NULL,
    [ModifiedAt]    DATETIME       DEFAULT (getdate()) NULL,
	PRIMARY KEY CLUSTERED(ID ASC)
);

CREATE TABLE UserCollectionItem (
    [ID]            INT,
    [RefType]    	SMALLINT      NOT NULL,
    [RefID] 		INT           NOT NULL,
    [CreatedAt]     DATETIME       DEFAULT (getdate()) NULL,
	PRIMARY KEY CLUSTERED(ID ASC, RefType ASC, RefID ASC),
	CONSTRAINT [FK_USERCOLL_ITEM_ID] FOREIGN KEY ([ID]) REFERENCES [dbo].[UserCollection] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE	
);

CREATE TABLE ExerciseItemUserScore (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
	[User]			NVARCHAR(50)	NOT NULL,
    [RefID] 		INT           NOT NULL,
    [TakenDate]     DATETIME       DEFAULT (getdate()),
	[Score] 		INT				NOT NULL,
	PRIMARY KEY CLUSTERED(ID ASC),
	CONSTRAINT [FK_EXERCISEITEM_USRSCORE_ID] FOREIGN KEY ([RefID]) REFERENCES [dbo].[ExerciseItem] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE	
);

-- Award Rules Group
CREATE TABLE [dbo].[AwardRuleGroup] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [RuleType]    SMALLINT      NOT NULL,
    [TargetUser]  NVARCHAR (50) NOT NULL,
    [DESP]        NVARCHAR (50) NOT NULL,
    [ValidFrom]   DATETIME      DEFAULT (getdate()) NULL,
    [ValidTo]     DATETIME      DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

-- Award Rules
CREATE TABLE [dbo].[AwardRule] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
	[GroupID]	  INT			NOT NULL,
    [CountOfFactLow] INT        NULL,
	[CountOfFactHigh] INT       NULL,
    [DoneOfFact]  BIT           NULL,
    [TimeStart]   DECIMAL (6, 2)  NULL,
    [TimeEnd]     DECIMAL (6, 2)  NULL,
    [DaysFrom]    INT           NULL,
    [DaysTo]      INT           NULL,
    [Point]       INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_AWARDRULE_GROUPID] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[AwardRuleGroup] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE	
);

-- AwardUser
CREATE TABLE [dbo].[AwardUser] (
	[TargetUser]  NVARCHAR (50) NOT NULL,
	[Supervisor] NVARCHAR(50) NOT NULL,
	PRIMARY KEY CLUSTERED ([TargetUser] ASC, [Supervisor] ASC)
);

-- Invited USER
CREATE TABLE [dbo].[InvitedUser] (
	[UserID]  NVARCHAR(50) NOT NULL,
	[InvitationCode] NVARCHAR(20) NOT NULL,
	[UserName] NVARCHAR(50) NOT NULL,
	[DisplayAs] NVARCHAR(50) NOT NULL,
	[Deleted] BIT NULL,
	[CreatedAt]	DATETIME DEFAULT (getdate()) NULL,
	[LastLoginAt] DATETIME DEFAULT (getdate()) NULL,
	PRIMARY KEY CLUSTERED ([UserID] ASC),
	CONSTRAINT UX_INVITEDUSERS_CODE UNIQUE([InvitationCode]),
	CONSTRAINT UX_INVITEDUSERS_DISPLAYAS UNIQUE([DisplayAs])
);


-- Added since 2021.11.6
--  Habit Item
CREATE TABLE UserHabit(
    [ID]            INT           IDENTITY (1, 1) NOT NULL,
    [Category]      SMALLINT      DEFAULT(0) NOT NULL,
    [Name]          NVARCHAR(50)  NOT NULL,
    [TargetUser]    NVARCHAR (50) NOT NULL,

    [Frequency]     SMALLINT      DEFAULT(0) NOT NULL,
    [DoneCriteria]  INT           DEFAULT(1) NOT NULL,
    [StartDate]     INT           NULL,

    [Comment]       NVARCHAR(50)  NULL,
    [ValidFrom]     DATE          DEFAULT (getdate()) NULL,
    [ValidTo]       DATE          DEFAULT (getdate()) NULL,

    PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_USERHABIT_USER] FOREIGN KEY ([TargetUser]) REFERENCES [InvitedUser] ([UserID]) ON DELETE CASCADE ON UPDATE CASCADE	
);

-- Habit Item Rules
CREATE TABLE UserHabitRule(
    [HabitID]       INT            NOT NULL,
    [RuleID]        INT            NOT NULL,
    [ContinuousRecordFrom]   INT    NULL,
    [ContinuousRecordTo]     INT    NULL,
    [Point]         INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([HabitID] ASC, [RuleID] ASC),
	CONSTRAINT [FK_USERHABITRULE_HABIT] FOREIGN KEY ([HabitID]) REFERENCES [UserHabit] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE	
);

-- User Habit Record
CREATE TABLE UserHabitRecord(
    [HabitID]       INT            NOT NULL,
	[RecordDate]	DATE		   DEFAULT (getdate()) NOT NULL,
    [RuleID]        INT            NULL,
    [Comment]       NVARCHAR(50)   NULL,
    PRIMARY KEY CLUSTERED ([HabitID] ASC, [RecordDate] ASC),
	CONSTRAINT [FK_USERHABITRECORD_HABIT] FOREIGN KEY ([HabitID]) REFERENCES [UserHabit] ([ID]) ON DELETE CASCADE ON UPDATE CASCADE
);

