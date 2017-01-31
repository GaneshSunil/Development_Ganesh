
-- Creating table 'GNNotificationSenders'
CREATE TABLE [gn].[GNNotificationSenders] (
    [Sender] nchar(50)  NOT NULL,
    [Name] nchar(100)  NOT NULL
);



-- Creating primary key on [Sender] in table 'GNNotificationSenders'
ALTER TABLE [gn].[GNNotificationSenders]
ADD CONSTRAINT [PK_GNNotificationSenders]
    PRIMARY KEY CLUSTERED ([Sender] ASC);
    
    