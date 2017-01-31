alter table gn.GNBulkImportStaging ADD [ANALYSIS_TYPE] nvarchar(255)  NOT NULL DEFAULT 'DNA';
alter table gn.GNBulkImportStaging ADD [ANALYSIS_ADAPTOR] nvarchar(255)  NOT NULL  DEFAULT 'NONE';
alter table gn.GNBulkImportStaging ADD [ANALYSIS_GROUP_NAME] nvarchar(255)  NULL;
alter table gn.GNBulkImportStaging ADD [SAMPLE_QUALIFIER] nvarchar(255)  NOT NULL DEFAULT 'DNA';


-- Creating table 'GNBulkImportGroupStagings'
CREATE TABLE [gn].[GNBulkImportGroupStagings] (
    [ROW_IDX] bigint IDENTITY(1,1) NOT NULL,
    [ANALYSIS_NAME] nvarchar(255)  NULL,
    [CONTROL_GROUP_NAME] nvarchar(255)  NOT NULL,
    [COMPARISON_GROUP_NAME] nvarchar(255)  NOT NULL,
    [GNBulkImportStatusId] uniqueidentifier  NOT NULL
);
GO

-- Creating primary key on [ROW_IDX] in table 'GNBulkImportGroupStagings'
ALTER TABLE [gn].[GNBulkImportGroupStagings]
ADD CONSTRAINT [PK_GNBulkImportGroupStagings]
    PRIMARY KEY CLUSTERED ([ROW_IDX] ASC);
GO


-- Creating foreign key on [GNBulkImportStatusId] in table 'GNBulkImportGroupStagings'
ALTER TABLE [gn].[GNBulkImportGroupStagings]
ADD CONSTRAINT [FK_GNBulkImportGroupStagingGNBulkImportStatus]
    FOREIGN KEY ([GNBulkImportStatusId])
    REFERENCES [gn].[GNBulkImportStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBulkImportGroupStagingGNBulkImportStatus'
CREATE INDEX [IX_FK_GNBulkImportGroupStagingGNBulkImportStatus]
ON [gn].[GNBulkImportGroupStagings]
    ([GNBulkImportStatusId]);
GO

