

-- Creating table 'GNNotificationTopicSubscribers'
CREATE TABLE [gn].[GNNotificationTopicSubscribers] (
    [GNNotificationTopicId] int  NOT NULL,
    [GNContactId] uniqueidentifier  NOT NULL,
    [AddresseeType] nchar(10)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);


-- Creating primary key on [GNNotificationTopicId], [GNContactId], [AddresseeType] in table 'GNNotificationTopicSubscribers'
ALTER TABLE [gn].[GNNotificationTopicSubscribers]
ADD CONSTRAINT [PK_GNNotificationTopicSubscribers]
    PRIMARY KEY CLUSTERED ([GNNotificationTopicId], [GNContactId], [AddresseeType] ASC);
GO


-- Creating foreign key on [GNContactId] in table 'GNNotificationTopicSubscribers'
ALTER TABLE [gn].[GNNotificationTopicSubscribers]
ADD CONSTRAINT [FK_GNNotificationTopicSubscriberGNContact]
    FOREIGN KEY ([GNContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
    
    
-- Creating non-clustered index for FOREIGN KEY 'FK_GNNotificationTopicSubscriberGNContact'
CREATE INDEX [IX_FK_GNNotificationTopicSubscriberGNContact]
ON [gn].[GNNotificationTopicSubscribers]
    ([GNContactId]);
    
    