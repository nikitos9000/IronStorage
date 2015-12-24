ALTER TABLE [dbo].[Directory]
    ADD CONSTRAINT [CK_PublicCrypt] CHECK ([EncryptionLevel]=(0) AND [IsPublic]<>(0) OR [EncryptionLevel]>(0) AND [IsPublic]=(0));

