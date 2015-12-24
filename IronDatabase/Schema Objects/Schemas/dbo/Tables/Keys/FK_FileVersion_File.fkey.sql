ALTER TABLE [dbo].[FileVersion]
    ADD CONSTRAINT [FK_FileVersion_File] FOREIGN KEY ([FileId]) REFERENCES [dbo].[File] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;

