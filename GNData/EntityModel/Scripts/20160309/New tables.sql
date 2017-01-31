ALTER TABLE gn.GNSamples ADD [GNSampleQualifierCode] nvarchar(30)  NOT NULL DEFAULT 'DNA';

DROP TABLE gn.GNSampleQualifiers

-- Creating table 'GNSampleQualifiers'
CREATE TABLE [gn].[GNSampleQualifiers] (
    [Code] nvarchar(30)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,    
    [GNSampleQualifierGroupCode] nvarchar(30)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);

-- Creating primary key on [Code] in table 'GNSampleQualifiers'
ALTER TABLE [gn].[GNSampleQualifiers]
ADD CONSTRAINT [PK_GNSampleQualifiers]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating foreign key on [GNSampleQualifierCode] in table 'GNSamples'
ALTER TABLE [gn].[GNSamples]
ADD CONSTRAINT [FK_GNSampleGNSampleQualifier]
    FOREIGN KEY ([GNSampleQualifierCode])
    REFERENCES [gn].[GNSampleQualifiers]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleGNSampleQualifier'
CREATE INDEX [IX_FK_GNSampleGNSampleQualifier]
ON [gn].[GNSamples]
    ([GNSampleQualifierCode]);
GO
    
------------------------------
ALTER TABLE gn.GNAnalysisRequests ADD [GNAnalysisRequestTypeCode] nvarchar(30)  NOT NULL DEFAULT 'DNA';


-- Creating table 'GNAnalysisRequestTypes'
CREATE TABLE [gn].[GNAnalysisRequestTypes] (
    [Code] nvarchar(30)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO
-- Creating primary key on [Code] in table 'GNAnalysisRequestTypes'
ALTER TABLE [gn].[GNAnalysisRequestTypes]
ADD CONSTRAINT [PK_GNAnalysisRequestTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
    
-- Creating foreign key on [GNAnalysisRequestTypeCode] in table 'GNAnalysisRequests'
ALTER TABLE [gn].[GNAnalysisRequests]
ADD CONSTRAINT [FK_GNAnalysisRequestGNAnalysisRequestType]
    FOREIGN KEY ([GNAnalysisRequestTypeCode])
    REFERENCES [gn].[GNAnalysisRequestTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
    
    
-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNAnalysisRequestType'
CREATE INDEX [IX_FK_GNAnalysisRequestGNAnalysisRequestType]
ON [gn].[GNAnalysisRequests]
    ([GNAnalysisRequestTypeCode]);
    

-- Creating table 'GNSampleQualifierGroups'
CREATE TABLE [gn].[GNSampleQualifierGroups] (
    [Code] nvarchar(30)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);


-- Creating primary key on [Code] in table 'GNSampleQualifierGroups'
ALTER TABLE [gn].[GNSampleQualifierGroups]
ADD CONSTRAINT [PK_GNSampleQualifierGroups]
    PRIMARY KEY CLUSTERED ([Code] ASC);
    
    
-- Creating foreign key on [GNSampleQualifierGroupCode] in table 'GNSampleQualifiers'
ALTER TABLE [gn].[GNSampleQualifiers]
ADD CONSTRAINT [FK_GNSampleQualifierGroupGNSampleQualifier]
    FOREIGN KEY ([GNSampleQualifierGroupCode])
    REFERENCES [gn].[GNSampleQualifierGroups]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
    
    
-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleQualifierGroupGNSampleQualifier'
CREATE INDEX [IX_FK_GNSampleQualifierGroupGNSampleQualifier]
ON [gn].[GNSampleQualifiers]
    ([GNSampleQualifierGroupCode]);
    
    
    
-- Creating table 'GNSharedPurchaseOrderOrganizations'
CREATE TABLE [gn].[GNSharedPurchaseOrderOrganizations] (
    [Id] uniqueidentifier  NOT NULL,
    [AmountBilledOnInvoice] float  NOT NULL,
    [GNPurchaseOrderId] uniqueidentifier  NOT NULL,
    [GNInvoiceId] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'GNSharedPurchaseOrderOrganizations'
ALTER TABLE [gn].[GNSharedPurchaseOrderOrganizations]
ADD CONSTRAINT [PK_GNSharedPurchaseOrderOrganizations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
    
    
-- Creating foreign key on [GNInvoiceId] in table 'GNSharedPurchaseOrderOrganizations'
ALTER TABLE [gn].[GNSharedPurchaseOrderOrganizations]
ADD CONSTRAINT [FK_GNSharedPurchaseOrderOrganizationGNInvoice]
    FOREIGN KEY ([GNInvoiceId])
    REFERENCES [gn].[GNInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
    
    
-- Creating non-clustered index for FOREIGN KEY 'FK_GNSharedPurchaseOrderOrganizationGNInvoice'
CREATE INDEX [IX_FK_GNSharedPurchaseOrderOrganizationGNInvoice]
ON [gn].[GNSharedPurchaseOrderOrganizations]
    ([GNInvoiceId]);
    
    
-- Creating foreign key on [GNPurchaseOrderId] in table 'GNSharedPurchaseOrderOrganizations'
ALTER TABLE [gn].[GNSharedPurchaseOrderOrganizations]
ADD CONSTRAINT [FK_GNSharedPurchaseOrderOrganizationGNPurchaseOrder]
    FOREIGN KEY ([GNPurchaseOrderId])
    REFERENCES [gn].[GNPurchaseOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
    
    
-- Creating non-clustered index for FOREIGN KEY 'FK_GNSharedPurchaseOrderOrganizationGNPurchaseOrder'
CREATE INDEX [IX_FK_GNSharedPurchaseOrderOrganizationGNPurchaseOrder]
ON [gn].[GNSharedPurchaseOrderOrganizations]
    ([GNPurchaseOrderId]);