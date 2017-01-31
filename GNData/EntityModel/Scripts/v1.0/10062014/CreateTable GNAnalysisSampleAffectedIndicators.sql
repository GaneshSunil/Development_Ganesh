    
    
-- Creating table 'GNAnalysisSampleAffectedIndicators'
CREATE TABLE [gn].[GNAnalysisSampleAffectedIndicators] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nchar(1)  NOT NULL,
    [Name] nchar(50)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);



-- Creating primary key on [Id] in table 'GNAnalysisSampleAffectedIndicators'
ALTER TABLE [gn].[GNAnalysisSampleAffectedIndicators]
ADD CONSTRAINT [PK_GNAnalysisSampleAffectedIndicators]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
