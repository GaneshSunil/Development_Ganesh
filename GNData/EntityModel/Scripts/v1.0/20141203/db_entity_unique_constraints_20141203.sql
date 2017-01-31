USE [gn_db]
GO

/****** Object:  UserDefinedFunction [gn].[GetSampleNameForOrgCount]    Script Date: 12/2/2014 2:08:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [gn].[GetSampleNameForOrgCount] 
(
	@SampleName NVARCHAR(MAX),
	@OrgId UNIQUEIDENTIFIER
)
RETURNS int
AS
BEGIN				
	-- Return the result of the function
	RETURN (select count(*) from [gn].[GNSamples] as samp 
	where samp.Name = @SampleName
	and samp.GNOrganizationId = @OrgId);

END

GO

/*************************************************************************************/

ALTER TABLE [gn].[GNSamples]  WITH NOCHECK 
ADD  CONSTRAINT [CK_Unique_SampleNameForOrg_GNTeams] 
CHECK  (([gn].[GetSampleNameForOrgCount]([Name],[GNOrganizationId])=(1)))
GO

ALTER TABLE [gn].[GNSamples] CHECK CONSTRAINT [CK_Unique_SampleNameForOrg_GNTeams]
GO
