CREATE TABLE [dbo].[Directory] (
    [Id]                UNIQUEIDENTIFIER DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [ParentId]          UNIQUEIDENTIFIER NULL,
    [UserId]            UNIQUEIDENTIFIER NOT NULL,
    [IsPublic]          BIT              NOT NULL,
    [IsVersioned]       BIT              NOT NULL,
    [EncryptionLevel]   TINYINT          NOT NULL,
    [AvailabilityLevel] TINYINT          NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY]
);

