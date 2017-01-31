
-- Creating table 'GNSampleStatus'
CREATE TABLE [gn].[GNSampleStatus] (
    [Id] uniqueidentifier  NOT NULL,
    [GNSampleId] uniqueidentifier  NULL,
    [SampleName] nvarchar(50)  NOT NULL,
    [Repository] nvarchar(1000)  NOT NULL,
    [Message] nvarchar(1000)  NULL,
    [PercentComplete] int  NOT NULL,
    [IsError] bit  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating primary key on [Id] in table 'GNSampleStatus'
ALTER TABLE [gn].[GNSampleStatus]
ADD CONSTRAINT [PK_GNSampleStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [GNSampleId] in table 'GNSampleStatus'
ALTER TABLE [gn].[GNSampleStatus]
ADD CONSTRAINT [FK_GNSampleStatusGNSample]
    FOREIGN KEY ([GNSampleId])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleStatusGNSample'
CREATE INDEX [IX_FK_GNSampleStatusGNSample]
ON [gn].[GNSampleStatus]
    ([GNSampleId]);
GO