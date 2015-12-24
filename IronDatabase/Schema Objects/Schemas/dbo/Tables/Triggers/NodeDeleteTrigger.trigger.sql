
CREATE TRIGGER [dbo].[NodeDeleteTrigger]
ON [dbo].[Node] AFTER DELETE
AS
BEGIN
	DECLARE @deletedId UNIQUEIDENTIFIER
	DECLARE NodeDeleteCursor CURSOR LOCAL FAST_FORWARD FOR
		SELECT [Id] FROM DELETED
	OPEN NodeDeleteCursor

	FETCH NEXT FROM NodeDeleteCursor INTO @deletedId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE FROM [dbo].[FileNode] WHERE [dbo].[FileNode].[NodeId] = @deletedId
		FETCH NEXT FROM NodeDeleteCursor INTO @deletedId
	END

	CLOSE NodeDeleteCursor
END
