

-- Creating table 'GNTemplates'
CREATE TABLE [gn].[GNTemplates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(100)  NOT NULL,
    [Version] int  NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [GNOrganizationId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNTemplateGenes'
CREATE TABLE [gn].[GNTemplateGenes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GNTemplateId] int  NOT NULL,
    [GeneCode] nvarchar(25)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreatedDateTime] datetime  NOT NULL,
    [GNGeneId] bigint  NOT NULL
);
GO

-- Creating table 'GNAnalysisRequestGNTemplates'
CREATE TABLE [gn].[GNAnalysisRequestGNTemplates] (
    [Id] uniqueidentifier  NOT NULL,
    [GNAnalysisRequestId] uniqueidentifier  NOT NULL,
    [GNTemplateId] int  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreatedDateTime] datetime  NOT NULL
);
GO


-- Creating table 'GNGenes'
CREATE TABLE [gn].[GNGenes] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [GeneCode] nvarchar(25)  NOT NULL,
    [Chromosome] nvarchar(10)  NOT NULL,
    [CytoBand] nvarchar(10)  NOT NULL,
    [Isoform] nvarchar(15)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO




-- Creating primary key on [Id] in table 'GNTemplates'
ALTER TABLE [gn].[GNTemplates]
ADD CONSTRAINT [PK_GNTemplates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNTemplateGenes'
ALTER TABLE [gn].[GNTemplateGenes]
ADD CONSTRAINT [PK_GNTemplateGenes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisRequestGNTemplates'
ALTER TABLE [gn].[GNAnalysisRequestGNTemplates]
ADD CONSTRAINT [PK_GNAnalysisRequestGNTemplates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNGenes'
ALTER TABLE [gn].[GNGenes]
ADD CONSTRAINT [PK_GNGenes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO


-- Creating foreign key on [GNAnalysisRequestId] in table 'GNAnalysisRequestGNTemplates'
ALTER TABLE [gn].[GNAnalysisRequestGNTemplates]
ADD CONSTRAINT [FK_GNAnalysisRequestGNTemplateGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNTemplateGNAnalysisRequest'
CREATE INDEX [IX_FK_GNAnalysisRequestGNTemplateGNAnalysisRequest]
ON [gn].[GNAnalysisRequestGNTemplates]
    ([GNAnalysisRequestId]);
GO

-- Creating foreign key on [GNTemplateId] in table 'GNAnalysisRequestGNTemplates'
ALTER TABLE [gn].[GNAnalysisRequestGNTemplates]
ADD CONSTRAINT [FK_GNAnalysisRequestGNTemplateGNTemplate]
    FOREIGN KEY ([GNTemplateId])
    REFERENCES [gn].[GNTemplates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNTemplateGNTemplate'
CREATE INDEX [IX_FK_GNAnalysisRequestGNTemplateGNTemplate]
ON [gn].[GNAnalysisRequestGNTemplates]
    ([GNTemplateId]);
GO

-- Creating foreign key on [GNTemplateId] in table 'GNTemplateGenes'
ALTER TABLE [gn].[GNTemplateGenes]
ADD CONSTRAINT [FK_GNTemplateGNTemplateGene]
    FOREIGN KEY ([GNTemplateId])
    REFERENCES [gn].[GNTemplates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTemplateGNTemplateGene'
CREATE INDEX [IX_FK_GNTemplateGNTemplateGene]
ON [gn].[GNTemplateGenes]
    ([GNTemplateId]);
GO

-- Creating foreign key on [GNOrganizationId] in table 'GNTemplates'
ALTER TABLE [gn].[GNTemplates]
ADD CONSTRAINT [FK_GNTemplateGNOrganization]
    FOREIGN KEY ([GNOrganizationId])
    REFERENCES [gn].[GNOrganizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTemplateGNOrganization'
CREATE INDEX [IX_FK_GNTemplateGNOrganization]
ON [gn].[GNTemplates]
    ([GNOrganizationId]);
GO


-- Creating non-clustered index for FOREIGN KEY 'FK_GNTemplateGeneGNGene'
CREATE INDEX [IX_FK_GNTemplateGeneGNGene]
ON [gn].[GNTemplateGenes]
    ([GNGeneId]);
GO
