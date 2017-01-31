
-- Creating table 'GNSequencerJobs'
CREATE TABLE [gn].[GNSequencerJobs] (
    [Id] uniqueidentifier  NOT NULL,
    [GNOrganizationId] uniqueidentifier  NOT NULL,
    [Project] nchar(100)  NOT NULL,
    [GNProject_Id] uniqueidentifier  NOT NULL,
    [Status] nchar(25)  NOT NULL,
    [CreateDateTime] datetime  NULL
);
GO
-- Creating primary key on [Id] in table 'GNSequencerJobs'
ALTER TABLE [gn].[GNSequencerJobs]
ADD CONSTRAINT [PK_GNSequencerJobs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating foreign key on [GNOrganizationId] in table 'GNSequencerJobs'
ALTER TABLE [gn].[GNSequencerJobs]
ADD CONSTRAINT [FK_GNSequencerJobGNOrganization]
    FOREIGN KEY ([GNOrganizationId])
    REFERENCES [gn].[GNOrganizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSequencerJobGNOrganization'
CREATE INDEX [IX_FK_GNSequencerJobGNOrganization]
ON [gn].[GNSequencerJobs]
    ([GNOrganizationId]);
GO

-- Creating foreign key on [GNProject_Id] in table 'GNSequencerJobs'
ALTER TABLE [gn].[GNSequencerJobs]
ADD CONSTRAINT [FK_GNSequencerJobGNProject]
    FOREIGN KEY ([GNProject_Id])
    REFERENCES [gn].[GNProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO