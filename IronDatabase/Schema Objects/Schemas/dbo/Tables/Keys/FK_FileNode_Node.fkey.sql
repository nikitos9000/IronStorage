﻿ALTER TABLE [dbo].[FileNode]
    ADD CONSTRAINT [FK_FileNode_Node] FOREIGN KEY ([NodeId]) REFERENCES [dbo].[Node] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
