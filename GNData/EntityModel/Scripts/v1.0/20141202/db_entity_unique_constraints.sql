USE [gn_db]
GO

/****** Object:  Index [IX_Unique_ExternalInvoiceNum_GNInvoices]    Script Date: 12/2/2014 10:00:28 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Unique_ExternalInvoiceNum_GNInvoices] ON [gn].[GNInvoices]
(
	[ExternalInvoiceNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/****** Object:  UserDefinedFunction [gn].[GetInvoiceCount]    Script Date: 12/2/2014 10:44:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [gn].[GetInvoiceCount] 
(
	@InvStartDate DATETIME,
	@InvEndDate DATETIME,
	@AcctId UNIQUEIDENTIFIER
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @InvoiceCount int

	-- Add the T-SQL statements to compute the return value here
	SELECT @InvoiceCount = 
		(select count(*) from [gn].[GNInvoices] as inv 
	where inv.InvoiceStartDate = @InvStartDate 
	and inv.InvoiceEndDate = @InvEndDate 
	and inv.GNAccountId = @AcctId);
				
	-- Return the result of the function
	RETURN @InvoiceCount;

END


GO


/** Add Check Constraint to Make Sure Invoice Has Unique Invoice Dates ****************/

ALTER TABLE [gn].[GNInvoices]  WITH NOCHECK 
ADD  CONSTRAINT [CK_Unique_InvDatesForAcct_GNInvoices] 
CHECK  (([gn].[GetInvoiceCount]([InvoiceStartDate],[InvoiceEndDate],[GNAccountId])=(1)))
GO

ALTER TABLE [gn].[GNInvoices] CHECK CONSTRAINT [CK_Unique_InvDatesForAcct_GNInvoices]
GO



/****** Object:  UserDefinedFunction [gn].[GetOrganizationNameCount]    Script Date: 12/2/2014 2:08:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [gn].[GetOrganizationNameCount] 
(
	@OrgName NVARCHAR(MAX)
)
RETURNS int
AS
BEGIN				
	-- Return the result of the function
	RETURN (select count(*) from [gn].[GNOrganizations] as org 
	where org.Name = @OrgName);

END

GO

/*************************************************************************************/

ALTER TABLE [gn].[GNOrganizations]  WITH NOCHECK 
ADD  CONSTRAINT [CK_Unique_OrgName_GNOrganizations] 
CHECK  (([gn].[GetOrganizationNameCount]([Name])=(1)))
GO

ALTER TABLE [gn].[GNOrganizations] CHECK CONSTRAINT [CK_Unique_OrgName_GNOrganizations]
GO


/****** Object:  UserDefinedFunction [gn].[GetTeamNameForOrgCount]    Script Date: 12/2/2014 2:08:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [gn].[GetTeamNameForOrgCount] 
(
	@TeamName NVARCHAR(MAX),
	@OrgId UNIQUEIDENTIFIER
)
RETURNS int
AS
BEGIN				
	-- Return the result of the function
	RETURN (select count(*) from [gn].[GNTeams] as team 
	where team.Name = @TeamName
	and team.OrganizationId = @OrgId);

END

GO

/*************************************************************************************/

ALTER TABLE [gn].[GNTeams]  WITH NOCHECK 
ADD  CONSTRAINT [CK_Unique_TeamNameForOrg_GNTeams] 
CHECK  (([gn].[GetTeamNameForOrgCount]([Name],[OrganizationId])=(1)))
GO

ALTER TABLE [gn].[GNTeams] CHECK CONSTRAINT [CK_Unique_TeamNameForOrg_GNTeams]
GO


/****** Object:  UserDefinedFunction [gn].[GetProjectNameForOrgCount]    Script Date: 12/2/2014 2:08:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [gn].[GetProjectNameForOrgCount] 
(
	@ProjectName NVARCHAR(MAX),
	@ProjectLeadId UNIQUEIDENTIFIER
)
RETURNS int
AS
BEGIN				
	-- Return the result of the function
	RETURN (select count(*) from [gn].[GNProjects] as project 
inner join [gn].[GNContacts] contact ON project.ProjectLead_Id = contact.Id
inner join [gn].[GNOrganizations] org ON contact.GNOrganizationId = org.Id
where project.Name = @ProjectName
and org.Id = (
select distinct org2.Id from [gn].[GNProjects] as project2 
inner join [gn].[GNContacts] contact2 ON project2.ProjectLead_Id = contact2.Id
inner join [gn].[GNOrganizations] org2 ON contact2.GNOrganizationId = org2.Id
where project2.ProjectLead_Id = @ProjectLeadId
));

END

GO

/*************************************************************************************/

ALTER TABLE [gn].[GNProjects]  WITH NOCHECK 
ADD  CONSTRAINT [CK_Unique_ProjectNameForOrg_GNProjects] 
CHECK  (([gn].[GetProjectNameForOrgCount]([Name],[ProjectLead_Id])=(1)))
GO

ALTER TABLE [gn].[GNProjects] CHECK CONSTRAINT [CK_Unique_ProjectNameForOrg_GNProjects]
GO

/****** Object:  UserDefinedFunction [gn].[GetAnalysisNameForProjectCount]    Script Date: 12/2/2014 2:08:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [gn].[GetAnalysisNameForProjectCount] 
(
	@AnalysisName NVARCHAR(MAX),
	@ProjectId UNIQUEIDENTIFIER
)
RETURNS int
AS
BEGIN				
	-- Return the result of the function
	RETURN (select count(*) from [gn].[GNAnalysisRequests] as ar 
	where ar.Description = @AnalysisName
	and ar.GNProjectId = @ProjectId);

END

GO

/*************************************************************************************/

ALTER TABLE [gn].[GNAnalysisRequests]  WITH NOCHECK 
ADD  CONSTRAINT [CK_Unique_AnalysisNameForProject_GNTeams] 
CHECK  (([gn].[GetAnalysisNameForProjectCount]([Description],[GNProjectId])=(1)))
GO

ALTER TABLE [gn].[GNAnalysisRequests] CHECK CONSTRAINT [CK_Unique_AnalysisNameForProject_GNTeams]
GO
