-- Creating table 'AWSComputeEnvironments'
CREATE TABLE [gn].[AWSComputeEnvironments] (
    [Id] nvarchar(255)  NOT NULL,
    [VPC] nvarchar(50)  NOT NULL,
    [Subnet] nvarchar(50)  NOT NULL,
    [AvailZone] nvarchar(50)  NOT NULL,
    [MaxInstanceRequiredPerAnalysis] int  NOT NULL,
    [MaxInstanceExpectedCount] int  NOT NULL,
    [IPAvailCount] int  NOT NULL,
    [InstanceRunningCount] int  NOT NULL,
    [InstancePendingCount] int  NOT NULL,
    [AWSConfigId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating primary key on [Id] in table 'AWSComputeEnvironments'
ALTER TABLE [gn].[AWSComputeEnvironments]
ADD CONSTRAINT [PK_AWSComputeEnvironments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [AWSConfigId] in table 'AWSComputeEnvironments'
ALTER TABLE [gn].[AWSComputeEnvironments]
ADD CONSTRAINT [FK_AWSConfigAWSComputeEnvironment]
    FOREIGN KEY ([AWSConfigId])
    REFERENCES [gn].[AWSConfigs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AWSConfigAWSComputeEnvironment'
CREATE INDEX [IX_FK_AWSConfigAWSComputeEnvironment]
ON [gn].[AWSComputeEnvironments]
    ([AWSConfigId]);
GO