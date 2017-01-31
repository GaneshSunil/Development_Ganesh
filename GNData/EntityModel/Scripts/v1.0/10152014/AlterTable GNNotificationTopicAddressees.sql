ALTER TABLE [gn].[GNNotificationTopicAddressees]
DROP CONSTRAINT [PK_GNNotificationTopicAddressees];


ALTER TABLE gn.GNNotificationTopicAddressees Alter column AddresseeType nvarchar(3) not null;

update  gn.GNNotificationTopicAddressees set AddresseeType = RTRIM(AddresseeType);

ALTER TABLE [gn].[GNNotificationTopicAddressees]
ADD CONSTRAINT [PK_GNNotificationTopicAddressees]
    PRIMARY KEY CLUSTERED ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType] ASC);
GO