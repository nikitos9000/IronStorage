ALTER TABLE [dbo].[User]
    ADD CONSTRAINT [CK_Quota] CHECK ([QuotaUsed]<=[Quota]);

