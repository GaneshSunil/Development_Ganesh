
ALTER TABLE gn.GNCLoudFiles ADD [PassedQC] bit  NULL;
ALTER TABLE gn.GNCLoudFiles ADD [QcReportLocation] nchar(1000)  NULL;

-- Creating table 'GNNewSampleBatches'
CREATE TABLE [gn].[GNNewSampleBatches] (
    [Id] uniqueidentifier  NOT NULL,
    [BatchId] nchar(50)  NOT NULL,
    [GNSequencerJobId] uniqueidentifier  NOT NULL,
    [Repository] nchar(1000)  NULL,
    [Project] nchar(100)  NOT NULL,
    [Type] nchar(20)  NOT NULL,
    [Qualifier] nchar(30)  NOT NULL,
    [ReadType] nchar(20)  NOT NULL,
    [CreateAnalysisPerSample] bit  NOT NULL,
    [TotalSamples] int  NOT NULL,
    [TotalSamplesCompleted] int  NOT NULL,
	[TotalNumberOfFastqFiles] int  NOT NULL,
	[TotalNumberOfFastqFilesCompleted] int  NOT NULL,
    [AutoStartAnalysis] bit  NOT NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNNewSampleBatchStatus'
CREATE TABLE [gn].[GNNewSampleBatchStatus] (
    [Id] uniqueidentifier  NOT NULL,
    [GNNewSampleBatchId] uniqueidentifier  NOT NULL,
    [Repository] nchar(1000)  NULL,
    [Status] nchar(30)  NULL,
    [PercentComplete] int  NOT NULL,
    [IsError] bit  NOT NULL,
    [Message] nchar(1000)  NULL,
    [FilesBucket] nchar(1000)  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNNewSampleBatchSamples'
CREATE TABLE [gn].[GNNewSampleBatchSamples] (
    [Id] uniqueidentifier  NOT NULL,
    [GNNewSampleBatchId] uniqueidentifier  NOT NULL,
    [Name] nchar(50)  NOT NULL,
    [FamilyId] nchar(50)  NOT NULL,
    [RelationId] nchar(50)  NOT NULL,
    [Gender] nchar(1)  NOT NULL  DEFAULT 'U',
    [Affected] nchar(1)  NOT NULL DEFAULT 'N',
    [Proband] nchar(1)  NOT NULL DEFAULT 'N',
    [AnalysisName] nchar(50)  NULL,
    [CreateDateTime] datetime  NULL,
    [GNSample_Id] uniqueidentifier  NOT NULL
);


-- Creating primary key on [Id] in table 'GNNewSampleBatches'
ALTER TABLE [gn].[GNNewSampleBatches]
ADD CONSTRAINT [PK_GNNewSampleBatches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNNewSampleBatchStatus'
ALTER TABLE [gn].[GNNewSampleBatchStatus]
ADD CONSTRAINT [PK_GNNewSampleBatchStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNNewSampleBatchSamples'
ALTER TABLE [gn].[GNNewSampleBatchSamples]
ADD CONSTRAINT [PK_GNNewSampleBatchSamples]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [GNNewSampleBatchId] in table 'GNNewSampleBatchStatus'
ALTER TABLE [gn].[GNNewSampleBatchStatus]
ADD CONSTRAINT [FK_GNNewSampleBatchStatusGNNewSampleBatch]
    FOREIGN KEY ([GNNewSampleBatchId])
    REFERENCES [gn].[GNNewSampleBatches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNewSampleBatchStatusGNNewSampleBatch'
CREATE INDEX [IX_FK_GNNewSampleBatchStatusGNNewSampleBatch]
ON [gn].[GNNewSampleBatchStatus]
    ([GNNewSampleBatchId]);
GO

-- Creating foreign key on [GNNewSampleBatchId] in table 'GNNewSampleBatchSamples'
ALTER TABLE [gn].[GNNewSampleBatchSamples]
ADD CONSTRAINT [FK_GNNewSampleBatchSamplesGNNewSampleBatch]
    FOREIGN KEY ([GNNewSampleBatchId])
    REFERENCES [gn].[GNNewSampleBatches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNewSampleBatchSamplesGNNewSampleBatch'
CREATE INDEX [IX_FK_GNNewSampleBatchSamplesGNNewSampleBatch]
ON [gn].[GNNewSampleBatchSamples]
    ([GNNewSampleBatchId]);
GO

-- Creating foreign key on [GNSample_Id] in table 'GNNewSampleBatchSamples'
ALTER TABLE [gn].[GNNewSampleBatchSamples]
ADD CONSTRAINT [FK_GNNewSampleBatchSamplesGNSample]
    FOREIGN KEY ([GNSample_Id])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO



-- Creating foreign key on [GNSequencerJobId] in table 'GNNewSampleBatches'
ALTER TABLE [gn].[GNNewSampleBatches]
ADD CONSTRAINT [FK_GNNewSampleBatchGNSequencerJob]
    FOREIGN KEY ([GNSequencerJobId])
    REFERENCES [gn].[GNSequencerJobs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO