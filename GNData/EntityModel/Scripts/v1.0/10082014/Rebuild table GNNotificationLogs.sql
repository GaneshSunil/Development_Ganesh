

-- Creating table 'GNNotificationLogs'
DROP TABLE [gn].[GNNotificationLogs];


-- Creating table 'GNNotificationLogs'
CREATE TABLE [gn].[GNNotificationLogs] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [GNNotificationTopicId] int  NOT NULL,
    [Addressee] nvarchar(max)  NOT NULL,
    [Sender] nvarchar(max)  NOT NULL,
    [Subject] nvarchar(max)  NULL,
    [Message] nvarchar(max)  NOT NULL,
    [NotificationServiceResponse] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);

-- Creating primary key on [Id] in table 'GNNotificationLogs'
ALTER TABLE [gn].[GNNotificationLogs]
ADD CONSTRAINT [PK_GNNotificationLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);