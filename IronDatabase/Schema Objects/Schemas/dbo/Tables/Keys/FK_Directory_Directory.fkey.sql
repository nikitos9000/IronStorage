ALTER TABLE [dbo].[Directory]
    ADD CONSTRAINT [FK_Directory_Directory] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Directory] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

