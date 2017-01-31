
ALTER TABLE [gn].[GNAnalysisRequests]
ALTER COLUMN [Description] nvarchar(30);


ALTER TABLE [gn].[GNAnalysisRequests]  WITH NOCHECK ADD  CONSTRAINT [CK_Unique_AnalysisNameForProject_GNTeams] CHECK  (([gn].[GetAnalysisNameForProjectCount]([Description],[GNProjectId])=(1)))
GO

ALTER TABLE [gn].[GNAnalysisRequests] CHECK CONSTRAINT [CK_Unique_AnalysisNameForProject_GNTeams]
GO


