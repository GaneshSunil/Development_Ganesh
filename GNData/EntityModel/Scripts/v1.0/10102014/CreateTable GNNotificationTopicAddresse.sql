
-- Creating table 'GNNotificationTopicAddressees'
CREATE TABLE [gn].[GNNotificationTopicAddressees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GNNotificationTopicId] int  NOT NULL,
    [AspNetRoleId] uniqueidentifier  NOT NULL,
    [AddresseeType] nchar(10)  NOT NULL DEFAULT 'TO',
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);



-- Creating primary key on [Id] in table 'GNNotificationTopicAddressees'
ALTER TABLE [gn].[GNNotificationTopicAddressees]
ADD CONSTRAINT [PK_GNNotificationTopicAddressees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [GNNotificationTopicId] in table 'GNNotificationTopicAddressees'
ALTER TABLE [gn].[GNNotificationTopicAddressees]
ADD CONSTRAINT [FK_GNNotificationTopicGNNotificationTopicRole]
    FOREIGN KEY ([GNNotificationTopicId])
    REFERENCES [gn].[GNNotificationTopics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNotificationTopicGNNotificationTopicRole'
CREATE INDEX [IX_FK_GNNotificationTopicGNNotificationTopicRole]
ON [gn].[GNNotificationTopicAddressees]
    ([GNNotificationTopicId]);
GO