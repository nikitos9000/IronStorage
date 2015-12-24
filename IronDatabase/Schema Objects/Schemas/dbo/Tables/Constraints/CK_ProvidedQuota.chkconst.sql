ALTER TABLE [dbo].[Node]
    ADD CONSTRAINT [CK_ProvidedQuota] CHECK ([ProvidedQuotaUsed]<=[ProvidedQuota]);

