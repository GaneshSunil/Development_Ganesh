
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/09/2014 15:52:38
-- Generated from EDMX file: C:\dev\vs-workspace\gn\GN Genome Analysis Portal\GNPlatform\GNData\EntityModel\GNEntityModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [gn_db];
GO
IF SCHEMA_ID(N'gn') IS NULL EXECUTE(N'CREATE SCHEMA [gn]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[gn].[FK_GNTeamGNTeamMembers]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTeamMembers] DROP CONSTRAINT [FK_GNTeamGNTeamMembers];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNTeamMembers]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTeamMembers] DROP CONSTRAINT [FK_GNContactGNTeamMembers];
GO
IF OBJECT_ID(N'[gn].[FK_GNOrganizationGNTeam]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTeams] DROP CONSTRAINT [FK_GNOrganizationGNTeam];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNAccount]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAccounts] DROP CONSTRAINT [FK_GNContactGNAccount];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNAccount1]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAccounts] DROP CONSTRAINT [FK_GNContactGNAccount1];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNProject]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNProjects] DROP CONSTRAINT [FK_GNContactGNProject];
GO
IF OBJECT_ID(N'[gn].[FK_GNTeamGNProject_GNTeam]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTeamGNProject] DROP CONSTRAINT [FK_GNTeamGNProject_GNTeam];
GO
IF OBJECT_ID(N'[gn].[FK_GNTeamGNProject_GNProject]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTeamGNProject] DROP CONSTRAINT [FK_GNTeamGNProject_GNProject];
GO
IF OBJECT_ID(N'[gn].[FK_GNCloudFileCategoryGNCloudFile]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNCloudFiles] DROP CONSTRAINT [FK_GNCloudFileCategoryGNCloudFile];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleGNCloudFile_GNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSampleGNCloudFile] DROP CONSTRAINT [FK_GNSampleGNCloudFile_GNSample];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleGNCloudFile_GNCloudFile]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSampleGNCloudFile] DROP CONSTRAINT [FK_GNSampleGNCloudFile_GNCloudFile];
GO
IF OBJECT_ID(N'[gn].[FK_GNProjectGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequests] DROP CONSTRAINT [FK_GNProjectGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGNAnalysisType]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequests] DROP CONSTRAINT [FK_GNAnalysisRequestGNAnalysisType];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisResultGNCloudFile_GNAnalysisResult]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisResultGNCloudFile] DROP CONSTRAINT [FK_GNAnalysisResultGNCloudFile_GNAnalysisResult];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisResultGNCloudFile_GNCloudFile]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisResultGNCloudFile] DROP CONSTRAINT [FK_GNAnalysisResultGNCloudFile_GNCloudFile];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNAccount2]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAccounts] DROP CONSTRAINT [FK_GNContactGNAccount2];
GO
IF OBJECT_ID(N'[gn].[FK_GNAccountGNPaymentMethod]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPaymentMethods] DROP CONSTRAINT [FK_GNAccountGNPaymentMethod];
GO
IF OBJECT_ID(N'[gn].[FK_GNAccountGNInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNInvoices] DROP CONSTRAINT [FK_GNAccountGNInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNInvoiceDetailGNTransaction]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTransactions] DROP CONSTRAINT [FK_GNInvoiceDetailGNTransaction];
GO
IF OBJECT_ID(N'[gn].[FK_GNTransactionTypeGNTransaction]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTransactions] DROP CONSTRAINT [FK_GNTransactionTypeGNTransaction];
GO
IF OBJECT_ID(N'[gn].[FK_GNInvoiceGNInvoiceDetail]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNInvoiceDetails] DROP CONSTRAINT [FK_GNInvoiceGNInvoiceDetail];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNOrganization]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNContacts] DROP CONSTRAINT [FK_GNContactGNOrganization];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNTeam]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTeams] DROP CONSTRAINT [FK_GNContactGNTeam];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNOrganization1]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNOrganizations] DROP CONSTRAINT [FK_GNContactGNOrganization1];
GO
IF OBJECT_ID(N'[gn].[FK_GNOrganizationGNAccount]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAccounts] DROP CONSTRAINT [FK_GNOrganizationGNAccount];
GO
IF OBJECT_ID(N'[gn].[FK_GNOrganizationGNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSamples] DROP CONSTRAINT [FK_GNOrganizationGNSample];
GO
IF OBJECT_ID(N'[gn].[FK_AWSConfigAWSResource]', 'F') IS NOT NULL
    ALTER TABLE [gn].[AWSResources] DROP CONSTRAINT [FK_AWSConfigAWSResource];
GO
IF OBJECT_ID(N'[gn].[FK_AWSResourceTypeAWSResource]', 'F') IS NOT NULL
    ALTER TABLE [gn].[AWSResources] DROP CONSTRAINT [FK_AWSResourceTypeAWSResource];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleTypeGNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSamples] DROP CONSTRAINT [FK_GNSampleTypeGNSample];
GO
IF OBJECT_ID(N'[gn].[FK_AWSRegionAWSConfig]', 'F') IS NOT NULL
    ALTER TABLE [gn].[AWSConfigs] DROP CONSTRAINT [FK_AWSRegionAWSConfig];
GO
IF OBJECT_ID(N'[gn].[FK_AWSRegionGNCloudFile]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNCloudFiles] DROP CONSTRAINT [FK_AWSRegionGNCloudFile];
GO
IF OBJECT_ID(N'[gn].[FK_AWSRegionGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequests] DROP CONSTRAINT [FK_AWSRegionGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_AWSRegionGNAnalysisResult]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisResults] DROP CONSTRAINT [FK_AWSRegionGNAnalysisResult];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisResultGNAnalysisStatus]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisStatus] DROP CONSTRAINT [FK_GNAnalysisResultGNAnalysisStatus];
GO
IF OBJECT_ID(N'[gn].[FK_AWSConfigGNOrganization]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNOrganizations] DROP CONSTRAINT [FK_AWSConfigGNOrganization];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGNAnalysisStatus]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisStatus] DROP CONSTRAINT [FK_GNAnalysisRequestGNAnalysisStatus];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGNAnalysisResult]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisResults] DROP CONSTRAINT [FK_GNAnalysisRequestGNAnalysisResult];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNContactRole]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNContactRoles] DROP CONSTRAINT [FK_GNContactGNContactRole];
GO
IF OBJECT_ID(N'[gn].[FK_GNEntityTypeGNEntityTag]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNEntityTags] DROP CONSTRAINT [FK_GNEntityTypeGNEntityTag];
GO
IF OBJECT_ID(N'[gn].[FK_GNTagGNEntityTag]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNEntityTags] DROP CONSTRAINT [FK_GNTagGNEntityTag];
GO
IF OBJECT_ID(N'[gn].[FK_GNEntityTypeGNEntityAudit]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNEntityAudits] DROP CONSTRAINT [FK_GNEntityTypeGNEntityAudit];
GO
IF OBJECT_ID(N'[gn].[FK_GNEntityAuditTypeGNEntityAudit]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNEntityAudits] DROP CONSTRAINT [FK_GNEntityAuditTypeGNEntityAudit];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleRelationshipTypeGNSampleRelationship]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSampleRelationships] DROP CONSTRAINT [FK_GNSampleRelationshipTypeGNSampleRelationship];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleGNSampleLeftRelationship]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSampleRelationships] DROP CONSTRAINT [FK_GNSampleGNSampleLeftRelationship];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleGNSampleRightRelationship]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSampleRelationships] DROP CONSTRAINT [FK_GNSampleGNSampleRightRelationship];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNEntityAudit]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNEntityAudits] DROP CONSTRAINT [FK_GNContactGNEntityAudit];
GO
IF OBJECT_ID(N'[gn].[FK_GNPaymentMethodTypeGNPaymentMethod]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPaymentMethods] DROP CONSTRAINT [FK_GNPaymentMethodTypeGNPaymentMethod];
GO
IF OBJECT_ID(N'[gn].[FK_GNProductTypeGNProduct]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNProducts] DROP CONSTRAINT [FK_GNProductTypeGNProduct];
GO
IF OBJECT_ID(N'[gn].[FK_GNProductGNAccountProductSubscription]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAccountProductSubscriptions] DROP CONSTRAINT [FK_GNProductGNAccountProductSubscription];
GO
IF OBJECT_ID(N'[gn].[FK_GNAccountGNAccountProductSubscription]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAccountProductSubscriptions] DROP CONSTRAINT [FK_GNAccountGNAccountProductSubscription];
GO
IF OBJECT_ID(N'[gn].[FK_GNProductGNTransaction]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTransactions] DROP CONSTRAINT [FK_GNProductGNTransaction];
GO
IF OBJECT_ID(N'[gn].[FK_GNPaymentGNPaymentMethod]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPayments] DROP CONSTRAINT [FK_GNPaymentGNPaymentMethod];
GO
IF OBJECT_ID(N'[gn].[FK_GNPaymentGNInvoice_GNPayment]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPaymentGNInvoice] DROP CONSTRAINT [FK_GNPaymentGNInvoice_GNPayment];
GO
IF OBJECT_ID(N'[gn].[FK_GNPaymentGNInvoice_GNInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPaymentGNInvoice] DROP CONSTRAINT [FK_GNPaymentGNInvoice_GNInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNAccountTypeGNAccount]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAccounts] DROP CONSTRAINT [FK_GNAccountTypeGNAccount];
GO
IF OBJECT_ID(N'[gn].[FK_GNAccountGNPurchaseOrder]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPurchaseOrders] DROP CONSTRAINT [FK_GNAccountGNPurchaseOrder];
GO
IF OBJECT_ID(N'[gn].[FK_GNAccountGNAccountThreshold]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAccountThresholds] DROP CONSTRAINT [FK_GNAccountGNAccountThreshold];
GO
IF OBJECT_ID(N'[gn].[FK_GNAccountTypeGNProduct]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNProducts] DROP CONSTRAINT [FK_GNAccountTypeGNProduct];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGNSampleGNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGNSamples] DROP CONSTRAINT [FK_GNAnalysisRequestGNSampleGNSample];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGNSampleGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGNSamples] DROP CONSTRAINT [FK_GNAnalysisRequestGNSampleGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_GNPurchaseOrderGNInvoice_GNPurchaseOrder]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPurchaseOrderGNInvoice] DROP CONSTRAINT [FK_GNPurchaseOrderGNInvoice_GNPurchaseOrder];
GO
IF OBJECT_ID(N'[gn].[FK_GNPurchaseOrderGNInvoice_GNInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPurchaseOrderGNInvoice] DROP CONSTRAINT [FK_GNPurchaseOrderGNInvoice_GNInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNNotificationTopicSubscriberGNNotificationTopic]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNNotificationTopicSubscribers] DROP CONSTRAINT [FK_GNNotificationTopicSubscriberGNNotificationTopic];
GO
IF OBJECT_ID(N'[gn].[FK_GNNotificationTopicSubscriberGNContact]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNNotificationTopicSubscribers] DROP CONSTRAINT [FK_GNNotificationTopicSubscriberGNContact];
GO
IF OBJECT_ID(N'[gn].[FK_GNNotificationLogGNNotificationTopic]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNNotificationLogs] DROP CONSTRAINT [FK_GNNotificationLogGNNotificationTopic];
GO
IF OBJECT_ID(N'[gn].[FK_GNNotificationTopicAddresseeGNNotificationTopic]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNNotificationTopicAddressees] DROP CONSTRAINT [FK_GNNotificationTopicAddresseeGNNotificationTopic];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[gn].[GNAccounts]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAccounts];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisRequests]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisRequests];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisResults]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisResults];
GO
IF OBJECT_ID(N'[gn].[GNCloudFileCategories]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNCloudFileCategories];
GO
IF OBJECT_ID(N'[gn].[GNCloudFiles]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNCloudFiles];
GO
IF OBJECT_ID(N'[gn].[GNInvoiceDetails]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNInvoiceDetails];
GO
IF OBJECT_ID(N'[gn].[GNInvoices]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNInvoices];
GO
IF OBJECT_ID(N'[gn].[GNPaymentMethods]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNPaymentMethods];
GO
IF OBJECT_ID(N'[gn].[GNPayments]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNPayments];
GO
IF OBJECT_ID(N'[gn].[GNTransactions]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNTransactions];
GO
IF OBJECT_ID(N'[gn].[GNTransactionTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNTransactionTypes];
GO
IF OBJECT_ID(N'[gn].[GNOrganizations]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNOrganizations];
GO
IF OBJECT_ID(N'[gn].[GNTeams]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNTeams];
GO
IF OBJECT_ID(N'[gn].[GNContacts]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNContacts];
GO
IF OBJECT_ID(N'[gn].[GNTeamMembers]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNTeamMembers];
GO
IF OBJECT_ID(N'[gn].[GNProjects]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNProjects];
GO
IF OBJECT_ID(N'[gn].[GNSamples]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSamples];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisTypes];
GO
IF OBJECT_ID(N'[gn].[GNTags]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNTags];
GO
IF OBJECT_ID(N'[gn].[GNEntityTags]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNEntityTags];
GO
IF OBJECT_ID(N'[gn].[GNProducts]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNProducts];
GO
IF OBJECT_ID(N'[gn].[GNProductTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNProductTypes];
GO
IF OBJECT_ID(N'[gn].[GNEntityTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNEntityTypes];
GO
IF OBJECT_ID(N'[gn].[AWSConfigs]', 'U') IS NOT NULL
    DROP TABLE [gn].[AWSConfigs];
GO
IF OBJECT_ID(N'[gn].[AWSResources]', 'U') IS NOT NULL
    DROP TABLE [gn].[AWSResources];
GO
IF OBJECT_ID(N'[gn].[AWSResourceTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[AWSResourceTypes];
GO
IF OBJECT_ID(N'[gn].[GNSampleTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSampleTypes];
GO
IF OBJECT_ID(N'[gn].[AWSRegions]', 'U') IS NOT NULL
    DROP TABLE [gn].[AWSRegions];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisStatus]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisStatus];
GO
IF OBJECT_ID(N'[gn].[GNContactRoles]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNContactRoles];
GO
IF OBJECT_ID(N'[gn].[GNEntityAudits]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNEntityAudits];
GO
IF OBJECT_ID(N'[gn].[GNEntityAuditTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNEntityAuditTypes];
GO
IF OBJECT_ID(N'[gn].[GNSampleRelationships]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSampleRelationships];
GO
IF OBJECT_ID(N'[gn].[GNSampleRelationshipTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSampleRelationshipTypes];
GO
IF OBJECT_ID(N'[gn].[GNLogs]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNLogs];
GO
IF OBJECT_ID(N'[gn].[GNInviteCodes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNInviteCodes];
GO
IF OBJECT_ID(N'[gn].[GNPaymentMethodTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNPaymentMethodTypes];
GO
IF OBJECT_ID(N'[gn].[GNAccountProductSubscriptions]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAccountProductSubscriptions];
GO
IF OBJECT_ID(N'[gn].[GNNotificationTopics]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNotificationTopics];
GO
IF OBJECT_ID(N'[gn].[GNNotificationLogs]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNotificationLogs];
GO
IF OBJECT_ID(N'[gn].[GNAccountTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAccountTypes];
GO
IF OBJECT_ID(N'[gn].[GNPurchaseOrders]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNPurchaseOrders];
GO
IF OBJECT_ID(N'[gn].[GNAccountThresholds]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAccountThresholds];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisRequestGNSamples]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisRequestGNSamples];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisSampleAffectedIndicators]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisSampleAffectedIndicators];
GO
IF OBJECT_ID(N'[gn].[GNPurchaseOrderGNInvoice]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNPurchaseOrderGNInvoice];
GO
IF OBJECT_ID(N'[gn].[GNNotificationTopicSubscribers]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNotificationTopicSubscribers];
GO
IF OBJECT_ID(N'[gn].[GNNotificationTopicAddressees]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNotificationTopicAddressees];
GO
IF OBJECT_ID(N'[gn].[GNNotificationSenders]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNotificationSenders];
GO
IF OBJECT_ID(N'[gn].[GNTeamGNProject]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNTeamGNProject];
GO
IF OBJECT_ID(N'[gn].[GNSampleGNCloudFile]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSampleGNCloudFile];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisResultGNCloudFile]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisResultGNCloudFile];
GO
IF OBJECT_ID(N'[gn].[GNPaymentGNInvoice]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNPaymentGNInvoice];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'GNAccounts'
CREATE TABLE [gn].[GNAccounts] (
    [Id] uniqueidentifier  NOT NULL,
    [TotalAmountPaid] float  NOT NULL,
    [TotalAmountOwed] float  NOT NULL,
    [DefaultDiscountType] nchar(1)  NULL,
    [DefaultDiscountAmount] float  NOT NULL,
    [BillingMode] nchar(1)  NOT NULL,
    [MaxBalanceAllowed] float  NOT NULL,
    [ValidBillingAgreementRequired] bit  NOT NULL,
    [GNAccountTypeId] int  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [MailingContact_Id] uniqueidentifier  NULL,
    [BillingContact_Id] uniqueidentifier  NULL,
    [AccountOwner_Id] uniqueidentifier  NULL,
    [Organization_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNAnalysisRequests'
CREATE TABLE [gn].[GNAnalysisRequests] (
    [Id] uniqueidentifier  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [RequestProgress] float  NULL,
    [RequestDateTime] datetime  NULL,
    [GNProjectId] uniqueidentifier  NOT NULL,
    [AWSRegionSystemName] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [AnalysisType_Id] int  NOT NULL
);
GO

-- Creating table 'GNAnalysisResults'
CREATE TABLE [gn].[GNAnalysisResults] (
    [Id] uniqueidentifier  NOT NULL,
    [Success] bit  NOT NULL,
    [AnalysisStartDateTime] datetime  NOT NULL,
    [AnalysisEndDateTime] datetime  NOT NULL,
    [AWSRegionSystemName] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [AnalysisRequest_Id] uniqueidentifier  NULL
);
GO

-- Creating table 'GNCloudFileCategories'
CREATE TABLE [gn].[GNCloudFileCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Type] nvarchar(20)  NOT NULL,
    [FileExtensions] nvarchar(max)  NOT NULL,
    [FolderPath] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNCloudFiles'
CREATE TABLE [gn].[GNCloudFiles] (
    [Id] uniqueidentifier  NOT NULL,
    [FileURL] nvarchar(max)  NOT NULL,
    [Volume] nvarchar(max)  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [FolderPath] nvarchar(max)  NOT NULL,
    [FileSize] bigint  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [GNCloudFileCategoryId] int  NOT NULL,
    [AWSRegionSystemName] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNInvoiceDetails'
CREATE TABLE [gn].[GNInvoiceDetails] (
    [Id] uniqueidentifier  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [UnitCost] float  NOT NULL,
    [UnitPrice] float  NOT NULL,
    [Quantity] float  NOT NULL,
    [DiscountType] nchar(1)  NULL,
    [DiscountAmount] float  NOT NULL,
    [SubTotal] float  NOT NULL,
    [Total] float  NOT NULL,
    [GNInvoiceId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNInvoices'
CREATE TABLE [gn].[GNInvoices] (
    [Id] uniqueidentifier  NOT NULL,
    [ExternalInvoiceNum] nvarchar(75)  NULL,
    [InvoiceStartDate] datetime  NOT NULL,
    [InvoiceEndDate] datetime  NOT NULL,
    [Status] varchar(20)  NOT NULL,
    [TotalDiscountAmount] float  NOT NULL,
    [SubTotal] float  NOT NULL,
    [Total] float  NOT NULL,
    [NetTerms] int  NOT NULL,
    [GNAccountId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNPaymentMethods'
CREATE TABLE [gn].[GNPaymentMethods] (
    [Id] uniqueidentifier  NOT NULL,
    [GNPaymentMethodTypeId] int  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [LastFourDigits] nvarchar(max)  NOT NULL,
    [PCITokenId] nvarchar(max)  NOT NULL,
    [IsDefault] bit  NOT NULL,
    [IsActive] bit  NOT NULL,
    [UsedForRecurrentPayments] bit  NOT NULL,
    [ExpirationDate] datetime  NOT NULL,
    [GNAccountId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNPayments'
CREATE TABLE [gn].[GNPayments] (
    [Id] uniqueidentifier  NOT NULL,
    [TotalAmount] float  NOT NULL,
    [PaymentDate] datetime  NOT NULL,
    [ExternalTxnId] nvarchar(100)  NOT NULL,
    [GNPaymentMethodId] uniqueidentifier  NULL,
    [Status] nvarchar(20)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNTransactions'
CREATE TABLE [gn].[GNTransactions] (
    [Id] uniqueidentifier  NOT NULL,
    [GNInvoiceDetailId] uniqueidentifier  NOT NULL,
    [GNTransactionTypeId] int  NOT NULL,
    [GNProductId] uniqueidentifier  NULL,
    [Description] varchar(255)  NULL,
    [ExternalTxnId] nvarchar(100)  NULL,
    [ValueUsed] float  NOT NULL,
    [ValueUnits] nvarchar(100)  NOT NULL,
    [TotalCost] float  NOT NULL,
    [TotalPrice] float  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNTransactionTypes'
CREATE TABLE [gn].[GNTransactionTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(100)  NULL,
    [Description] varchar(max)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNOrganizations'
CREATE TABLE [gn].[GNOrganizations] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [GNContactId] uniqueidentifier  NULL,
    [AWSConfigId] uniqueidentifier  NOT NULL,
    [UTCOffset] nvarchar(10)  NOT NULL,
    [UTCOffsetDescription] nchar(255)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNTeams'
CREATE TABLE [gn].[GNTeams] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [OrganizationId] uniqueidentifier  NOT NULL,
    [GNContactId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNContacts'
CREATE TABLE [gn].[GNContacts] (
    [Id] uniqueidentifier  NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Website] nvarchar(max)  NULL,
    [StreetAddress1] nvarchar(max)  NULL,
    [StreetAddress2] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [State] nvarchar(10)  NULL,
    [Zip] nvarchar(15)  NULL,
    [IsInviteAccepted] bit  NULL,
    [TermsAcceptDateTime] datetime  NULL,
    [PrivacyPolicyAcceptDateTime] datetime  NULL,
    [AspNetUserId] nvarchar(max)  NOT NULL,
    [GNOrganizationId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNTeamMembers'
CREATE TABLE [gn].[GNTeamMembers] (
    [GNTeamId] uniqueidentifier  NOT NULL,
    [GNContactId] uniqueidentifier  NOT NULL,
    [AssignDate] datetime  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNProjects'
CREATE TABLE [gn].[GNProjects] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [ProjectLead_Id] uniqueidentifier  NULL
);
GO

-- Creating table 'GNSamples'
CREATE TABLE [gn].[GNSamples] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsReady] bit  NOT NULL,
    [IsPairEnded] bit  NOT NULL,
    [Gender] nchar(1)  NULL,
    [GNOrganizationId] uniqueidentifier  NOT NULL,
    [GNSampleTypeId] int  NOT NULL,
    [Tags] nvarchar(max)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNAnalysisTypes'
CREATE TABLE [gn].[GNAnalysisTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [TypeValue] nvarchar(50)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNTags'
CREATE TABLE [gn].[GNTags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNEntityTags'
CREATE TABLE [gn].[GNEntityTags] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [EntityId] uniqueidentifier  NOT NULL,
    [GNEntityTypeId] int  NOT NULL,
    [GNTagId] int  NOT NULL,
    [TagDate] datetime  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNProducts'
CREATE TABLE [gn].[GNProducts] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] varchar(100)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [GNProductTypeId] int  NOT NULL,
    [SubscribeFrequency] int  NOT NULL,
    [Cost] float  NOT NULL,
    [Price] float  NOT NULL,
    [MinRangeValue] float  NOT NULL,
    [MaxRangeValue] float  NOT NULL,
    [ValueUnits] nvarchar(100)  NOT NULL,
    [GNAccountTypeId] int  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNProductTypes'
CREATE TABLE [gn].[GNProductTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(100)  NOT NULL,
    [Description] varchar(max)  NOT NULL,
    [IsEligibleForDiscount] bit  NOT NULL,
    [CanSubscribe] bit  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNEntityTypes'
CREATE TABLE [gn].[GNEntityTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'AWSConfigs'
CREATE TABLE [gn].[AWSConfigs] (
    [Id] uniqueidentifier  NOT NULL,
    [AWSAccessKeyId] nvarchar(max)  NOT NULL,
    [AWSSecretAccessKey] nvarchar(max)  NOT NULL,
    [AWSRegionSystemName] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'AWSResources'
CREATE TABLE [gn].[AWSResources] (
    [Id] uniqueidentifier  NOT NULL,
    [ARN] nvarchar(max)  NOT NULL,
    [AWSConfigId] uniqueidentifier  NOT NULL,
    [AWSResourceTypeId] int  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'AWSResourceTypes'
CREATE TABLE [gn].[AWSResourceTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNSampleTypes'
CREATE TABLE [gn].[GNSampleTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [TypeValue] nvarchar(50)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'AWSRegions'
CREATE TABLE [gn].[AWSRegions] (
    [AWSRegionSystemName] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNAnalysisStatus'
CREATE TABLE [gn].[GNAnalysisStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [PercentComplete] int  NOT NULL,
    [IsError] bit  NOT NULL,
    [GNAnalysisResultId] uniqueidentifier  NULL,
    [GNAnalysisRequestId] uniqueidentifier  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNContactRoles'
CREATE TABLE [gn].[GNContactRoles] (
    [GNContactId] uniqueidentifier  NOT NULL,
    [AspNetRoleId] nvarchar(128)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNEntityAudits'
CREATE TABLE [gn].[GNEntityAudits] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [EntityId] nvarchar(max)  NOT NULL,
    [GNEntityTypeId] int  NOT NULL,
    [GNEntityAuditTypeId] int  NOT NULL,
    [GNContactId] uniqueidentifier  NOT NULL,
    [AuditDate] nvarchar(max)  NOT NULL,
    [ValueOld] nvarchar(max)  NULL,
    [ValueNew] nvarchar(max)  NULL
);
GO

-- Creating table 'GNEntityAuditTypes'
CREATE TABLE [gn].[GNEntityAuditTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNSampleRelationships'
CREATE TABLE [gn].[GNSampleRelationships] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GNLeftSampleId] uniqueidentifier  NOT NULL,
    [GNRightSampleId] uniqueidentifier  NOT NULL,
    [GNSampleRelationshipTypeId] int  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNSampleRelationshipTypes'
CREATE TABLE [gn].[GNSampleRelationshipTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [Gender] nchar(1)  NOT NULL,
    [MaxRelationships] int  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNLogs'
CREATE TABLE [gn].[GNLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Server] nvarchar(255)  NOT NULL,
    [Thread] nvarchar(255)  NOT NULL,
    [Level] nvarchar(50)  NOT NULL,
    [Logger] nvarchar(255)  NOT NULL,
    [Message] nvarchar(4000)  NOT NULL,
    [Exception] nvarchar(4000)  NOT NULL
);
GO

-- Creating table 'GNInviteCodes'
CREATE TABLE [gn].[GNInviteCodes] (
    [InviteCode] nvarchar(100)  NOT NULL,
    [Description] nvarchar(255)  NOT NULL,
    [UseCount] int  NOT NULL,
    [UseMaxAllowed] int  NULL,
    [ExpireDate] datetime  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNPaymentMethodTypes'
CREATE TABLE [gn].[GNPaymentMethodTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNAccountProductSubscriptions'
CREATE TABLE [gn].[GNAccountProductSubscriptions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GNAccountId] uniqueidentifier  NOT NULL,
    [GNProductId] uniqueidentifier  NOT NULL,
    [CurrentValueUsed] float  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [SubscribeFrequency] int  NOT NULL,
    [IsActive] bit  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNNotificationTopics'
CREATE TABLE [gn].[GNNotificationTopics] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [Title] nvarchar(50)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Sender] nvarchar(100)  NOT NULL,
    [Format] nvarchar(5)  NOT NULL,
    [Priority] nvarchar(max)  NOT NULL,
    [Subject] nvarchar(70)  NULL,
    [Message] nvarchar(max)  NULL,
    [MessageTemplatePath] nvarchar(max)  NULL,
    [IsSubscriptionOptional] nchar(1)  NOT NULL,
    [NotifyObjectCreator] nchar(1)  NULL,
    [Status] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNNotificationLogs'
CREATE TABLE [gn].[GNNotificationLogs] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [GNNotificationTopicId] int  NOT NULL,
    [GNNotificationTopicCode] nvarchar(50)  NOT NULL,
    [Addressee] nvarchar(max)  NOT NULL,
    [Sender] nvarchar(max)  NOT NULL,
    [Subject] nvarchar(max)  NULL,
    [Message] nvarchar(max)  NOT NULL,
    [NotificationServiceResponse] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNAccountTypes'
CREATE TABLE [gn].[GNAccountTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNPurchaseOrders'
CREATE TABLE [gn].[GNPurchaseOrders] (
    [Id] uniqueidentifier  NOT NULL,
    [ExternalPONum] nvarchar(50)  NULL,
    [Total] float  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [GNAccountId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNAccountThresholds'
CREATE TABLE [gn].[GNAccountThresholds] (
    [Id] uniqueidentifier  NOT NULL,
    [ThresholdAmount] float  NOT NULL,
    [Rank] int  NOT NULL,
    [ThresholdType] nvarchar(25)  NOT NULL,
    [ThresholdInterval] int  NOT NULL,
    [GNAccountId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNAnalysisRequestGNSamples'
CREATE TABLE [gn].[GNAnalysisRequestGNSamples] (
    [GNAnalysisRequestId] uniqueidentifier  NOT NULL,
    [GNSampleId] uniqueidentifier  NOT NULL,
    [AffectedIndicator] nchar(1)  NOT NULL DEFAULT 'N',
    [TargetIndicator] nchar(1)  NOT NULL DEFAULT 'N'
);
GO

-- Creating table 'GNAnalysisSampleAffectedIndicators'
CREATE TABLE [gn].[GNAnalysisSampleAffectedIndicators] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nchar(1)  NOT NULL,
    [Name] nchar(50)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNPurchaseOrderGNInvoice'
CREATE TABLE [gn].[GNPurchaseOrderGNInvoice] (
    [PurchaseOrders_Id] uniqueidentifier  NOT NULL,
    [Invoices_Id] uniqueidentifier  NOT NULL,
    [TotalApplied] float  NOT NULL
);
GO

-- Creating table 'GNNotificationTopicSubscribers'
CREATE TABLE [gn].[GNNotificationTopicSubscribers] (
    [GNNotificationTopicId] int  NOT NULL,
    [GNContactId] uniqueidentifier  NOT NULL,
    [AddresseeType] nvarchar(3)  NOT NULL,
    [IsSubscribed] nchar(1)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNNotificationTopicAddressees'
CREATE TABLE [gn].[GNNotificationTopicAddressees] (
    [GNNotificationTopicId] int  NOT NULL,
    [AspNetRoleId] nvarchar(128)  NOT NULL,
    [AddresseeType] nvarchar(3)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO

-- Creating table 'GNNotificationSenders'
CREATE TABLE [gn].[GNNotificationSenders] (
    [Sender] nchar(50)  NOT NULL,
    [Name] nchar(100)  NOT NULL
);
GO

-- Creating table 'GNTeamGNProject'
CREATE TABLE [gn].[GNTeamGNProject] (
    [Teams_Id] uniqueidentifier  NOT NULL,
    [Projects_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNSampleGNCloudFile'
CREATE TABLE [gn].[GNSampleGNCloudFile] (
    [GNSampleGNCloudFile_GNCloudFile_Id] uniqueidentifier  NOT NULL,
    [CloudFiles_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNAnalysisResultGNCloudFile'
CREATE TABLE [gn].[GNAnalysisResultGNCloudFile] (
    [GNAnalysisResultGNCloudFile_GNCloudFile_Id] uniqueidentifier  NOT NULL,
    [ResultFiles_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNPaymentGNInvoice'
CREATE TABLE [gn].[GNPaymentGNInvoice] (
    [Payments_Id] uniqueidentifier  NOT NULL,
    [Invoices_Id] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'GNAccounts'
ALTER TABLE [gn].[GNAccounts]
ADD CONSTRAINT [PK_GNAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisRequests'
ALTER TABLE [gn].[GNAnalysisRequests]
ADD CONSTRAINT [PK_GNAnalysisRequests]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisResults'
ALTER TABLE [gn].[GNAnalysisResults]
ADD CONSTRAINT [PK_GNAnalysisResults]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNCloudFileCategories'
ALTER TABLE [gn].[GNCloudFileCategories]
ADD CONSTRAINT [PK_GNCloudFileCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNCloudFiles'
ALTER TABLE [gn].[GNCloudFiles]
ADD CONSTRAINT [PK_GNCloudFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNInvoiceDetails'
ALTER TABLE [gn].[GNInvoiceDetails]
ADD CONSTRAINT [PK_GNInvoiceDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNInvoices'
ALTER TABLE [gn].[GNInvoices]
ADD CONSTRAINT [PK_GNInvoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNPaymentMethods'
ALTER TABLE [gn].[GNPaymentMethods]
ADD CONSTRAINT [PK_GNPaymentMethods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNPayments'
ALTER TABLE [gn].[GNPayments]
ADD CONSTRAINT [PK_GNPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNTransactions'
ALTER TABLE [gn].[GNTransactions]
ADD CONSTRAINT [PK_GNTransactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNTransactionTypes'
ALTER TABLE [gn].[GNTransactionTypes]
ADD CONSTRAINT [PK_GNTransactionTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNOrganizations'
ALTER TABLE [gn].[GNOrganizations]
ADD CONSTRAINT [PK_GNOrganizations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNTeams'
ALTER TABLE [gn].[GNTeams]
ADD CONSTRAINT [PK_GNTeams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNContacts'
ALTER TABLE [gn].[GNContacts]
ADD CONSTRAINT [PK_GNContacts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [GNTeamId], [GNContactId] in table 'GNTeamMembers'
ALTER TABLE [gn].[GNTeamMembers]
ADD CONSTRAINT [PK_GNTeamMembers]
    PRIMARY KEY CLUSTERED ([GNTeamId], [GNContactId] ASC);
GO

-- Creating primary key on [Id] in table 'GNProjects'
ALTER TABLE [gn].[GNProjects]
ADD CONSTRAINT [PK_GNProjects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNSamples'
ALTER TABLE [gn].[GNSamples]
ADD CONSTRAINT [PK_GNSamples]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisTypes'
ALTER TABLE [gn].[GNAnalysisTypes]
ADD CONSTRAINT [PK_GNAnalysisTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNTags'
ALTER TABLE [gn].[GNTags]
ADD CONSTRAINT [PK_GNTags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNEntityTags'
ALTER TABLE [gn].[GNEntityTags]
ADD CONSTRAINT [PK_GNEntityTags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNProducts'
ALTER TABLE [gn].[GNProducts]
ADD CONSTRAINT [PK_GNProducts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNProductTypes'
ALTER TABLE [gn].[GNProductTypes]
ADD CONSTRAINT [PK_GNProductTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNEntityTypes'
ALTER TABLE [gn].[GNEntityTypes]
ADD CONSTRAINT [PK_GNEntityTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AWSConfigs'
ALTER TABLE [gn].[AWSConfigs]
ADD CONSTRAINT [PK_AWSConfigs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AWSResources'
ALTER TABLE [gn].[AWSResources]
ADD CONSTRAINT [PK_AWSResources]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AWSResourceTypes'
ALTER TABLE [gn].[AWSResourceTypes]
ADD CONSTRAINT [PK_AWSResourceTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNSampleTypes'
ALTER TABLE [gn].[GNSampleTypes]
ADD CONSTRAINT [PK_GNSampleTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AWSRegionSystemName] in table 'AWSRegions'
ALTER TABLE [gn].[AWSRegions]
ADD CONSTRAINT [PK_AWSRegions]
    PRIMARY KEY CLUSTERED ([AWSRegionSystemName] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisStatus'
ALTER TABLE [gn].[GNAnalysisStatus]
ADD CONSTRAINT [PK_GNAnalysisStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [GNContactId], [AspNetRoleId] in table 'GNContactRoles'
ALTER TABLE [gn].[GNContactRoles]
ADD CONSTRAINT [PK_GNContactRoles]
    PRIMARY KEY CLUSTERED ([GNContactId], [AspNetRoleId] ASC);
GO

-- Creating primary key on [Id] in table 'GNEntityAudits'
ALTER TABLE [gn].[GNEntityAudits]
ADD CONSTRAINT [PK_GNEntityAudits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNEntityAuditTypes'
ALTER TABLE [gn].[GNEntityAuditTypes]
ADD CONSTRAINT [PK_GNEntityAuditTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNSampleRelationships'
ALTER TABLE [gn].[GNSampleRelationships]
ADD CONSTRAINT [PK_GNSampleRelationships]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNSampleRelationshipTypes'
ALTER TABLE [gn].[GNSampleRelationshipTypes]
ADD CONSTRAINT [PK_GNSampleRelationshipTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNLogs'
ALTER TABLE [gn].[GNLogs]
ADD CONSTRAINT [PK_GNLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [InviteCode] in table 'GNInviteCodes'
ALTER TABLE [gn].[GNInviteCodes]
ADD CONSTRAINT [PK_GNInviteCodes]
    PRIMARY KEY CLUSTERED ([InviteCode] ASC);
GO

-- Creating primary key on [Id] in table 'GNPaymentMethodTypes'
ALTER TABLE [gn].[GNPaymentMethodTypes]
ADD CONSTRAINT [PK_GNPaymentMethodTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAccountProductSubscriptions'
ALTER TABLE [gn].[GNAccountProductSubscriptions]
ADD CONSTRAINT [PK_GNAccountProductSubscriptions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNNotificationTopics'
ALTER TABLE [gn].[GNNotificationTopics]
ADD CONSTRAINT [PK_GNNotificationTopics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNNotificationLogs'
ALTER TABLE [gn].[GNNotificationLogs]
ADD CONSTRAINT [PK_GNNotificationLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAccountTypes'
ALTER TABLE [gn].[GNAccountTypes]
ADD CONSTRAINT [PK_GNAccountTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNPurchaseOrders'
ALTER TABLE [gn].[GNPurchaseOrders]
ADD CONSTRAINT [PK_GNPurchaseOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAccountThresholds'
ALTER TABLE [gn].[GNAccountThresholds]
ADD CONSTRAINT [PK_GNAccountThresholds]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [GNSampleId], [GNAnalysisRequestId] in table 'GNAnalysisRequestGNSamples'
ALTER TABLE [gn].[GNAnalysisRequestGNSamples]
ADD CONSTRAINT [PK_GNAnalysisRequestGNSamples]
    PRIMARY KEY CLUSTERED ([GNSampleId], [GNAnalysisRequestId] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisSampleAffectedIndicators'
ALTER TABLE [gn].[GNAnalysisSampleAffectedIndicators]
ADD CONSTRAINT [PK_GNAnalysisSampleAffectedIndicators]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [PurchaseOrders_Id], [Invoices_Id] in table 'GNPurchaseOrderGNInvoice'
ALTER TABLE [gn].[GNPurchaseOrderGNInvoice]
ADD CONSTRAINT [PK_GNPurchaseOrderGNInvoice]
    PRIMARY KEY CLUSTERED ([PurchaseOrders_Id], [Invoices_Id] ASC);
GO

-- Creating primary key on [GNNotificationTopicId], [GNContactId], [AddresseeType] in table 'GNNotificationTopicSubscribers'
ALTER TABLE [gn].[GNNotificationTopicSubscribers]
ADD CONSTRAINT [PK_GNNotificationTopicSubscribers]
    PRIMARY KEY CLUSTERED ([GNNotificationTopicId], [GNContactId], [AddresseeType] ASC);
GO

-- Creating primary key on [GNNotificationTopicId], [AspNetRoleId], [AddresseeType] in table 'GNNotificationTopicAddressees'
ALTER TABLE [gn].[GNNotificationTopicAddressees]
ADD CONSTRAINT [PK_GNNotificationTopicAddressees]
    PRIMARY KEY CLUSTERED ([GNNotificationTopicId], [AspNetRoleId], [AddresseeType] ASC);
GO

-- Creating primary key on [Sender] in table 'GNNotificationSenders'
ALTER TABLE [gn].[GNNotificationSenders]
ADD CONSTRAINT [PK_GNNotificationSenders]
    PRIMARY KEY CLUSTERED ([Sender] ASC);
GO

-- Creating primary key on [Teams_Id], [Projects_Id] in table 'GNTeamGNProject'
ALTER TABLE [gn].[GNTeamGNProject]
ADD CONSTRAINT [PK_GNTeamGNProject]
    PRIMARY KEY CLUSTERED ([Teams_Id], [Projects_Id] ASC);
GO

-- Creating primary key on [GNSampleGNCloudFile_GNCloudFile_Id], [CloudFiles_Id] in table 'GNSampleGNCloudFile'
ALTER TABLE [gn].[GNSampleGNCloudFile]
ADD CONSTRAINT [PK_GNSampleGNCloudFile]
    PRIMARY KEY CLUSTERED ([GNSampleGNCloudFile_GNCloudFile_Id], [CloudFiles_Id] ASC);
GO

-- Creating primary key on [GNAnalysisResultGNCloudFile_GNCloudFile_Id], [ResultFiles_Id] in table 'GNAnalysisResultGNCloudFile'
ALTER TABLE [gn].[GNAnalysisResultGNCloudFile]
ADD CONSTRAINT [PK_GNAnalysisResultGNCloudFile]
    PRIMARY KEY CLUSTERED ([GNAnalysisResultGNCloudFile_GNCloudFile_Id], [ResultFiles_Id] ASC);
GO

-- Creating primary key on [Payments_Id], [Invoices_Id] in table 'GNPaymentGNInvoice'
ALTER TABLE [gn].[GNPaymentGNInvoice]
ADD CONSTRAINT [PK_GNPaymentGNInvoice]
    PRIMARY KEY CLUSTERED ([Payments_Id], [Invoices_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [GNTeamId] in table 'GNTeamMembers'
ALTER TABLE [gn].[GNTeamMembers]
ADD CONSTRAINT [FK_GNTeamGNTeamMembers]
    FOREIGN KEY ([GNTeamId])
    REFERENCES [gn].[GNTeams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GNContactId] in table 'GNTeamMembers'
ALTER TABLE [gn].[GNTeamMembers]
ADD CONSTRAINT [FK_GNContactGNTeamMembers]
    FOREIGN KEY ([GNContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNTeamMembers'
CREATE INDEX [IX_FK_GNContactGNTeamMembers]
ON [gn].[GNTeamMembers]
    ([GNContactId]);
GO

-- Creating foreign key on [OrganizationId] in table 'GNTeams'
ALTER TABLE [gn].[GNTeams]
ADD CONSTRAINT [FK_GNOrganizationGNTeam]
    FOREIGN KEY ([OrganizationId])
    REFERENCES [gn].[GNOrganizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNOrganizationGNTeam'
CREATE INDEX [IX_FK_GNOrganizationGNTeam]
ON [gn].[GNTeams]
    ([OrganizationId]);
GO

-- Creating foreign key on [MailingContact_Id] in table 'GNAccounts'
ALTER TABLE [gn].[GNAccounts]
ADD CONSTRAINT [FK_GNContactGNAccount]
    FOREIGN KEY ([MailingContact_Id])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNAccount'
CREATE INDEX [IX_FK_GNContactGNAccount]
ON [gn].[GNAccounts]
    ([MailingContact_Id]);
GO

-- Creating foreign key on [BillingContact_Id] in table 'GNAccounts'
ALTER TABLE [gn].[GNAccounts]
ADD CONSTRAINT [FK_GNContactGNAccount1]
    FOREIGN KEY ([BillingContact_Id])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNAccount1'
CREATE INDEX [IX_FK_GNContactGNAccount1]
ON [gn].[GNAccounts]
    ([BillingContact_Id]);
GO

-- Creating foreign key on [ProjectLead_Id] in table 'GNProjects'
ALTER TABLE [gn].[GNProjects]
ADD CONSTRAINT [FK_GNContactGNProject]
    FOREIGN KEY ([ProjectLead_Id])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNProject'
CREATE INDEX [IX_FK_GNContactGNProject]
ON [gn].[GNProjects]
    ([ProjectLead_Id]);
GO

-- Creating foreign key on [Teams_Id] in table 'GNTeamGNProject'
ALTER TABLE [gn].[GNTeamGNProject]
ADD CONSTRAINT [FK_GNTeamGNProject_GNTeam]
    FOREIGN KEY ([Teams_Id])
    REFERENCES [gn].[GNTeams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Projects_Id] in table 'GNTeamGNProject'
ALTER TABLE [gn].[GNTeamGNProject]
ADD CONSTRAINT [FK_GNTeamGNProject_GNProject]
    FOREIGN KEY ([Projects_Id])
    REFERENCES [gn].[GNProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTeamGNProject_GNProject'
CREATE INDEX [IX_FK_GNTeamGNProject_GNProject]
ON [gn].[GNTeamGNProject]
    ([Projects_Id]);
GO

-- Creating foreign key on [GNCloudFileCategoryId] in table 'GNCloudFiles'
ALTER TABLE [gn].[GNCloudFiles]
ADD CONSTRAINT [FK_GNCloudFileCategoryGNCloudFile]
    FOREIGN KEY ([GNCloudFileCategoryId])
    REFERENCES [gn].[GNCloudFileCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNCloudFileCategoryGNCloudFile'
CREATE INDEX [IX_FK_GNCloudFileCategoryGNCloudFile]
ON [gn].[GNCloudFiles]
    ([GNCloudFileCategoryId]);
GO

-- Creating foreign key on [GNSampleGNCloudFile_GNCloudFile_Id] in table 'GNSampleGNCloudFile'
ALTER TABLE [gn].[GNSampleGNCloudFile]
ADD CONSTRAINT [FK_GNSampleGNCloudFile_GNSample]
    FOREIGN KEY ([GNSampleGNCloudFile_GNCloudFile_Id])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CloudFiles_Id] in table 'GNSampleGNCloudFile'
ALTER TABLE [gn].[GNSampleGNCloudFile]
ADD CONSTRAINT [FK_GNSampleGNCloudFile_GNCloudFile]
    FOREIGN KEY ([CloudFiles_Id])
    REFERENCES [gn].[GNCloudFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleGNCloudFile_GNCloudFile'
CREATE INDEX [IX_FK_GNSampleGNCloudFile_GNCloudFile]
ON [gn].[GNSampleGNCloudFile]
    ([CloudFiles_Id]);
GO

-- Creating foreign key on [GNProjectId] in table 'GNAnalysisRequests'
ALTER TABLE [gn].[GNAnalysisRequests]
ADD CONSTRAINT [FK_GNProjectGNAnalysisRequest]
    FOREIGN KEY ([GNProjectId])
    REFERENCES [gn].[GNProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNProjectGNAnalysisRequest'
CREATE INDEX [IX_FK_GNProjectGNAnalysisRequest]
ON [gn].[GNAnalysisRequests]
    ([GNProjectId]);
GO

-- Creating foreign key on [AnalysisType_Id] in table 'GNAnalysisRequests'
ALTER TABLE [gn].[GNAnalysisRequests]
ADD CONSTRAINT [FK_GNAnalysisRequestGNAnalysisType]
    FOREIGN KEY ([AnalysisType_Id])
    REFERENCES [gn].[GNAnalysisTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNAnalysisType'
CREATE INDEX [IX_FK_GNAnalysisRequestGNAnalysisType]
ON [gn].[GNAnalysisRequests]
    ([AnalysisType_Id]);
GO

-- Creating foreign key on [GNAnalysisResultGNCloudFile_GNCloudFile_Id] in table 'GNAnalysisResultGNCloudFile'
ALTER TABLE [gn].[GNAnalysisResultGNCloudFile]
ADD CONSTRAINT [FK_GNAnalysisResultGNCloudFile_GNAnalysisResult]
    FOREIGN KEY ([GNAnalysisResultGNCloudFile_GNCloudFile_Id])
    REFERENCES [gn].[GNAnalysisResults]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ResultFiles_Id] in table 'GNAnalysisResultGNCloudFile'
ALTER TABLE [gn].[GNAnalysisResultGNCloudFile]
ADD CONSTRAINT [FK_GNAnalysisResultGNCloudFile_GNCloudFile]
    FOREIGN KEY ([ResultFiles_Id])
    REFERENCES [gn].[GNCloudFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisResultGNCloudFile_GNCloudFile'
CREATE INDEX [IX_FK_GNAnalysisResultGNCloudFile_GNCloudFile]
ON [gn].[GNAnalysisResultGNCloudFile]
    ([ResultFiles_Id]);
GO

-- Creating foreign key on [AccountOwner_Id] in table 'GNAccounts'
ALTER TABLE [gn].[GNAccounts]
ADD CONSTRAINT [FK_GNContactGNAccount2]
    FOREIGN KEY ([AccountOwner_Id])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNAccount2'
CREATE INDEX [IX_FK_GNContactGNAccount2]
ON [gn].[GNAccounts]
    ([AccountOwner_Id]);
GO

-- Creating foreign key on [GNAccountId] in table 'GNPaymentMethods'
ALTER TABLE [gn].[GNPaymentMethods]
ADD CONSTRAINT [FK_GNAccountGNPaymentMethod]
    FOREIGN KEY ([GNAccountId])
    REFERENCES [gn].[GNAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAccountGNPaymentMethod'
CREATE INDEX [IX_FK_GNAccountGNPaymentMethod]
ON [gn].[GNPaymentMethods]
    ([GNAccountId]);
GO

-- Creating foreign key on [GNAccountId] in table 'GNInvoices'
ALTER TABLE [gn].[GNInvoices]
ADD CONSTRAINT [FK_GNAccountGNInvoice]
    FOREIGN KEY ([GNAccountId])
    REFERENCES [gn].[GNAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAccountGNInvoice'
CREATE INDEX [IX_FK_GNAccountGNInvoice]
ON [gn].[GNInvoices]
    ([GNAccountId]);
GO

-- Creating foreign key on [GNInvoiceDetailId] in table 'GNTransactions'
ALTER TABLE [gn].[GNTransactions]
ADD CONSTRAINT [FK_GNInvoiceDetailGNTransaction]
    FOREIGN KEY ([GNInvoiceDetailId])
    REFERENCES [gn].[GNInvoiceDetails]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNInvoiceDetailGNTransaction'
CREATE INDEX [IX_FK_GNInvoiceDetailGNTransaction]
ON [gn].[GNTransactions]
    ([GNInvoiceDetailId]);
GO

-- Creating foreign key on [GNTransactionTypeId] in table 'GNTransactions'
ALTER TABLE [gn].[GNTransactions]
ADD CONSTRAINT [FK_GNTransactionTypeGNTransaction]
    FOREIGN KEY ([GNTransactionTypeId])
    REFERENCES [gn].[GNTransactionTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTransactionTypeGNTransaction'
CREATE INDEX [IX_FK_GNTransactionTypeGNTransaction]
ON [gn].[GNTransactions]
    ([GNTransactionTypeId]);
GO

-- Creating foreign key on [GNInvoiceId] in table 'GNInvoiceDetails'
ALTER TABLE [gn].[GNInvoiceDetails]
ADD CONSTRAINT [FK_GNInvoiceGNInvoiceDetail]
    FOREIGN KEY ([GNInvoiceId])
    REFERENCES [gn].[GNInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNInvoiceGNInvoiceDetail'
CREATE INDEX [IX_FK_GNInvoiceGNInvoiceDetail]
ON [gn].[GNInvoiceDetails]
    ([GNInvoiceId]);
GO

-- Creating foreign key on [GNOrganizationId] in table 'GNContacts'
ALTER TABLE [gn].[GNContacts]
ADD CONSTRAINT [FK_GNContactGNOrganization]
    FOREIGN KEY ([GNOrganizationId])
    REFERENCES [gn].[GNOrganizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNOrganization'
CREATE INDEX [IX_FK_GNContactGNOrganization]
ON [gn].[GNContacts]
    ([GNOrganizationId]);
GO

-- Creating foreign key on [GNContactId] in table 'GNTeams'
ALTER TABLE [gn].[GNTeams]
ADD CONSTRAINT [FK_GNContactGNTeam]
    FOREIGN KEY ([GNContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNTeam'
CREATE INDEX [IX_FK_GNContactGNTeam]
ON [gn].[GNTeams]
    ([GNContactId]);
GO

-- Creating foreign key on [GNContactId] in table 'GNOrganizations'
ALTER TABLE [gn].[GNOrganizations]
ADD CONSTRAINT [FK_GNContactGNOrganization1]
    FOREIGN KEY ([GNContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNOrganization1'
CREATE INDEX [IX_FK_GNContactGNOrganization1]
ON [gn].[GNOrganizations]
    ([GNContactId]);
GO

-- Creating foreign key on [Organization_Id] in table 'GNAccounts'
ALTER TABLE [gn].[GNAccounts]
ADD CONSTRAINT [FK_GNOrganizationGNAccount]
    FOREIGN KEY ([Organization_Id])
    REFERENCES [gn].[GNOrganizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNOrganizationGNAccount'
CREATE INDEX [IX_FK_GNOrganizationGNAccount]
ON [gn].[GNAccounts]
    ([Organization_Id]);
GO

-- Creating foreign key on [GNOrganizationId] in table 'GNSamples'
ALTER TABLE [gn].[GNSamples]
ADD CONSTRAINT [FK_GNOrganizationGNSample]
    FOREIGN KEY ([GNOrganizationId])
    REFERENCES [gn].[GNOrganizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNOrganizationGNSample'
CREATE INDEX [IX_FK_GNOrganizationGNSample]
ON [gn].[GNSamples]
    ([GNOrganizationId]);
GO

-- Creating foreign key on [AWSConfigId] in table 'AWSResources'
ALTER TABLE [gn].[AWSResources]
ADD CONSTRAINT [FK_AWSConfigAWSResource]
    FOREIGN KEY ([AWSConfigId])
    REFERENCES [gn].[AWSConfigs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AWSConfigAWSResource'
CREATE INDEX [IX_FK_AWSConfigAWSResource]
ON [gn].[AWSResources]
    ([AWSConfigId]);
GO

-- Creating foreign key on [AWSResourceTypeId] in table 'AWSResources'
ALTER TABLE [gn].[AWSResources]
ADD CONSTRAINT [FK_AWSResourceTypeAWSResource]
    FOREIGN KEY ([AWSResourceTypeId])
    REFERENCES [gn].[AWSResourceTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AWSResourceTypeAWSResource'
CREATE INDEX [IX_FK_AWSResourceTypeAWSResource]
ON [gn].[AWSResources]
    ([AWSResourceTypeId]);
GO

-- Creating foreign key on [GNSampleTypeId] in table 'GNSamples'
ALTER TABLE [gn].[GNSamples]
ADD CONSTRAINT [FK_GNSampleTypeGNSample]
    FOREIGN KEY ([GNSampleTypeId])
    REFERENCES [gn].[GNSampleTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleTypeGNSample'
CREATE INDEX [IX_FK_GNSampleTypeGNSample]
ON [gn].[GNSamples]
    ([GNSampleTypeId]);
GO

-- Creating foreign key on [AWSRegionSystemName] in table 'AWSConfigs'
ALTER TABLE [gn].[AWSConfigs]
ADD CONSTRAINT [FK_AWSRegionAWSConfig]
    FOREIGN KEY ([AWSRegionSystemName])
    REFERENCES [gn].[AWSRegions]
        ([AWSRegionSystemName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AWSRegionAWSConfig'
CREATE INDEX [IX_FK_AWSRegionAWSConfig]
ON [gn].[AWSConfigs]
    ([AWSRegionSystemName]);
GO

-- Creating foreign key on [AWSRegionSystemName] in table 'GNCloudFiles'
ALTER TABLE [gn].[GNCloudFiles]
ADD CONSTRAINT [FK_AWSRegionGNCloudFile]
    FOREIGN KEY ([AWSRegionSystemName])
    REFERENCES [gn].[AWSRegions]
        ([AWSRegionSystemName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AWSRegionGNCloudFile'
CREATE INDEX [IX_FK_AWSRegionGNCloudFile]
ON [gn].[GNCloudFiles]
    ([AWSRegionSystemName]);
GO

-- Creating foreign key on [AWSRegionSystemName] in table 'GNAnalysisRequests'
ALTER TABLE [gn].[GNAnalysisRequests]
ADD CONSTRAINT [FK_AWSRegionGNAnalysisRequest]
    FOREIGN KEY ([AWSRegionSystemName])
    REFERENCES [gn].[AWSRegions]
        ([AWSRegionSystemName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AWSRegionGNAnalysisRequest'
CREATE INDEX [IX_FK_AWSRegionGNAnalysisRequest]
ON [gn].[GNAnalysisRequests]
    ([AWSRegionSystemName]);
GO

-- Creating foreign key on [AWSRegionSystemName] in table 'GNAnalysisResults'
ALTER TABLE [gn].[GNAnalysisResults]
ADD CONSTRAINT [FK_AWSRegionGNAnalysisResult]
    FOREIGN KEY ([AWSRegionSystemName])
    REFERENCES [gn].[AWSRegions]
        ([AWSRegionSystemName])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AWSRegionGNAnalysisResult'
CREATE INDEX [IX_FK_AWSRegionGNAnalysisResult]
ON [gn].[GNAnalysisResults]
    ([AWSRegionSystemName]);
GO

-- Creating foreign key on [GNAnalysisResultId] in table 'GNAnalysisStatus'
ALTER TABLE [gn].[GNAnalysisStatus]
ADD CONSTRAINT [FK_GNAnalysisResultGNAnalysisStatus]
    FOREIGN KEY ([GNAnalysisResultId])
    REFERENCES [gn].[GNAnalysisResults]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisResultGNAnalysisStatus'
CREATE INDEX [IX_FK_GNAnalysisResultGNAnalysisStatus]
ON [gn].[GNAnalysisStatus]
    ([GNAnalysisResultId]);
GO

-- Creating foreign key on [AWSConfigId] in table 'GNOrganizations'
ALTER TABLE [gn].[GNOrganizations]
ADD CONSTRAINT [FK_AWSConfigGNOrganization]
    FOREIGN KEY ([AWSConfigId])
    REFERENCES [gn].[AWSConfigs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AWSConfigGNOrganization'
CREATE INDEX [IX_FK_AWSConfigGNOrganization]
ON [gn].[GNOrganizations]
    ([AWSConfigId]);
GO

-- Creating foreign key on [GNAnalysisRequestId] in table 'GNAnalysisStatus'
ALTER TABLE [gn].[GNAnalysisStatus]
ADD CONSTRAINT [FK_GNAnalysisRequestGNAnalysisStatus]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNAnalysisStatus'
CREATE INDEX [IX_FK_GNAnalysisRequestGNAnalysisStatus]
ON [gn].[GNAnalysisStatus]
    ([GNAnalysisRequestId]);
GO

-- Creating foreign key on [AnalysisRequest_Id] in table 'GNAnalysisResults'
ALTER TABLE [gn].[GNAnalysisResults]
ADD CONSTRAINT [FK_GNAnalysisRequestGNAnalysisResult]
    FOREIGN KEY ([AnalysisRequest_Id])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNAnalysisResult'
CREATE INDEX [IX_FK_GNAnalysisRequestGNAnalysisResult]
ON [gn].[GNAnalysisResults]
    ([AnalysisRequest_Id]);
GO

-- Creating foreign key on [GNContactId] in table 'GNContactRoles'
ALTER TABLE [gn].[GNContactRoles]
ADD CONSTRAINT [FK_GNContactGNContactRole]
    FOREIGN KEY ([GNContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GNEntityTypeId] in table 'GNEntityTags'
ALTER TABLE [gn].[GNEntityTags]
ADD CONSTRAINT [FK_GNEntityTypeGNEntityTag]
    FOREIGN KEY ([GNEntityTypeId])
    REFERENCES [gn].[GNEntityTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNEntityTypeGNEntityTag'
CREATE INDEX [IX_FK_GNEntityTypeGNEntityTag]
ON [gn].[GNEntityTags]
    ([GNEntityTypeId]);
GO

-- Creating foreign key on [GNTagId] in table 'GNEntityTags'
ALTER TABLE [gn].[GNEntityTags]
ADD CONSTRAINT [FK_GNTagGNEntityTag]
    FOREIGN KEY ([GNTagId])
    REFERENCES [gn].[GNTags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTagGNEntityTag'
CREATE INDEX [IX_FK_GNTagGNEntityTag]
ON [gn].[GNEntityTags]
    ([GNTagId]);
GO

-- Creating foreign key on [GNEntityTypeId] in table 'GNEntityAudits'
ALTER TABLE [gn].[GNEntityAudits]
ADD CONSTRAINT [FK_GNEntityTypeGNEntityAudit]
    FOREIGN KEY ([GNEntityTypeId])
    REFERENCES [gn].[GNEntityTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNEntityTypeGNEntityAudit'
CREATE INDEX [IX_FK_GNEntityTypeGNEntityAudit]
ON [gn].[GNEntityAudits]
    ([GNEntityTypeId]);
GO

-- Creating foreign key on [GNEntityAuditTypeId] in table 'GNEntityAudits'
ALTER TABLE [gn].[GNEntityAudits]
ADD CONSTRAINT [FK_GNEntityAuditTypeGNEntityAudit]
    FOREIGN KEY ([GNEntityAuditTypeId])
    REFERENCES [gn].[GNEntityAuditTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNEntityAuditTypeGNEntityAudit'
CREATE INDEX [IX_FK_GNEntityAuditTypeGNEntityAudit]
ON [gn].[GNEntityAudits]
    ([GNEntityAuditTypeId]);
GO

-- Creating foreign key on [GNSampleRelationshipTypeId] in table 'GNSampleRelationships'
ALTER TABLE [gn].[GNSampleRelationships]
ADD CONSTRAINT [FK_GNSampleRelationshipTypeGNSampleRelationship]
    FOREIGN KEY ([GNSampleRelationshipTypeId])
    REFERENCES [gn].[GNSampleRelationshipTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleRelationshipTypeGNSampleRelationship'
CREATE INDEX [IX_FK_GNSampleRelationshipTypeGNSampleRelationship]
ON [gn].[GNSampleRelationships]
    ([GNSampleRelationshipTypeId]);
GO

-- Creating foreign key on [GNLeftSampleId] in table 'GNSampleRelationships'
ALTER TABLE [gn].[GNSampleRelationships]
ADD CONSTRAINT [FK_GNSampleGNSampleLeftRelationship]
    FOREIGN KEY ([GNLeftSampleId])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleGNSampleLeftRelationship'
CREATE INDEX [IX_FK_GNSampleGNSampleLeftRelationship]
ON [gn].[GNSampleRelationships]
    ([GNLeftSampleId]);
GO

-- Creating foreign key on [GNRightSampleId] in table 'GNSampleRelationships'
ALTER TABLE [gn].[GNSampleRelationships]
ADD CONSTRAINT [FK_GNSampleGNSampleRightRelationship]
    FOREIGN KEY ([GNRightSampleId])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleGNSampleRightRelationship'
CREATE INDEX [IX_FK_GNSampleGNSampleRightRelationship]
ON [gn].[GNSampleRelationships]
    ([GNRightSampleId]);
GO

-- Creating foreign key on [GNContactId] in table 'GNEntityAudits'
ALTER TABLE [gn].[GNEntityAudits]
ADD CONSTRAINT [FK_GNContactGNEntityAudit]
    FOREIGN KEY ([GNContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNEntityAudit'
CREATE INDEX [IX_FK_GNContactGNEntityAudit]
ON [gn].[GNEntityAudits]
    ([GNContactId]);
GO

-- Creating foreign key on [GNPaymentMethodTypeId] in table 'GNPaymentMethods'
ALTER TABLE [gn].[GNPaymentMethods]
ADD CONSTRAINT [FK_GNPaymentMethodTypeGNPaymentMethod]
    FOREIGN KEY ([GNPaymentMethodTypeId])
    REFERENCES [gn].[GNPaymentMethodTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNPaymentMethodTypeGNPaymentMethod'
CREATE INDEX [IX_FK_GNPaymentMethodTypeGNPaymentMethod]
ON [gn].[GNPaymentMethods]
    ([GNPaymentMethodTypeId]);
GO

-- Creating foreign key on [GNProductTypeId] in table 'GNProducts'
ALTER TABLE [gn].[GNProducts]
ADD CONSTRAINT [FK_GNProductTypeGNProduct]
    FOREIGN KEY ([GNProductTypeId])
    REFERENCES [gn].[GNProductTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNProductTypeGNProduct'
CREATE INDEX [IX_FK_GNProductTypeGNProduct]
ON [gn].[GNProducts]
    ([GNProductTypeId]);
GO

-- Creating foreign key on [GNProductId] in table 'GNAccountProductSubscriptions'
ALTER TABLE [gn].[GNAccountProductSubscriptions]
ADD CONSTRAINT [FK_GNProductGNAccountProductSubscription]
    FOREIGN KEY ([GNProductId])
    REFERENCES [gn].[GNProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNProductGNAccountProductSubscription'
CREATE INDEX [IX_FK_GNProductGNAccountProductSubscription]
ON [gn].[GNAccountProductSubscriptions]
    ([GNProductId]);
GO

-- Creating foreign key on [GNAccountId] in table 'GNAccountProductSubscriptions'
ALTER TABLE [gn].[GNAccountProductSubscriptions]
ADD CONSTRAINT [FK_GNAccountGNAccountProductSubscription]
    FOREIGN KEY ([GNAccountId])
    REFERENCES [gn].[GNAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAccountGNAccountProductSubscription'
CREATE INDEX [IX_FK_GNAccountGNAccountProductSubscription]
ON [gn].[GNAccountProductSubscriptions]
    ([GNAccountId]);
GO

-- Creating foreign key on [GNProductId] in table 'GNTransactions'
ALTER TABLE [gn].[GNTransactions]
ADD CONSTRAINT [FK_GNProductGNTransaction]
    FOREIGN KEY ([GNProductId])
    REFERENCES [gn].[GNProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNProductGNTransaction'
CREATE INDEX [IX_FK_GNProductGNTransaction]
ON [gn].[GNTransactions]
    ([GNProductId]);
GO

-- Creating foreign key on [GNPaymentMethodId] in table 'GNPayments'
ALTER TABLE [gn].[GNPayments]
ADD CONSTRAINT [FK_GNPaymentGNPaymentMethod]
    FOREIGN KEY ([GNPaymentMethodId])
    REFERENCES [gn].[GNPaymentMethods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNPaymentGNPaymentMethod'
CREATE INDEX [IX_FK_GNPaymentGNPaymentMethod]
ON [gn].[GNPayments]
    ([GNPaymentMethodId]);
GO

-- Creating foreign key on [Payments_Id] in table 'GNPaymentGNInvoice'
ALTER TABLE [gn].[GNPaymentGNInvoice]
ADD CONSTRAINT [FK_GNPaymentGNInvoice_GNPayment]
    FOREIGN KEY ([Payments_Id])
    REFERENCES [gn].[GNPayments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Invoices_Id] in table 'GNPaymentGNInvoice'
ALTER TABLE [gn].[GNPaymentGNInvoice]
ADD CONSTRAINT [FK_GNPaymentGNInvoice_GNInvoice]
    FOREIGN KEY ([Invoices_Id])
    REFERENCES [gn].[GNInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNPaymentGNInvoice_GNInvoice'
CREATE INDEX [IX_FK_GNPaymentGNInvoice_GNInvoice]
ON [gn].[GNPaymentGNInvoice]
    ([Invoices_Id]);
GO

-- Creating foreign key on [GNAccountTypeId] in table 'GNAccounts'
ALTER TABLE [gn].[GNAccounts]
ADD CONSTRAINT [FK_GNAccountTypeGNAccount]
    FOREIGN KEY ([GNAccountTypeId])
    REFERENCES [gn].[GNAccountTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAccountTypeGNAccount'
CREATE INDEX [IX_FK_GNAccountTypeGNAccount]
ON [gn].[GNAccounts]
    ([GNAccountTypeId]);
GO

-- Creating foreign key on [GNAccountId] in table 'GNPurchaseOrders'
ALTER TABLE [gn].[GNPurchaseOrders]
ADD CONSTRAINT [FK_GNAccountGNPurchaseOrder]
    FOREIGN KEY ([GNAccountId])
    REFERENCES [gn].[GNAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAccountGNPurchaseOrder'
CREATE INDEX [IX_FK_GNAccountGNPurchaseOrder]
ON [gn].[GNPurchaseOrders]
    ([GNAccountId]);
GO

-- Creating foreign key on [GNAccountId] in table 'GNAccountThresholds'
ALTER TABLE [gn].[GNAccountThresholds]
ADD CONSTRAINT [FK_GNAccountGNAccountThreshold]
    FOREIGN KEY ([GNAccountId])
    REFERENCES [gn].[GNAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAccountGNAccountThreshold'
CREATE INDEX [IX_FK_GNAccountGNAccountThreshold]
ON [gn].[GNAccountThresholds]
    ([GNAccountId]);
GO

-- Creating foreign key on [GNAccountTypeId] in table 'GNProducts'
ALTER TABLE [gn].[GNProducts]
ADD CONSTRAINT [FK_GNAccountTypeGNProduct]
    FOREIGN KEY ([GNAccountTypeId])
    REFERENCES [gn].[GNAccountTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAccountTypeGNProduct'
CREATE INDEX [IX_FK_GNAccountTypeGNProduct]
ON [gn].[GNProducts]
    ([GNAccountTypeId]);
GO

-- Creating foreign key on [GNSampleId] in table 'GNAnalysisRequestGNSamples'
ALTER TABLE [gn].[GNAnalysisRequestGNSamples]
ADD CONSTRAINT [FK_GNAnalysisRequestGNSampleGNSample]
    FOREIGN KEY ([GNSampleId])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GNAnalysisRequestId] in table 'GNAnalysisRequestGNSamples'
ALTER TABLE [gn].[GNAnalysisRequestGNSamples]
ADD CONSTRAINT [FK_GNAnalysisRequestGNSampleGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNSampleGNAnalysisRequest'
CREATE INDEX [IX_FK_GNAnalysisRequestGNSampleGNAnalysisRequest]
ON [gn].[GNAnalysisRequestGNSamples]
    ([GNAnalysisRequestId]);
GO

-- Creating foreign key on [PurchaseOrders_Id] in table 'GNPurchaseOrderGNInvoice'
ALTER TABLE [gn].[GNPurchaseOrderGNInvoice]
ADD CONSTRAINT [FK_GNPurchaseOrderGNInvoice_GNPurchaseOrder]
    FOREIGN KEY ([PurchaseOrders_Id])
    REFERENCES [gn].[GNPurchaseOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Invoices_Id] in table 'GNPurchaseOrderGNInvoice'
ALTER TABLE [gn].[GNPurchaseOrderGNInvoice]
ADD CONSTRAINT [FK_GNPurchaseOrderGNInvoice_GNInvoice]
    FOREIGN KEY ([Invoices_Id])
    REFERENCES [gn].[GNInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNPurchaseOrderGNInvoice_GNInvoice'
CREATE INDEX [IX_FK_GNPurchaseOrderGNInvoice_GNInvoice]
ON [gn].[GNPurchaseOrderGNInvoice]
    ([Invoices_Id]);
GO

-- Creating foreign key on [GNNotificationTopicId] in table 'GNNotificationTopicSubscribers'
ALTER TABLE [gn].[GNNotificationTopicSubscribers]
ADD CONSTRAINT [FK_GNNotificationTopicSubscriberGNNotificationTopic]
    FOREIGN KEY ([GNNotificationTopicId])
    REFERENCES [gn].[GNNotificationTopics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GNContactId] in table 'GNNotificationTopicSubscribers'
ALTER TABLE [gn].[GNNotificationTopicSubscribers]
ADD CONSTRAINT [FK_GNNotificationTopicSubscriberGNContact]
    FOREIGN KEY ([GNContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNotificationTopicSubscriberGNContact'
CREATE INDEX [IX_FK_GNNotificationTopicSubscriberGNContact]
ON [gn].[GNNotificationTopicSubscribers]
    ([GNContactId]);
GO

-- Creating foreign key on [GNNotificationTopicId] in table 'GNNotificationLogs'
ALTER TABLE [gn].[GNNotificationLogs]
ADD CONSTRAINT [FK_GNNotificationLogGNNotificationTopic]
    FOREIGN KEY ([GNNotificationTopicId])
    REFERENCES [gn].[GNNotificationTopics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNotificationLogGNNotificationTopic'
CREATE INDEX [IX_FK_GNNotificationLogGNNotificationTopic]
ON [gn].[GNNotificationLogs]
    ([GNNotificationTopicId]);
GO

-- Creating foreign key on [GNNotificationTopicId] in table 'GNNotificationTopicAddressees'
ALTER TABLE [gn].[GNNotificationTopicAddressees]
ADD CONSTRAINT [FK_GNNotificationTopicAddresseeGNNotificationTopic]
    FOREIGN KEY ([GNNotificationTopicId])
    REFERENCES [gn].[GNNotificationTopics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------