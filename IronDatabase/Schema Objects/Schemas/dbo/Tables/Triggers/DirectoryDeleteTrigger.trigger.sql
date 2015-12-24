
CREATE TRIGGER [dbo].[DirectoryDeleteTrigger]
ON [dbo].[Directory] FOR DELETE
AS
BEGIN
	DECLARE @deletedId UNIQUEIDENTIFIER
	DECLARE DirectoryDeleteCursor CURSOR LOCAL FAST_FORWARD FOR
		SELECT [Id] FROM DELETED
	OPEN DirectoryDeleteCursor

	FETCH NEXT FROM DirectoryDeleteCursor INTO @deletedId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DELETE FROM [dbo].[Directory] WHERE [dbo].[Directory].[ParentId] = @deletedId
		FETCH NEXT FROM DirectoryDeleteCursor INTO @deletedId
	END

	CLOSE DirectoryDeleteCursor
END
