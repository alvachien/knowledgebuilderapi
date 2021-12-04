CREATE VIEW Tag
	AS 
	SELECT Tag, 1 as RefType, RefID FROM KnowledgeTag
	UNION ALL
	SELECT Tag, 2 as RefType, RefID FROM ExerciseTag;


CREATE VIEW TagCountByRefType
	AS
	SELECT 1 as RefType, count(*) as TagCount FROM KnowledgeTag
	UNION ALL
	SELECT 2 as RefType, count(*) as TagCount FROM ExerciseTag;
	

CREATE VIEW TagCount
	AS
	SELECT Tag, RefType, count(*) as Count
	FROM Tag
	GROUP BY Tag, RefType;

CREATE VIEW OverviewInfo
	AS 
	SELECT 1 AS RefType, count(*) AS Count FROM KnowledgeItem
 	UNION ALL
	SELECT 2 AS RefType, count(*) AS Count FROM ExerciseItem;

-- Updated on 2021.04.10
CREATE VIEW ExerciseItemWithTagView AS 
	SELECT a.*, b.Tags
	from ExerciseItem as a
	left outer join ( select RefID, STRING_AGG(Tag, ',') as Tags from ExerciseTag GROUP BY RefID ) as b
	on a.ID = b.RefID;

CREATE VIEW KnowledgeItemWithTagView AS 
	SELECT a.ID, a.Content, a.ContentType, a.CreatedAt, a.ModifiedAt, a.Title, b.Tags
	FROM KnowledgeItem as a
	LEFT OUTER JOIN ( select RefID, STRING_AGG(Tag, ',') as Tags from KnowledgeTag GROUP BY RefID ) as b
	on a.ID = b.RefID;

-- View: AwardPointReport
CREATE VIEW AwardPointReport AS 
	WITH records AS ( select TargetUser, RecordDate, SUM(Point) as Point 
			from AwardPoint group by TargetUser, RecordDate )
	select TargetUser, RecordDate, Point, SUM(Point) OVER ( PARTITION BY TargetUser ORDER BY RecordDate ASC  ) as AggPoint
	 from records;

-- View: AwardUserView
CREATE VIEW AwardUserView AS 
	SELECT a.TargetUser, b.UserName, b.DisplayAs, a.Supervisor
		FROM AwardUser AS a
		INNER JOIN InvitedUser AS b
		ON a.TargetUser = b.UserID


-- Created on 2021.11.27
-- Updated on 2021.12.02
-- Updated on 2021.12.03
--CREATE VIEW HabitUserDatePointView AS 
--	SELECT TargetUser, RecordDate, SUM( Points ) OVER ( PARTITION BY TargetUser ORDER BY RecordDate ASC ) as Point
--	from (
--		SELECT c.TargetUser as TargetUser, a.RecordDate as RecordDate, SUM( b.Point ) as Points
--			FROM UserHabitRecord as a
--				INNER JOIN UserHabit as c
--					ON a.HabitID = c.ID
--				LEFT OUTER JOIN UserHabitRule as b
--					ON a.RuleID = b.RuleID
--			WHERE b.RuleID IS NOT NULL
--			GROUP BY c.TargetUser, a.RecordDate ) as a
CREATE VIEW HabitUserDatePointReport AS 
	SELECT c.TargetUser as TargetUser, a.RecordDate as RecordDate, SUM( b.Point ) as Point
			FROM UserHabitRecord as a
				INNER JOIN UserHabit as c
					ON a.HabitID = c.ID
				LEFT OUTER JOIN UserHabitRule as b
					ON a.RuleID = b.RuleID
			WHERE b.RuleID IS NOT NULL
			GROUP BY c.TargetUser, a.RecordDate

-- Created on 2021.11.27
-- Updated on 2021.12.03
CREATE VIEW HabitUserHabitDatePointReport AS 
--	SELECT TargetUser, HabitID, RecordDate, SUM( Points ) OVER ( PARTITION BY TargetUser, HabitID ORDER BY RecordDate ASC ) as Point
--	from (
		SELECT c.TargetUser, a.HabitID, a.RecordDate, SUM( b.Point ) as Point
			FROM UserHabitRecord as a
				INNER JOIN UserHabit as c
					ON a.HabitID = c.ID
				LEFT OUTER JOIN UserHabitRule as b
					ON a.RuleID = b.RuleID
			WHERE b.RuleID IS NOT NULL
			GROUP BY c.TargetUser, a.HabitID, a.RecordDate 	
--	) as a
		

-- Created on 2021.12.02
-- Updated on 2021.12.03
CREATE VIEW UserHabitPointReport AS
--	SELECT TargetUser, HabitID, RecordDate, SUM( Points ) OVER ( PARTITION BY TargetUser, HabitID ORDER BY RecordDate ASC ) as Point
--	from (
	SELECT TargetUser, RecordDate, SUM( Point ) as Point
		FROM UserHabitPoint
		GROUP BY TargetUser, RecordDate
--		) as a 


