alter table gn.GNSamples add GNReplicateCode nchar(2)  NOT NULL default 'NO';

-- Creating table 'GNReplicates'
CREATE TABLE [gn].[GNReplicates] (
    [Code] nchar(2)  NOT NULL,
    [Name] nchar(2)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating primary key on [Code] in table 'GNReplicates'
ALTER TABLE [gn].[GNReplicates]
ADD CONSTRAINT [PK_GNReplicates]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [GNReplicateCode] in table 'GNSamples'
ALTER TABLE [gn].[GNSamples]
ADD CONSTRAINT [IX_FK_GNSampleGNReplicate2]
    FOREIGN KEY ([GNReplicateCode])
    REFERENCES [gn].[GNReplicates]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleGNReplicate1'
CREATE INDEX [IX_FK_GNSampleGNReplicate2]
ON [gn].[GNSamples]
    ([GNReplicateCode]);
GO