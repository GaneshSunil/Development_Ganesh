
-- Creating table 'GNSampleRelationshipTypeMappings'
CREATE TABLE [gn].[GNSampleRelationshipTypeMappings] (
    [NameLeftRelationship] nvarchar(50)  NOT NULL,
    [NameRightRelationship] nvarchar(50)  NOT NULL,
    [GenderRightRelationship] nvarchar(1)  NOT NULL
);
GO



-- Creating primary key on [NameLeftRelationship], [NameRightRelationship] in table 'GNSampleRelationshipTypeMappings'
ALTER TABLE [gn].[GNSampleRelationshipTypeMappings]
ADD CONSTRAINT [PK_GNSampleRelationshipTypeMappings]
    PRIMARY KEY CLUSTERED ([NameLeftRelationship], [NameRightRelationship] ASC);
GO