
-- Creating table 'GNAnalysisRequestGroups'
CREATE TABLE [gn].[GNAnalysisRequestGroups] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [GNAnalysisRequestId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO


-- Creating table 'GNAnalysisRequestGroupGNSample'
CREATE TABLE [gn].[GNAnalysisRequestGroupGNSample] (
    [GNAnalysisRequestGroups_Id] uniqueidentifier  NOT NULL,
    [GNSamples_Id] uniqueidentifier  NOT NULL
);


-- Creating primary key on [Id] in table 'GNAnalysisRequestGroups'
ALTER TABLE [gn].[GNAnalysisRequestGroups]
ADD CONSTRAINT [PK_GNAnalysisRequestGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
    
    
-- Creating primary key on [GNAnalysisRequestGroups_Id], [GNSamples_Id] in table 'GNAnalysisRequestGroupGNSample'
ALTER TABLE [gn].[GNAnalysisRequestGroupGNSample]
ADD CONSTRAINT [PK_GNAnalysisRequestGroupGNSample]
    PRIMARY KEY CLUSTERED ([GNAnalysisRequestGroups_Id], [GNSamples_Id] ASC);
    
    
-- Creating foreign key on [GNAnalysisRequestId] in table 'GNAnalysisRequestGroups'
ALTER TABLE [gn].[GNAnalysisRequestGroups]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
    
    
-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupGNAnalysisRequest'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupGNAnalysisRequest]
ON [gn].[GNAnalysisRequestGroups]
    ([GNAnalysisRequestId]);
GO

-- Creating foreign key on [GNAnalysisRequestGroups_Id] in table 'GNAnalysisRequestGroupGNSample'
ALTER TABLE [gn].[GNAnalysisRequestGroupGNSample]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupGNSample_GNAnalysisRequestGroup]
    FOREIGN KEY ([GNAnalysisRequestGroups_Id])
    REFERENCES [gn].[GNAnalysisRequestGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GNSamples_Id] in table 'GNAnalysisRequestGroupGNSample'
ALTER TABLE [gn].[GNAnalysisRequestGroupGNSample]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupGNSample_GNSample]
    FOREIGN KEY ([GNSamples_Id])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupGNSample_GNSample'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupGNSample_GNSample]
ON [gn].[GNAnalysisRequestGroupGNSample]
    ([GNSamples_Id]);
GO


-- Creating table 'GNAnalysisRequestGroupComparisons'
CREATE TABLE [gn].[GNAnalysisRequestGroupComparisons] (
    [Id] uniqueidentifier  NOT NULL,
    [GNAnalysisRequestId] uniqueidentifier  NOT NULL,
    [GNAnalysisRequestControlGroupId] uniqueidentifier  NOT NULL,
    [GNAnalysisRequestComparisonGroupId] uniqueidentifier  NOT NULL
);


-- Creating primary key on [Id] in table 'GNAnalysisRequestGroupComparisons'
ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons]
ADD CONSTRAINT [PK_GNAnalysisRequestGroupComparisons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
    
    
-- Creating foreign key on [GNAnalysisRequestControlGroupId] in table 'GNAnalysisRequestGroupComparisons'
ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupComparisonGNAnalysisRequestGroup]
    FOREIGN KEY ([GNAnalysisRequestControlGroupId])
    REFERENCES [gn].[GNAnalysisRequestGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
    
-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupComparisonGNAnalysisRequestGroup'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupComparisonGNAnalysisRequestGroup]
ON [gn].[GNAnalysisRequestGroupComparisons]
    ([GNAnalysisRequestControlGroupId]);
GO

-- Creating foreign key on [GNAnalysisRequestComparisonGroupId] in table 'GNAnalysisRequestGroupComparisons'
ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupGNAnalysisRequestGroupComparison]
    FOREIGN KEY ([GNAnalysisRequestComparisonGroupId])
    REFERENCES [gn].[GNAnalysisRequestGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupGNAnalysisRequestGroupComparison'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupGNAnalysisRequestGroupComparison]
ON [gn].[GNAnalysisRequestGroupComparisons]
    ([GNAnalysisRequestComparisonGroupId]);
GO


-- Creating foreign key on [GNAnalysisRequestId] in table 'GNAnalysisRequestGroupComparisons'
ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupComparisonGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupComparisonGNAnalysisRequest'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupComparisonGNAnalysisRequest]
ON [gn].[GNAnalysisRequestGroupComparisons]
    ([GNAnalysisRequestId]);
GO


