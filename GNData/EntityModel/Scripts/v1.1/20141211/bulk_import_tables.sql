-- Creating table 'GNBulkImportStatus'
CREATE TABLE [gn].[GNBulkImportStatus] (
    [Id] uniqueidentifier  NOT NULL,
    [FileURIType] nvarchar(255)  NOT NULL,
    [FileURI] nvarchar(1024)  NOT NULL,
    [FileExtension] nvarchar(25)  NOT NULL,
    [FileMimeType] nvarchar(25)  NULL,
    [TotalRecordCount] bigint  NOT NULL,
    [ImportedRecordCount] bigint  NOT NULL,
    [FailedRecordCount] bigint  NOT NULL,
    [DuplicateRecordCount] bigint  NOT NULL,
    [TeamsCreatedCount] bigint  NOT NULL,
    [ProjectsCreatedCount] bigint  NOT NULL,
    [AnalysesCreatedCount] bigint  NOT NULL,
    [SamplesCreatedCount] bigint  NOT NULL,
    [FilesCreatedCount] bigint  NOT NULL,
    [ImportStartDateTime] datetime  NULL,
    [ImportEndDateTime] datetime  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNBulkImportLogs'
CREATE TABLE [gn].[GNBulkImportLogs] (
    [Id] uniqueidentifier  NOT NULL,
    [RecordRowId] nvarchar(55)  NOT NULL,
    [IsError] bit  NOT NULL,
    [IsDuplicate] bit  NOT NULL,
    [Message] nvarchar(2048)  NOT NULL,
    [GNBulkImportStatusId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating primary key on [Id] in table 'GNBulkImportStatus'
ALTER TABLE [gn].[GNBulkImportStatus]
ADD CONSTRAINT [PK_GNBulkImportStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBulkImportLogs'
ALTER TABLE [gn].[GNBulkImportLogs]
ADD CONSTRAINT [PK_GNBulkImportLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [GNBulkImportStatusId] in table 'GNBulkImportLogs'
ALTER TABLE [gn].[GNBulkImportLogs]
ADD CONSTRAINT [FK_GNBulkImportStatusGNBulkImportLog]
    FOREIGN KEY ([GNBulkImportStatusId])
    REFERENCES [gn].[GNBulkImportStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBulkImportStatusGNBulkImportLog'
CREATE INDEX [IX_FK_GNBulkImportStatusGNBulkImportLog]
ON [gn].[GNBulkImportLogs]
    ([GNBulkImportStatusId]);
GO
