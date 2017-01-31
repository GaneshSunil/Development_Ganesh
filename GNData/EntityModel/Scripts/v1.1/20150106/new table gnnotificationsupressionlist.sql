
-- Creating table 'GNNotificationSuppressionLists'
CREATE TABLE [gn].[GNNotificationSuppressionLists] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [CreateDateTime] datetime  NOT NULL,
    [Category] nvarchar(20)  NOT NULL,
    [Type] nvarchar(max)  NULL,
    [Subtype] nvarchar(max)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [NotifiedAdminOn] datetime  NULL,
    [AdminNotes] nvarchar(max)  NULL
);
GO


-- Creating primary key on [Id] in table 'GNNotificationSuppressionLists'
ALTER TABLE [gn].[GNNotificationSuppressionLists]
ADD CONSTRAINT [PK_GNNotificationSuppressionLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO