ALTER TABLE [dbo].[File]
    ADD CONSTRAINT [FK_File_Directory] FOREIGN KEY ([DirectoryId]) REFERENCES [dbo].[Directory] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;

