ALTER TABLE gn.GNNotificationTopics DROP COLUMN [Description];
ALTER TABLE gn.GNNotificationTopics ADD [Description] varchar(MAX) null;
