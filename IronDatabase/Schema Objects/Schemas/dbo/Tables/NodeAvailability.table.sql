CREATE TABLE [dbo].[NodeAvailability] (
    [NodeId]          UNIQUEIDENTIFIER NOT NULL,
    [TotalAttempts]   INT              NOT NULL,
    [SuccessAttempts] INT              NOT NULL,
    [LogDate]         DATETIME         NULL
);

