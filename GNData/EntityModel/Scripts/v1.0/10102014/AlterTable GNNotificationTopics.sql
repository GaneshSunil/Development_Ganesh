
alter table gn.GNNotificationTopics
add Title nchar(50) NOT NULL DEFAULT 'TITLE MISSING',
    [IsSubscriptionOptional] nchar(1)  NOT NULL DEFAULT 'N'
    