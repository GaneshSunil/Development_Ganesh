alter table gn.GNOrganizations add 
    [GNSettingsTemplate_Id] int  NULL;

IF OBJECT_ID(N'[gn].[FK_GNOrganizationGNSettingsTemplate]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNOrganizations] DROP CONSTRAINT [FK_GNOrganizationGNSettingsTemplate];
GO
IF OBJECT_ID(N'[gn].[FK_GNSettingsTemplateGNSettingsTemplateConfig]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSettingsTemplateConfigs] DROP CONSTRAINT [FK_GNSettingsTemplateGNSettingsTemplateConfig];
GO
IF OBJECT_ID(N'[gn].[FK_GNSettingsTemplateConfigGNSettingsTemplateField]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSettingsTemplateConfigs] DROP CONSTRAINT [FK_GNSettingsTemplateConfigGNSettingsTemplateField];
GO

IF OBJECT_ID(N'[gn].[GNSettingsTemplates]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSettingsTemplates];
GO
IF OBJECT_ID(N'[gn].[GNSettingsTemplateConfigs]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSettingsTemplateConfigs];
GO
IF OBJECT_ID(N'[gn].[GNSettingsTemplateFields]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSettingsTemplateFields];
GO



-- Creating table 'GNSettingsTemplates'
CREATE TABLE [gn].[GNSettingsTemplates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNSettingsTemplateConfigs'
CREATE TABLE [gn].[GNSettingsTemplateConfigs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [GNSettingsTemplate_Id] int  NOT NULL,
    [GNSettingsTemplateField_Id] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'GNSettingsTemplateFields'
CREATE TABLE [gn].[GNSettingsTemplateFields] (
    [Id] nvarchar(50)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO




-- Creating primary key on [Id] in table 'GNSettingsTemplates'
ALTER TABLE [gn].[GNSettingsTemplates]
ADD CONSTRAINT [PK_GNSettingsTemplates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNSettingsTemplateConfigs'
ALTER TABLE [gn].[GNSettingsTemplateConfigs]
ADD CONSTRAINT [PK_GNSettingsTemplateConfigs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNSettingsTemplateFields'
ALTER TABLE [gn].[GNSettingsTemplateFields]
ADD CONSTRAINT [PK_GNSettingsTemplateFields]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO



-- Creating foreign key on [GNSettingsTemplate_Id] in table 'GNOrganizations'
ALTER TABLE [gn].[GNOrganizations]
ADD CONSTRAINT [FK_GNOrganizationGNSettingsTemplate]
    FOREIGN KEY ([GNSettingsTemplate_Id])
    REFERENCES [gn].[GNSettingsTemplates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNOrganizationGNSettingsTemplate'
CREATE INDEX [IX_FK_GNOrganizationGNSettingsTemplate]
ON [gn].[GNOrganizations]
    ([GNSettingsTemplate_Id]);
GO

-- Creating foreign key on [GNSettingsTemplate_Id] in table 'GNSettingsTemplateConfigs'
ALTER TABLE [gn].[GNSettingsTemplateConfigs]
ADD CONSTRAINT [FK_GNSettingsTemplateGNSettingsTemplateConfig]
    FOREIGN KEY ([GNSettingsTemplate_Id])
    REFERENCES [gn].[GNSettingsTemplates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSettingsTemplateGNSettingsTemplateConfig'
CREATE INDEX [IX_FK_GNSettingsTemplateGNSettingsTemplateConfig]
ON [gn].[GNSettingsTemplateConfigs]
    ([GNSettingsTemplate_Id]);
GO

-- Creating foreign key on [GNSettingsTemplateField_Id] in table 'GNSettingsTemplateConfigs'
ALTER TABLE [gn].[GNSettingsTemplateConfigs]
ADD CONSTRAINT [FK_GNSettingsTemplateConfigGNSettingsTemplateField]
    FOREIGN KEY ([GNSettingsTemplateField_Id])
    REFERENCES [gn].[GNSettingsTemplateFields]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSettingsTemplateConfigGNSettingsTemplateField'
CREATE INDEX [IX_FK_GNSettingsTemplateConfigGNSettingsTemplateField]
ON [gn].[GNSettingsTemplateConfigs]
    ([GNSettingsTemplateField_Id]);
GO

