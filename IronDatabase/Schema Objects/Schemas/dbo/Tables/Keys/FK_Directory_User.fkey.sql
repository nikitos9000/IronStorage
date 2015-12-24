ALTER TABLE [dbo].[Directory]
    ADD CONSTRAINT [FK_Directory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;

