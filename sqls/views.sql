CREATE VIEW Tag
	AS 
	SELECT Tag, 1 as RefType, RefID FROM KnowledgeTag
	UNION ALL
	SELECT Tag, 2 as RefType, RefID FROM ExerciseTag
