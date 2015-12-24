--Накидал.

USE IronDatabase

DROP TABLE [dbo].[NodeAvailability]
DROP TABLE [dbo].[FileNode]
DROP TABLE [dbo].[Node]
DROP TABLE [dbo].[FileAction]
DROP TABLE [dbo].[FileVersion]
DROP TABLE [dbo].[File]
DROP TABLE [dbo].[Directory]
DROP TABLE [dbo].[User]
GO

USE IronDatabase

CREATE TABLE [dbo].[User]
(
	Id UNIQUEIDENTIFIER ROWGUIDCOL DEFAULT NEWID() PRIMARY KEY CLUSTERED, --ID пользователя
	EMailAddress VARCHAR(50) NOT NULL UNIQUE, --Адрес почты
	PasswordHash BINARY(16) NOT NULL, --Хеш пароля (бинарный, 16 байт - 128-бит, MD5)
	Quota BIGINT NOT NULL, --Квота дискового пространства
	QuotaUsed BIGINT NOT NULL, --Использовано квоты (а вообще, это можно по файлам подсчитывать, сделать триггер в File например, и проверку, чтобы было не больше Quota)
	CONSTRAINT [CK_Quota] CHECK (QuotaUsed <= Quota) 
	--Еще нужен ключ с солью для шифрования файлов
)

CREATE TABLE [dbo].[Directory]
(
	Id UNIQUEIDENTIFIER ROWGUIDCOL DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED, --ID файла
	ParentId UNIQUEIDENTIFIER CONSTRAINT [FK_Directory_Directory] FOREIGN KEY REFERENCES [dbo].[Directory](Id), --ID надпапки, NULL - корневая. Удаление всех подпапок реализовать через TRIGGER.
	UserId UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_Directory_User] FOREIGN KEY REFERENCES [dbo].[User](Id) ON UPDATE CASCADE ON DELETE CASCADE, --ID владельца
	IsPublic BIT NOT NULL, --Публичный
	IsVersioned BIT NOT NULL, --Версионный
	EncryptionLevel TINYINT NOT NULL, --Уровень шифрования
	AvailabilityLevel TINYINT NOT NULL, --Уровень доступности
	CONSTRAINT [CK_PublicCrypt] CHECK ((EncryptionLevel = 0 AND IsPublic <> 0) OR (EncryptionLevel > 0 AND IsPublic = 0))
)

CREATE TABLE [dbo].[File]
(
	Id UNIQUEIDENTIFIER ROWGUIDCOL DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED, --ID файла
	DirectoryId UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_File_Directory] FOREIGN KEY REFERENCES [dbo].[Directory](Id) ON UPDATE CASCADE ON DELETE CASCADE, --ID папки
	FileName NVARCHAR(255) NOT NULL, --Имя файла
	IsLocked BIT NOT NULL --Файл находится в процессе обновления
)

CREATE TABLE [dbo].[FileVersion]
(
	Id UNIQUEIDENTIFIER ROWGUIDCOL DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED, --ID файла
	FileId UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_FileVersion_File] FOREIGN KEY REFERENCES [dbo].[File](Id) ON UPDATE CASCADE ON DELETE CASCADE, --ID файла
	FileSize BIGINT NOT NULL, --Размер файла
	DeltaSize BIGINT NOT NULL, --Размер измененной части файла
	FileHash BINARY(16) NOT NULL, --Чексумма файла
)

CREATE TABLE [dbo].[Node]
(
	Id UNIQUEIDENTIFIER ROWGUIDCOL DEFAULT NEWID() NOT NULL PRIMARY KEY CLUSTERED, --ID узла
	UserId UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_Node_User] FOREIGN KEY REFERENCES [dbo].[User](Id) ON UPDATE CASCADE ON DELETE CASCADE, --ID владельца
	IpAddress BINARY(6) NOT NULL, --Ip-адрес (сразу под Ipv6, если Ipv4 То два старших байта нули)
	Port INT NOT NULL, --Порт узла
	Name NVARCHAR(255), --Имя узла
	ProvidedQuota BIGINT NOT NULL, --Выделено места
	ProvidedQuotaUsed BIGINT NOT NULL, --Занято места (это тоже можно подсчитывать по файлам, сделать триггер в File например, и проверку, чтобы было не больше ProvidedQuota)
	ActionNumber BIGINT NOT NULL, --Порядковый номер последнего действия.
	ActionKeyEncrypt BINARY(64) NOT NULL, --Ключ для шифровки токенов действий, используется на основном сервере.
	ActionKeyDecrypt BINARY(64) NOT NULL, --Ключ для расшифровки токенов действий, используется на узлах.
	CONSTRAINT [CK_ProvidedQuota] CHECK (ProvidedQuotaUsed <= ProvidedQuota)
)

CREATE TABLE [dbo].[FileNode]
(
	FileId UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_FileNode_File] FOREIGN KEY REFERENCES [dbo].[File](Id) ON UPDATE CASCADE ON DELETE CASCADE, --ID файла
	NodeId UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_FileNode_Node] FOREIGN KEY REFERENCES [dbo].[Node](Id), --ID узла, хранящего файл (не владельца!). Удаление всех записей реализовать через TRIGGER (по NULL или удалению узла)
	IsDeleted BIT NOT NULL --Файл помечен для удаления
)

CREATE TABLE [dbo].[FileAction]
( 
	Id UNIQUEIDENTIFIER ROWGUIDCOL DEFAULT NEWID() NOT NULL PRIMARY KEY NONCLUSTERED, --ID действия
	FileId UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_FileAction_File] FOREIGN KEY REFERENCES [dbo].[File](Id) ON UPDATE CASCADE ON DELETE CASCADE, --ID файла
	ActionType INT NOT NULL --Тип блокировки
)

CREATE TABLE [dbo].[NodeAvailability]
(
	NodeId UNIQUEIDENTIFIER NOT NULL CONSTRAINT [FK_NodeAvailability_Node] FOREIGN KEY REFERENCES [dbo].[Node](Id) ON UPDATE CASCADE ON DELETE CASCADE,
	TotalAttempts INT NOT NULL,
	SuccessAttempts INT NOT NULL,
	LogDate DATETIME
)
GO

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
GO

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
GO

CREATE INDEX [IX_Directory_ParentId] ON [dbo].[Directory](ParentId)
CREATE INDEX [IX_Directory_UserId] ON [dbo].[Directory](UserId)

CREATE INDEX [IX_File_DirectoryId] ON [dbo].[File](DirectoryId)

CREATE INDEX [IX_FileVersion_FileId] ON [dbo].[FileVersion](FileId)

CREATE INDEX [IX_Node_UserId] ON [dbo].[Node](UserId)

CREATE INDEX [IX_FileNode_FileId] ON [dbo].[FileNode](FileId)
CREATE INDEX [IX_FileNode_NodeId] ON [dbo].[FileNode](NodeId)

CREATE INDEX [IX_FileAction_FileId] ON [dbo].[FileAction](FileId)

CREATE INDEX [IX_NodeAvailability_NodeId] ON [dbo].[NodeAvailability](NodeId)
GO

/* Кластерный индекс может быть только один, и подходит для таких таблиц, которые редко изменяются, но по которым часто идет поиск. (User, Node)
Некластерный соответственно наоборот. (FileLock, File, FileNode)

Не забыть еще убрать избыточные конструкции.
*/