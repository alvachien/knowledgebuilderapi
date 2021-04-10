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
