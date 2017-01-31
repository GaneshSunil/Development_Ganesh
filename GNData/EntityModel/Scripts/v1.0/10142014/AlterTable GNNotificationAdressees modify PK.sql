ALTER TABLE gn.GNNotificationTopicAddressees DROP CONSTRAINT PK_GNNotificationTopicAddressees
ALTER TABLE gn.GNNotificationTopicAddressees ALTER COLUMN [AspNetRoleId] NVARCHAR(50) NOT NULL;

-- Creating primary key on [GNNotificationTopicId], [AspNetRoleId], [AddresseeType] in table 'GNNotificationTopicAddressees'
ALTER TABLE [gn].[GNNotificationTopicAddressees]
ADD CONSTRAINT [PK_GNNotificationTopicAddressees]
    PRIMARY KEY CLUSTERED ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType] ASC);
GO