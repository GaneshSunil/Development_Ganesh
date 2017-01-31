--Create new table
CREATE TABLE [gn].[GNAnalysisRequestGNSamples] (
    [GNAnalysisRequestId] uniqueidentifier  NOT NULL,
    [GNSampleId] uniqueidentifier  NOT NULL,
    [AffectedIndicator] nchar(1)  NOT NULL DEFAULT 'U',
    [TargetIndicator] nchar(1)  NOT NULL DEFAULT 'N'
);

--Primary Key
ALTER TABLE [gn].[GNAnalysisRequestGNSamples]
ADD CONSTRAINT [PK_GNAnalysisRequestGNSamples]
    PRIMARY KEY CLUSTERED ([GNSampleId], [GNAnalysisRequestId] ASC);

--Foreign Keys
ALTER TABLE [gn].[GNAnalysisRequestGNSamples]
ADD CONSTRAINT [FK_GNAnalysisRequestGNSampleGNSample]
    FOREIGN KEY ([GNSampleId])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE [gn].[GNAnalysisRequestGNSamples]
ADD CONSTRAINT [FK_GNAnalysisRequestGNSampleGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

--Migrate values from old table. New fields will be populated with default values.
INSERT INTO [gn].[GNAnalysisRequestGNSamples] (GNAnalysisRequestId, GNSampleId)
(SELECT AnalysisRequests_Id, Samples_Id FROM gn.GNAnalysisRequestGNSample)

