CREATE TABLE [dbo].[Node] (
    [Id]                UNIQUEIDENTIFIER DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [UserId]            UNIQUEIDENTIFIER NOT NULL,
    [IpAddress]         BINARY (6)       NULL,
    [Name]              NVARCHAR (255)   NULL,
    [ProvidedQuota]     BIGINT           NOT NULL,
    [ProvidedQuotaUsed] BIGINT           NOT NULL,
    [ActionNumber]      BIGINT           NOT NULL,
    [ActionKeyEncrypt]  BINARY (64)      NOT NULL,
    [ActionKeyDecrypt]  BINARY (64)      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

