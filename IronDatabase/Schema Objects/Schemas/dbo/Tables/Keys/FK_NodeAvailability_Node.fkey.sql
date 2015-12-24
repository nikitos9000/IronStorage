ALTER TABLE [dbo].[NodeAvailability]
    ADD CONSTRAINT [FK_NodeAvailability_Node] FOREIGN KEY ([NodeId]) REFERENCES [dbo].[Node] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;

