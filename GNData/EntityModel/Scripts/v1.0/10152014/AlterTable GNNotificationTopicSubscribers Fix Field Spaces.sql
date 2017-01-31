
alter table gn.GNNotificationTopicSubscribers drop constraint [PK_GNNotificationtopicsubscribers]

ALTER TABLE gn.GNNotificationTopicSubscribers Alter column AddresseeType nvarchar(10) not null;
update  gn.GNNotificationTopicSubscribers set AddresseeType = RTRIM(AddresseeType);

ALTER TABLE [gn].[GNNotificationTopicSubscribers]
ADD CONSTRAINT [PK_GNNotificationTopicSubscribers]
    PRIMARY KEY CLUSTERED ([GNNotificationTopicId], [GNContactId], [AddresseeType] ASC);
GO