USE [gn_db]
GO

/****** Object:  Table [gn].[GNBulkImportStaging]    Script Date: 12/16/2014 8:43:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [gn].[GNBulkImportStaging](
	[ROW_IDX] [bigint] IDENTITY(1,1) NOT NULL,
	[TEAM_NAME] [nvarchar](255) NULL,
	[PROJECT_NAME] [nvarchar](255) NULL,
	[ANALYSIS_NAME] [nvarchar](255) NULL,
	[SAMPLE_NAME] [nvarchar](255) NULL,
	[SAMPLE_TYPE] [nvarchar](255) NULL,
	[SAMPLE_GENDER] [nvarchar](255) NULL,
	[SAMPLE_READ_TYPE] [nvarchar](255) NULL,
	[FILE_BUCKET] [nvarchar](255) NULL,
	[FILE_KEY] [nvarchar](2048) NULL,
	[FILE_SIZE] [nvarchar](100) NULL,
    [GNBulkImportStatusId] uniqueidentifier  NOT NULL,
 CONSTRAINT [PK_GNBulkImportStaging] PRIMARY KEY CLUSTERED 
(
	[ROW_IDX] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


-- Creating foreign key on [GNBulkImportStatusId] in table 'GNBulkImportStaging'
ALTER TABLE [gn].[GNBulkImportStaging]
ADD CONSTRAINT [FK_GNBulkImportStatusGNBulkImportStaging]
    FOREIGN KEY ([GNBulkImportStatusId])
    REFERENCES [gn].[GNBulkImportStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBulkImportStatusGNBulkImportStaging'
CREATE INDEX [IX_FK_GNBulkImportStatusGNBulkImportStaging]
ON [gn].[GNBulkImportStaging]
    ([GNBulkImportStatusId]);
GO
