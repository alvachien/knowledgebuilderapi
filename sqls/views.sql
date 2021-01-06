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
