ALTER TABLE gn.GNNotificationTopics Alter column Code nvarchar(50) not null;
ALTER TABLE gn.GNNotificationTopics Alter column Subject nvarchar(70) not null;
ALTER TABLE gn.GNNotificationTopics Alter column Sender nvarchar(100) not null;
ALTER TABLE gn.GNNotificationTopics Alter column Format nvarchar(5) not null;

update  gn.GNNotificationTopics set code = RTRIM(code), subject = rtrim(subject), sender = rtrim(sender), format=rtrim(format), title=rtrim(format);


alter table gn.GNNotificationTopics drop column [NotifyObjectCreator];
alter table gn.GNNotificationTopics add [NotifyObjectCreator] nchar(1) NULL DEFAULT 'Y';