
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/30/2017 14:07:01
-- Generated from EDMX file: F:\GitHub\New_SRC\New_Webapp\webapp-development\GNData\EntityModel\GNEntityModel.edmx
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
IF OBJECT_ID(N'[gn].[FK_GNBulkImportStatusGNBulkImportLog]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBulkImportLogs] DROP CONSTRAINT [FK_GNBulkImportStatusGNBulkImportLog];
GO
IF OBJECT_ID(N'[gn].[FK_GNBulkImportStatusGNBulkImportStaging]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBulkImportStaging] DROP CONSTRAINT [FK_GNBulkImportStatusGNBulkImportStaging];
GO
IF OBJECT_ID(N'[gn].[FK_GNSettingsTemplateGNSettingsTemplateConfig]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSettingsTemplateConfigs] DROP CONSTRAINT [FK_GNSettingsTemplateGNSettingsTemplateConfig];
GO
IF OBJECT_ID(N'[gn].[FK_GNSettingsTemplateConfigGNSettingsTemplateField]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSettingsTemplateConfigs] DROP CONSTRAINT [FK_GNSettingsTemplateConfigGNSettingsTemplateField];
GO
IF OBJECT_ID(N'[gn].[FK_GNOrganizationGNSettingsTemplate]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNOrganizations] DROP CONSTRAINT [FK_GNOrganizationGNSettingsTemplate];
GO
IF OBJECT_ID(N'[gn].[FK_AWSConfigAWSComputeEnvironment]', 'F') IS NOT NULL
    ALTER TABLE [gn].[AWSComputeEnvironments] DROP CONSTRAINT [FK_AWSConfigAWSComputeEnvironment];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGNTemplateGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGNTemplates] DROP CONSTRAINT [FK_GNAnalysisRequestGNTemplateGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGNTemplateGNTemplate]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGNTemplates] DROP CONSTRAINT [FK_GNAnalysisRequestGNTemplateGNTemplate];
GO
IF OBJECT_ID(N'[gn].[FK_GNTemplateGNTemplateGene]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTemplateGenes] DROP CONSTRAINT [FK_GNTemplateGNTemplateGene];
GO
IF OBJECT_ID(N'[gn].[FK_GNTemplateGNOrganization]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTemplates] DROP CONSTRAINT [FK_GNTemplateGNOrganization];
GO
IF OBJECT_ID(N'[gn].[FK_GNTemplateGeneGNGene]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTemplateGenes] DROP CONSTRAINT [FK_GNTemplateGeneGNGene];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGNAnalysisRequestType]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequests] DROP CONSTRAINT [FK_GNAnalysisRequestGNAnalysisRequestType];
GO
IF OBJECT_ID(N'[gn].[FK_GNTransactionGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTransactions] DROP CONSTRAINT [FK_GNTransactionGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_GNTransactionGNProject]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTransactions] DROP CONSTRAINT [FK_GNTransactionGNProject];
GO
IF OBJECT_ID(N'[gn].[FK_GNTransactionGNTeam]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNTransactions] DROP CONSTRAINT [FK_GNTransactionGNTeam];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleQualifierGNSampleQualifierGroup]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSampleQualifiers] DROP CONSTRAINT [FK_GNSampleQualifierGNSampleQualifierGroup];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleQualifierGNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSamples] DROP CONSTRAINT [FK_GNSampleQualifierGNSample];
GO
IF OBJECT_ID(N'[gn].[FK_GNSharedPurchaseOrderOrganizationGNInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSharedPurchaseOrderOrganizations] DROP CONSTRAINT [FK_GNSharedPurchaseOrderOrganizationGNInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNSharedPurchaseOrderOrganizationGNPurchaseOrder]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSharedPurchaseOrderOrganizations] DROP CONSTRAINT [FK_GNSharedPurchaseOrderOrganizationGNPurchaseOrder];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGroupGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGroups] DROP CONSTRAINT [FK_GNAnalysisRequestGroupGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGroupGNSample_GNAnalysisRequestGroup]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGroupGNSample] DROP CONSTRAINT [FK_GNAnalysisRequestGroupGNSample_GNAnalysisRequestGroup];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGroupGNSample_GNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGroupGNSample] DROP CONSTRAINT [FK_GNAnalysisRequestGroupGNSample_GNSample];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGroupComparisonGNAnalysisRequestGroup]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons] DROP CONSTRAINT [FK_GNAnalysisRequestGroupComparisonGNAnalysisRequestGroup];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGroupGNAnalysisRequestGroupComparison]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons] DROP CONSTRAINT [FK_GNAnalysisRequestGroupGNAnalysisRequestGroupComparison];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisRequestGroupComparisonGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons] DROP CONSTRAINT [FK_GNAnalysisRequestGroupComparisonGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_GNAnalysisAdaptorGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNAnalysisRequests] DROP CONSTRAINT [FK_GNAnalysisAdaptorGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingPurchaseOrderGNBillingAccount_GNBillingPurchaseOrder]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount] DROP CONSTRAINT [FK_GNBillingPurchaseOrderGNBillingAccount_GNBillingPurchaseOrder];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingPurchaseOrderGNBillingAccount_GNBillingAccount]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount] DROP CONSTRAINT [FK_GNBillingPurchaseOrderGNBillingAccount_GNBillingAccount];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingAccountGNBillingPaymentMethod]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPaymentMethods] DROP CONSTRAINT [FK_GNBillingAccountGNBillingPaymentMethod];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingInvoiceGNBillingPurchaseOrderInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPurchaseOrderInvoices] DROP CONSTRAINT [FK_GNBillingInvoiceGNBillingPurchaseOrderInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingPurchaseOrderGNBillingPurchaseOrderInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPurchaseOrderInvoices] DROP CONSTRAINT [FK_GNBillingPurchaseOrderGNBillingPurchaseOrderInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingAccountGNOrganization]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingAccounts] DROP CONSTRAINT [FK_GNBillingAccountGNOrganization];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingAccountGNAccountType]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingAccounts] DROP CONSTRAINT [FK_GNBillingAccountGNAccountType];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingPaymentMethodGNPaymentMethodType]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPaymentMethods] DROP CONSTRAINT [FK_GNBillingPaymentMethodGNPaymentMethodType];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingTransactionGNTransactionType]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingTransactions] DROP CONSTRAINT [FK_GNBillingTransactionGNTransactionType];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingTransactionGNTeam]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingTransactions] DROP CONSTRAINT [FK_GNBillingTransactionGNTeam];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingTransactionGNAnalysisRequest]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingTransactions] DROP CONSTRAINT [FK_GNBillingTransactionGNAnalysisRequest];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingTransactionGNProject]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingTransactions] DROP CONSTRAINT [FK_GNBillingTransactionGNProject];
GO
IF OBJECT_ID(N'[gn].[FK_GNContactGNBillingAccount]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingAccounts] DROP CONSTRAINT [FK_GNContactGNBillingAccount];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingAccountGNContact]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingAccounts] DROP CONSTRAINT [FK_GNBillingAccountGNContact];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingInvoiceDetailGNBillingInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingInvoiceDetails] DROP CONSTRAINT [FK_GNBillingInvoiceDetailGNBillingInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingInvoiceDetailGNBillingTransaction]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingTransactions] DROP CONSTRAINT [FK_GNBillingInvoiceDetailGNBillingTransaction];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingTransactionGNProduct]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingTransactions] DROP CONSTRAINT [FK_GNBillingTransactionGNProduct];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingInvoiceGNBillingAccount]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingInvoices] DROP CONSTRAINT [FK_GNBillingInvoiceGNBillingAccount];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingPaymentGNPaymentMethod]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPayments] DROP CONSTRAINT [FK_GNBillingPaymentGNPaymentMethod];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingPaymentInvoiceGNBillingInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPaymentInvoices] DROP CONSTRAINT [FK_GNBillingPaymentInvoiceGNBillingInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNBillingPaymentInvoiceGNBillingPayment]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBillingPaymentInvoices] DROP CONSTRAINT [FK_GNBillingPaymentInvoiceGNBillingPayment];
GO
IF OBJECT_ID(N'[gn].[FK_GNBulkImportGroupStagingGNBulkImportStatus]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNBulkImportGroupStagings] DROP CONSTRAINT [FK_GNBulkImportGroupStagingGNBulkImportStatus];
GO
IF OBJECT_ID(N'[gn].[FK_GNSampleStatusGNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSampleStatus] DROP CONSTRAINT [FK_GNSampleStatusGNSample];
GO
IF OBJECT_ID(N'[gn].[FK_GNNewSampleBatchStatusGNNewSampleBatch]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNNewSampleBatchStatus] DROP CONSTRAINT [FK_GNNewSampleBatchStatusGNNewSampleBatch];
GO
IF OBJECT_ID(N'[gn].[FK_GNNewSampleBatchSamplesGNNewSampleBatch]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNNewSampleBatchSamples] DROP CONSTRAINT [FK_GNNewSampleBatchSamplesGNNewSampleBatch];
GO
IF OBJECT_ID(N'[gn].[FK_GNNewSampleBatchSamplesGNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNNewSampleBatchSamples] DROP CONSTRAINT [FK_GNNewSampleBatchSamplesGNSample];
GO
IF OBJECT_ID(N'[gn].[FK_GNSequencerJobGNOrganization]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSequencerJobs] DROP CONSTRAINT [FK_GNSequencerJobGNOrganization];
GO
IF OBJECT_ID(N'[gn].[FK_GNNewSampleBatchGNSequencerJob]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNNewSampleBatches] DROP CONSTRAINT [FK_GNNewSampleBatchGNSequencerJob];
GO
IF OBJECT_ID(N'[gn].[FK_GNSequencerJobGNProject]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSequencerJobs] DROP CONSTRAINT [FK_GNSequencerJobGNProject];
GO
IF OBJECT_ID(N'[gn].[FK_GNReplicateGNSample]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNSamples] DROP CONSTRAINT [FK_GNReplicateGNSample];
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
IF OBJECT_ID(N'[gn].[GNSampleRelationshipTypeMappings]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSampleRelationshipTypeMappings];
GO
IF OBJECT_ID(N'[gn].[GNBulkImportStatus]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBulkImportStatus];
GO
IF OBJECT_ID(N'[gn].[GNBulkImportLogs]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBulkImportLogs];
GO
IF OBJECT_ID(N'[gn].[GNBulkImportStaging]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBulkImportStaging];
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
IF OBJECT_ID(N'[gn].[AWSComputeEnvironments]', 'U') IS NOT NULL
    DROP TABLE [gn].[AWSComputeEnvironments];
GO
IF OBJECT_ID(N'[gn].[GNNotificationSuppressionLists]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNotificationSuppressionLists];
GO
IF OBJECT_ID(N'[gn].[GNTemplates]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNTemplates];
GO
IF OBJECT_ID(N'[gn].[GNTemplateGenes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNTemplateGenes];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisRequestGNTemplates]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisRequestGNTemplates];
GO
IF OBJECT_ID(N'[gn].[GNGenes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNGenes];
GO
IF OBJECT_ID(N'[gn].[GNSampleQualifiers]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSampleQualifiers];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisRequestTypes]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisRequestTypes];
GO
IF OBJECT_ID(N'[gn].[GNSampleQualifierGroups]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSampleQualifierGroups];
GO
IF OBJECT_ID(N'[gn].[GNSharedPurchaseOrderOrganizations]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSharedPurchaseOrderOrganizations];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisRequestGroups]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisRequestGroups];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisRequestGroupComparisons]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisRequestGroupComparisons];
GO
IF OBJECT_ID(N'[gn].[GNAnalysisAdaptors]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisAdaptors];
GO
IF OBJECT_ID(N'[gn].[GNBillingAccounts]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingAccounts];
GO
IF OBJECT_ID(N'[gn].[GNBillingInvoices]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingInvoices];
GO
IF OBJECT_ID(N'[gn].[GNBillingInvoiceDetails]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingInvoiceDetails];
GO
IF OBJECT_ID(N'[gn].[GNBillingPaymentMethods]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingPaymentMethods];
GO
IF OBJECT_ID(N'[gn].[GNBillingPurchaseOrders]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingPurchaseOrders];
GO
IF OBJECT_ID(N'[gn].[GNBillingPurchaseOrderInvoices]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingPurchaseOrderInvoices];
GO
IF OBJECT_ID(N'[gn].[GNBillingPaymentInvoices]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingPaymentInvoices];
GO
IF OBJECT_ID(N'[gn].[GNBillingPayments]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingPayments];
GO
IF OBJECT_ID(N'[gn].[GNBillingTransactions]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingTransactions];
GO
IF OBJECT_ID(N'[gn].[GNBulkImportGroupStagings]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBulkImportGroupStagings];
GO
IF OBJECT_ID(N'[gn].[GNSampleStatus]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSampleStatus];
GO
IF OBJECT_ID(N'[gn].[GNNewSampleBatches]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNewSampleBatches];
GO
IF OBJECT_ID(N'[gn].[GNNewSampleBatchStatus]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNewSampleBatchStatus];
GO
IF OBJECT_ID(N'[gn].[GNNewSampleBatchSamples]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNNewSampleBatchSamples];
GO
IF OBJECT_ID(N'[gn].[GNSequencerJobs]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNSequencerJobs];
GO
IF OBJECT_ID(N'[gn].[GNReplicates]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNReplicates];
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
IF OBJECT_ID(N'[gn].[GNAnalysisRequestGroupGNSample]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNAnalysisRequestGroupGNSample];
GO
IF OBJECT_ID(N'[gn].[GNBillingPurchaseOrderGNBillingAccount]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount];
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
    [AvailableCredits] float  NOT NULL,
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
    [Description] nvarchar(30)  NULL,
    [RequestProgress] float  NULL,
    [LatestRequestPhase] nvarchar(50)  NOT NULL,
    [RequestDateTime] datetime  NULL,
    [GNProjectId] uniqueidentifier  NOT NULL,
    [AWSRegionSystemName] nvarchar(100)  NOT NULL,
    [GNAnalysisRequestTypeCode] nvarchar(30)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [GNAnalysisAdaptorCode] nvarchar(30)  NULL,
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
    [QcStatsAvailable] bit  NULL,
    [QcStatsReportLocation] nchar(1000)  NULL,
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
    [InvoiceCycle] nvarchar(6)  NOT NULL,
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
    [CreateDateTime] datetime  NULL,
    [GNAnalysisRequest_Id] uniqueidentifier  NULL,
    [GNProject_Id] uniqueidentifier  NULL,
    [GNTeam_Id] uniqueidentifier  NULL
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
    [CreateDateTime] datetime  NULL,
    [GNSettingsTemplateId] int  NULL,
    [Repository] nvarchar(1000)  NULL
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
    [IsSubscribedForNewsletters] bit  NULL,
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
    [GNSampleQualifierCode] nvarchar(30)  NOT NULL,
    [Tags] nvarchar(max)  NULL,
    [GNReplicateCode] nchar(2)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [GNReplicate_Id] uniqueidentifier  NOT NULL
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
    [Tag] nvarchar(100)  NOT NULL,
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
    [AnalysisPhase] nvarchar(50)  NOT NULL,
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
    [FreeCreditAllowance] float  NOT NULL,
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
    [SendingCondition] nvarchar(max)  NULL,
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
    [Source] nvarchar(max)  NULL,
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
    [AffectedIndicator] nchar(1)  NOT NULL,
    [TargetIndicator] nchar(1)  NOT NULL
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

-- Creating table 'GNSampleRelationshipTypeMappings'
CREATE TABLE [gn].[GNSampleRelationshipTypeMappings] (
    [NameLeftRelationship] nchar(50)  NOT NULL,
    [NameRightRelationship] nchar(50)  NOT NULL,
    [GenderRightRelationship] nchar(1)  NOT NULL
);
GO

-- Creating table 'GNBulkImportStatus'
CREATE TABLE [gn].[GNBulkImportStatus] (
    [Id] uniqueidentifier  NOT NULL,
    [FileURIType] nvarchar(255)  NOT NULL,
    [FileURI] nvarchar(1024)  NOT NULL,
    [FileExtension] nvarchar(25)  NOT NULL,
    [FileMimeType] nvarchar(25)  NULL,
    [TotalRecordCount] bigint  NOT NULL,
    [ImportedRecordCount] bigint  NOT NULL,
    [FailedRecordCount] bigint  NOT NULL,
    [DuplicateRecordCount] bigint  NOT NULL,
    [TeamsCreatedCount] bigint  NOT NULL,
    [ProjectsCreatedCount] bigint  NOT NULL,
    [AnalysesCreatedCount] bigint  NOT NULL,
    [SamplesCreatedCount] bigint  NOT NULL,
    [FilesCreatedCount] bigint  NOT NULL,
    [ImportStartDateTime] datetime  NULL,
    [ImportEndDateTime] datetime  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNBulkImportLogs'
CREATE TABLE [gn].[GNBulkImportLogs] (
    [Id] uniqueidentifier  NOT NULL,
    [RecordRowId] nvarchar(55)  NOT NULL,
    [IsError] bit  NOT NULL,
    [IsDuplicate] bit  NOT NULL,
    [Message] nvarchar(2048)  NOT NULL,
    [GNBulkImportStatusId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNBulkImportStaging'
CREATE TABLE [gn].[GNBulkImportStaging] (
    [ROW_IDX] bigint IDENTITY(1,1) NOT NULL,
    [TEAM_NAME] nvarchar(255)  NULL,
    [PROJECT_NAME] nvarchar(255)  NULL,
    [ANALYSIS_NAME] nvarchar(255)  NULL,
    [ANALYSIS_TYPE] nvarchar(255)  NOT NULL,
    [ANALYSIS_ADAPTOR] nvarchar(255)  NOT NULL,
    [ANALYSIS_GROUP_NAME] nvarchar(255)  NULL,
    [SAMPLE_NAME] nvarchar(255)  NULL,
    [SAMPLE_TYPE] nvarchar(255)  NULL,
    [SAMPLE_GENDER] nvarchar(255)  NULL,
    [SAMPLE_READ_TYPE] nvarchar(255)  NULL,
    [SAMPLE_QUALIFIER] nvarchar(255)  NOT NULL,
    [FILE_BUCKET] nvarchar(255)  NULL,
    [FILE_KEY] nvarchar(2048)  NULL,
    [FILE_SIZE] nvarchar(100)  NULL,
    [AUTO_START] nvarchar(100)  NULL,
    [GNBulkImportStatusId] uniqueidentifier  NOT NULL
);
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
    [ConfigType] nvarchar(100)  NULL,
    [Description] nvarchar(max)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

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

-- Creating table 'GNNotificationSuppressionLists'
CREATE TABLE [gn].[GNNotificationSuppressionLists] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [CreateDateTime] datetime  NOT NULL,
    [Category] nvarchar(20)  NOT NULL,
    [Type] nvarchar(max)  NULL,
    [Subtype] nvarchar(max)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [NotifiedAdminOn] datetime  NULL,
    [AdminNotes] nvarchar(max)  NULL
);
GO

-- Creating table 'GNTemplates'
CREATE TABLE [gn].[GNTemplates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(100)  NOT NULL,
    [Version] int  NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [GNOrganizationId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNTemplateGenes'
CREATE TABLE [gn].[GNTemplateGenes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GNTemplateId] int  NOT NULL,
    [GeneCode] nvarchar(25)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreatedDateTime] datetime  NOT NULL,
    [GNGeneId] bigint  NOT NULL
);
GO

-- Creating table 'GNAnalysisRequestGNTemplates'
CREATE TABLE [gn].[GNAnalysisRequestGNTemplates] (
    [Id] uniqueidentifier  NOT NULL,
    [GNAnalysisRequestId] uniqueidentifier  NOT NULL,
    [GNTemplateId] int  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreatedDateTime] datetime  NOT NULL
);
GO

-- Creating table 'GNGenes'
CREATE TABLE [gn].[GNGenes] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [GeneCode] nvarchar(25)  NOT NULL,
    [Chromosome] nvarchar(10)  NOT NULL,
    [CytoBand] nvarchar(10)  NOT NULL,
    [Isoform] nvarchar(15)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNSampleQualifiers'
CREATE TABLE [gn].[GNSampleQualifiers] (
    [Code] nvarchar(30)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [GNSampleQualifierGroupCode] nvarchar(30)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO

-- Creating table 'GNAnalysisRequestTypes'
CREATE TABLE [gn].[GNAnalysisRequestTypes] (
    [Code] nvarchar(30)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO

-- Creating table 'GNSampleQualifierGroups'
CREATE TABLE [gn].[GNSampleQualifierGroups] (
    [Code] nvarchar(30)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO

-- Creating table 'GNSharedPurchaseOrderOrganizations'
CREATE TABLE [gn].[GNSharedPurchaseOrderOrganizations] (
    [Id] uniqueidentifier  NOT NULL,
    [AmountBilledOnInvoice] float  NOT NULL,
    [GNPurchaseOrderId] uniqueidentifier  NOT NULL,
    [GNInvoiceId] uniqueidentifier  NOT NULL,
    [Notes] nvarchar(max)  NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO

-- Creating table 'GNAnalysisRequestGroups'
CREATE TABLE [gn].[GNAnalysisRequestGroups] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [GNAnalysisRequestId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO

-- Creating table 'GNAnalysisRequestGroupComparisons'
CREATE TABLE [gn].[GNAnalysisRequestGroupComparisons] (
    [Id] uniqueidentifier  NOT NULL,
    [GNAnalysisRequestId] uniqueidentifier  NOT NULL,
    [GNAnalysisRequestControlGroupId] uniqueidentifier  NOT NULL,
    [GNAnalysisRequestComparisonGroupId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNAnalysisAdaptors'
CREATE TABLE [gn].[GNAnalysisAdaptors] (
    [Code] nvarchar(30)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [CreatedBy] uniqueidentifier  NOT NULL,
    [CreateDateTime] datetime  NOT NULL
);
GO

-- Creating table 'GNBillingAccounts'
CREATE TABLE [gn].[GNBillingAccounts] (
    [Id] uniqueidentifier  NOT NULL,
    [GNAccountTypeId] int  NOT NULL,
    [GNOrganizationId] uniqueidentifier  NOT NULL,
    [BillingMode] nchar(1)  NOT NULL,
    [MaxBalanceAllowed] float  NOT NULL,
    [TotalAmountOwed] float  NOT NULL,
    [AvailableCredits] float  NOT NULL,
    [ValidBillingAgreementRequired] bit  NOT NULL,
    [CreateDateTime] datetime  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [GNBillingContact_Id] uniqueidentifier  NOT NULL,
    [GNMailingContact_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNBillingInvoices'
CREATE TABLE [gn].[GNBillingInvoices] (
    [Id] uniqueidentifier  NOT NULL,
    [GNBillingAccountId] uniqueidentifier  NOT NULL,
    [ExternalInvoiceNum] nvarchar(75)  NULL,
    [InvoiceStartDate] datetime  NOT NULL,
    [InvoiceEndDate] datetime  NOT NULL,
    [Status] varchar(20)  NOT NULL,
    [TotalDiscountAmount] float  NOT NULL,
    [Total] float  NOT NULL,
    [TotalPaid] float  NOT NULL,
    [NetTerms] int  NOT NULL,
    [InvoiceCycle] nvarchar(6)  NOT NULL,
    [CreateDateTime] datetime  NULL,
    [CreatedBy] uniqueidentifier  NULL
);
GO

-- Creating table 'GNBillingInvoiceDetails'
CREATE TABLE [gn].[GNBillingInvoiceDetails] (
    [Id] uniqueidentifier  NOT NULL,
    [GNBillingInvoiceId] uniqueidentifier  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [UnitCost] float  NOT NULL,
    [UnitPrice] float  NOT NULL,
    [Quantity] float  NOT NULL,
    [DiscountType] nchar(1)  NULL,
    [DiscountAmount] float  NOT NULL,
    [SubTotal] float  NOT NULL,
    [Total] float  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNBillingPaymentMethods'
CREATE TABLE [gn].[GNBillingPaymentMethods] (
    [Id] uniqueidentifier  NOT NULL,
    [GNBillingAccountId] uniqueidentifier  NOT NULL,
    [GNPaymentMethodTypeId] int  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [LastFourDigits] nvarchar(max)  NOT NULL,
    [PCITokenId] nvarchar(max)  NOT NULL,
    [IsDefault] bit  NOT NULL,
    [IsActive] bit  NOT NULL,
    [UsedForRecurrentPayments] bit  NOT NULL,
    [ExpirationDate] datetime  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNBillingPurchaseOrders'
CREATE TABLE [gn].[GNBillingPurchaseOrders] (
    [Id] uniqueidentifier  NOT NULL,
    [ExternalPONum] nvarchar(50)  NULL,
    [TotalCredits] float  NOT NULL,
    [Balance] float  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [ExpirationDate] float  NOT NULL,
    [CreateDateTime] datetime  NULL,
    [CreatedBy] uniqueidentifier  NULL
);
GO

-- Creating table 'GNBillingPurchaseOrderInvoices'
CREATE TABLE [gn].[GNBillingPurchaseOrderInvoices] (
    [Id] nvarchar(max)  NOT NULL,
    [GNBillingPurchaseOrderId] uniqueidentifier  NOT NULL,
    [GNBillingInvoiceId] uniqueidentifier  NOT NULL,
    [TotalApplied] float  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNBillingPaymentInvoices'
CREATE TABLE [gn].[GNBillingPaymentInvoices] (
    [Id] uniqueidentifier  NOT NULL,
    [TotalApplied] float  NOT NULL,
    [GNBillingInvoiceId] uniqueidentifier  NOT NULL,
    [GNBillingPaymentId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNBillingPayments'
CREATE TABLE [gn].[GNBillingPayments] (
    [Id] uniqueidentifier  NOT NULL,
    [GNPaymentMethodId] uniqueidentifier  NOT NULL,
    [TotalAmount] float  NOT NULL,
    [PaymentDate] datetime  NOT NULL,
    [ExternalTxnId] nvarchar(100)  NOT NULL,
    [Status] nvarchar(20)  NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNBillingTransactions'
CREATE TABLE [gn].[GNBillingTransactions] (
    [Id] uniqueidentifier  NOT NULL,
    [GNBillingInvoiceDetailId] uniqueidentifier  NOT NULL,
    [GNProductId] uniqueidentifier  NOT NULL,
    [Description] varchar(255)  NULL,
    [ValueUsed] float  NOT NULL,
    [ValueUnits] nvarchar(100)  NOT NULL,
    [TotalCost] float  NOT NULL,
    [TotalPrice] float  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [GNTransactionTypeId] int  NOT NULL,
    [GNTeam_Id] uniqueidentifier  NULL,
    [GNAnalysisRequest_Id] uniqueidentifier  NULL,
    [GNProject_Id] uniqueidentifier  NULL
);
GO

-- Creating table 'GNBulkImportGroupStagings'
CREATE TABLE [gn].[GNBulkImportGroupStagings] (
    [ROW_IDX] bigint IDENTITY(1,1) NOT NULL,
    [ANALYSIS_NAME] nvarchar(255)  NULL,
    [CONTROL_GROUP_NAME] nvarchar(255)  NOT NULL,
    [COMPARISON_GROUP_NAME] nvarchar(255)  NOT NULL,
    [GNBulkImportStatusId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNSampleStatus'
CREATE TABLE [gn].[GNSampleStatus] (
    [Id] uniqueidentifier  NOT NULL,
    [GNSampleId] uniqueidentifier  NULL,
    [SampleName] nvarchar(50)  NOT NULL,
    [Repository] nvarchar(1000)  NOT NULL,
    [Message] nvarchar(1000)  NULL,
    [PercentComplete] int  NOT NULL,
    [IsError] bit  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNNewSampleBatches'
CREATE TABLE [gn].[GNNewSampleBatches] (
    [Id] uniqueidentifier  NOT NULL,
    [BatchId] nchar(50)  NOT NULL,
    [GNSequencerJobId] uniqueidentifier  NOT NULL,
    [Repository] nchar(1000)  NULL,
    [Project] nchar(100)  NOT NULL,
    [Type] nchar(20)  NOT NULL,
    [Qualifier] nchar(30)  NOT NULL,
    [ReadType] nchar(20)  NOT NULL,
    [CreateAnalysisPerSample] bit  NOT NULL,
    [AutoStartAnalysis] bit  NOT NULL,
    [TotalSamples] int  NOT NULL,
    [TotalSamplesCompleted] int  NOT NULL,
    [TotalNumberOfFastqFiles] int  NOT NULL,
    [TotalNumberOfFastqFilesCompleted] int  NOT NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNNewSampleBatchStatus'
CREATE TABLE [gn].[GNNewSampleBatchStatus] (
    [Id] uniqueidentifier  NOT NULL,
    [GNNewSampleBatchId] uniqueidentifier  NOT NULL,
    [Repository] nchar(1000)  NULL,
    [Status] nchar(30)  NULL,
    [PercentComplete] int  NOT NULL,
    [IsError] bit  NOT NULL,
    [Message] nchar(1000)  NULL,
    [FilesBucket] nchar(1000)  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNNewSampleBatchSamples'
CREATE TABLE [gn].[GNNewSampleBatchSamples] (
    [Id] uniqueidentifier  NOT NULL,
    [GNNewSampleBatchId] uniqueidentifier  NOT NULL,
    [Name] nchar(50)  NOT NULL,
    [Gender] nchar(1)  NOT NULL,
    [FamilyId] nchar(50)  NOT NULL,
    [RelationId] nchar(1)  NOT NULL,
    [Affected] nchar(1)  NULL,
    [Proband] nchar(1)  NULL,
    [AnalysisName] nchar(50)  NULL,
    [CreateDateTime] datetime  NULL,
    [GNSample_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNSequencerJobs'
CREATE TABLE [gn].[GNSequencerJobs] (
    [Id] uniqueidentifier  NOT NULL,
    [GNOrganizationId] uniqueidentifier  NOT NULL,
    [Project] nchar(100)  NOT NULL,
    [Status] nchar(25)  NOT NULL,
    [CreateDateTime] datetime  NULL,
    [GNProject_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNReplicates'
CREATE TABLE [gn].[GNReplicates] (
    [Code] nchar(2)  NOT NULL,
    [Name] nchar(2)  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [Id] uniqueidentifier  NOT NULL
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

-- Creating table 'GNAnalysisRequestGroupGNSample'
CREATE TABLE [gn].[GNAnalysisRequestGroupGNSample] (
    [GNAnalysisRequestGroups_Id] uniqueidentifier  NOT NULL,
    [GNSamples_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNBillingPurchaseOrderGNBillingAccount'
CREATE TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount] (
    [GNBillingPurchaseOrders_Id] uniqueidentifier  NOT NULL,
    [GNBillingAccounts_Id] uniqueidentifier  NOT NULL
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

-- Creating primary key on [NameLeftRelationship], [NameRightRelationship] in table 'GNSampleRelationshipTypeMappings'
ALTER TABLE [gn].[GNSampleRelationshipTypeMappings]
ADD CONSTRAINT [PK_GNSampleRelationshipTypeMappings]
    PRIMARY KEY CLUSTERED ([NameLeftRelationship], [NameRightRelationship] ASC);
GO

-- Creating primary key on [Id] in table 'GNBulkImportStatus'
ALTER TABLE [gn].[GNBulkImportStatus]
ADD CONSTRAINT [PK_GNBulkImportStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBulkImportLogs'
ALTER TABLE [gn].[GNBulkImportLogs]
ADD CONSTRAINT [PK_GNBulkImportLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ROW_IDX] in table 'GNBulkImportStaging'
ALTER TABLE [gn].[GNBulkImportStaging]
ADD CONSTRAINT [PK_GNBulkImportStaging]
    PRIMARY KEY CLUSTERED ([ROW_IDX] ASC);
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

-- Creating primary key on [Id] in table 'AWSComputeEnvironments'
ALTER TABLE [gn].[AWSComputeEnvironments]
ADD CONSTRAINT [PK_AWSComputeEnvironments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNNotificationSuppressionLists'
ALTER TABLE [gn].[GNNotificationSuppressionLists]
ADD CONSTRAINT [PK_GNNotificationSuppressionLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNTemplates'
ALTER TABLE [gn].[GNTemplates]
ADD CONSTRAINT [PK_GNTemplates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNTemplateGenes'
ALTER TABLE [gn].[GNTemplateGenes]
ADD CONSTRAINT [PK_GNTemplateGenes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisRequestGNTemplates'
ALTER TABLE [gn].[GNAnalysisRequestGNTemplates]
ADD CONSTRAINT [PK_GNAnalysisRequestGNTemplates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNGenes'
ALTER TABLE [gn].[GNGenes]
ADD CONSTRAINT [PK_GNGenes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Code] in table 'GNSampleQualifiers'
ALTER TABLE [gn].[GNSampleQualifiers]
ADD CONSTRAINT [PK_GNSampleQualifiers]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Code] in table 'GNAnalysisRequestTypes'
ALTER TABLE [gn].[GNAnalysisRequestTypes]
ADD CONSTRAINT [PK_GNAnalysisRequestTypes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Code] in table 'GNSampleQualifierGroups'
ALTER TABLE [gn].[GNSampleQualifierGroups]
ADD CONSTRAINT [PK_GNSampleQualifierGroups]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Id] in table 'GNSharedPurchaseOrderOrganizations'
ALTER TABLE [gn].[GNSharedPurchaseOrderOrganizations]
ADD CONSTRAINT [PK_GNSharedPurchaseOrderOrganizations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisRequestGroups'
ALTER TABLE [gn].[GNAnalysisRequestGroups]
ADD CONSTRAINT [PK_GNAnalysisRequestGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNAnalysisRequestGroupComparisons'
ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons]
ADD CONSTRAINT [PK_GNAnalysisRequestGroupComparisons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Code] in table 'GNAnalysisAdaptors'
ALTER TABLE [gn].[GNAnalysisAdaptors]
ADD CONSTRAINT [PK_GNAnalysisAdaptors]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingAccounts'
ALTER TABLE [gn].[GNBillingAccounts]
ADD CONSTRAINT [PK_GNBillingAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingInvoices'
ALTER TABLE [gn].[GNBillingInvoices]
ADD CONSTRAINT [PK_GNBillingInvoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingInvoiceDetails'
ALTER TABLE [gn].[GNBillingInvoiceDetails]
ADD CONSTRAINT [PK_GNBillingInvoiceDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingPaymentMethods'
ALTER TABLE [gn].[GNBillingPaymentMethods]
ADD CONSTRAINT [PK_GNBillingPaymentMethods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingPurchaseOrders'
ALTER TABLE [gn].[GNBillingPurchaseOrders]
ADD CONSTRAINT [PK_GNBillingPurchaseOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingPurchaseOrderInvoices'
ALTER TABLE [gn].[GNBillingPurchaseOrderInvoices]
ADD CONSTRAINT [PK_GNBillingPurchaseOrderInvoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingPaymentInvoices'
ALTER TABLE [gn].[GNBillingPaymentInvoices]
ADD CONSTRAINT [PK_GNBillingPaymentInvoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingPayments'
ALTER TABLE [gn].[GNBillingPayments]
ADD CONSTRAINT [PK_GNBillingPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [PK_GNBillingTransactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ROW_IDX] in table 'GNBulkImportGroupStagings'
ALTER TABLE [gn].[GNBulkImportGroupStagings]
ADD CONSTRAINT [PK_GNBulkImportGroupStagings]
    PRIMARY KEY CLUSTERED ([ROW_IDX] ASC);
GO

-- Creating primary key on [Id] in table 'GNSampleStatus'
ALTER TABLE [gn].[GNSampleStatus]
ADD CONSTRAINT [PK_GNSampleStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNNewSampleBatches'
ALTER TABLE [gn].[GNNewSampleBatches]
ADD CONSTRAINT [PK_GNNewSampleBatches]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNNewSampleBatchStatus'
ALTER TABLE [gn].[GNNewSampleBatchStatus]
ADD CONSTRAINT [PK_GNNewSampleBatchStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNNewSampleBatchSamples'
ALTER TABLE [gn].[GNNewSampleBatchSamples]
ADD CONSTRAINT [PK_GNNewSampleBatchSamples]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNSequencerJobs'
ALTER TABLE [gn].[GNSequencerJobs]
ADD CONSTRAINT [PK_GNSequencerJobs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNReplicates'
ALTER TABLE [gn].[GNReplicates]
ADD CONSTRAINT [PK_GNReplicates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
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

-- Creating primary key on [GNAnalysisRequestGroups_Id], [GNSamples_Id] in table 'GNAnalysisRequestGroupGNSample'
ALTER TABLE [gn].[GNAnalysisRequestGroupGNSample]
ADD CONSTRAINT [PK_GNAnalysisRequestGroupGNSample]
    PRIMARY KEY CLUSTERED ([GNAnalysisRequestGroups_Id], [GNSamples_Id] ASC);
GO

-- Creating primary key on [GNBillingPurchaseOrders_Id], [GNBillingAccounts_Id] in table 'GNBillingPurchaseOrderGNBillingAccount'
ALTER TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount]
ADD CONSTRAINT [PK_GNBillingPurchaseOrderGNBillingAccount]
    PRIMARY KEY CLUSTERED ([GNBillingPurchaseOrders_Id], [GNBillingAccounts_Id] ASC);
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

-- Creating foreign key on [GNBulkImportStatusId] in table 'GNBulkImportLogs'
ALTER TABLE [gn].[GNBulkImportLogs]
ADD CONSTRAINT [FK_GNBulkImportStatusGNBulkImportLog]
    FOREIGN KEY ([GNBulkImportStatusId])
    REFERENCES [gn].[GNBulkImportStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBulkImportStatusGNBulkImportLog'
CREATE INDEX [IX_FK_GNBulkImportStatusGNBulkImportLog]
ON [gn].[GNBulkImportLogs]
    ([GNBulkImportStatusId]);
GO

-- Creating foreign key on [GNBulkImportStatusId] in table 'GNBulkImportStaging'
ALTER TABLE [gn].[GNBulkImportStaging]
ADD CONSTRAINT [FK_GNBulkImportStatusGNBulkImportStaging]
    FOREIGN KEY ([GNBulkImportStatusId])
    REFERENCES [gn].[GNBulkImportStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBulkImportStatusGNBulkImportStaging'
CREATE INDEX [IX_FK_GNBulkImportStatusGNBulkImportStaging]
ON [gn].[GNBulkImportStaging]
    ([GNBulkImportStatusId]);
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

-- Creating foreign key on [GNSettingsTemplateId] in table 'GNOrganizations'
ALTER TABLE [gn].[GNOrganizations]
ADD CONSTRAINT [FK_GNOrganizationGNSettingsTemplate]
    FOREIGN KEY ([GNSettingsTemplateId])
    REFERENCES [gn].[GNSettingsTemplates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNOrganizationGNSettingsTemplate'
CREATE INDEX [IX_FK_GNOrganizationGNSettingsTemplate]
ON [gn].[GNOrganizations]
    ([GNSettingsTemplateId]);
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

-- Creating foreign key on [GNAnalysisRequestId] in table 'GNAnalysisRequestGNTemplates'
ALTER TABLE [gn].[GNAnalysisRequestGNTemplates]
ADD CONSTRAINT [FK_GNAnalysisRequestGNTemplateGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNTemplateGNAnalysisRequest'
CREATE INDEX [IX_FK_GNAnalysisRequestGNTemplateGNAnalysisRequest]
ON [gn].[GNAnalysisRequestGNTemplates]
    ([GNAnalysisRequestId]);
GO

-- Creating foreign key on [GNTemplateId] in table 'GNAnalysisRequestGNTemplates'
ALTER TABLE [gn].[GNAnalysisRequestGNTemplates]
ADD CONSTRAINT [FK_GNAnalysisRequestGNTemplateGNTemplate]
    FOREIGN KEY ([GNTemplateId])
    REFERENCES [gn].[GNTemplates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNTemplateGNTemplate'
CREATE INDEX [IX_FK_GNAnalysisRequestGNTemplateGNTemplate]
ON [gn].[GNAnalysisRequestGNTemplates]
    ([GNTemplateId]);
GO

-- Creating foreign key on [GNTemplateId] in table 'GNTemplateGenes'
ALTER TABLE [gn].[GNTemplateGenes]
ADD CONSTRAINT [FK_GNTemplateGNTemplateGene]
    FOREIGN KEY ([GNTemplateId])
    REFERENCES [gn].[GNTemplates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTemplateGNTemplateGene'
CREATE INDEX [IX_FK_GNTemplateGNTemplateGene]
ON [gn].[GNTemplateGenes]
    ([GNTemplateId]);
GO

-- Creating foreign key on [GNOrganizationId] in table 'GNTemplates'
ALTER TABLE [gn].[GNTemplates]
ADD CONSTRAINT [FK_GNTemplateGNOrganization]
    FOREIGN KEY ([GNOrganizationId])
    REFERENCES [gn].[GNOrganizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTemplateGNOrganization'
CREATE INDEX [IX_FK_GNTemplateGNOrganization]
ON [gn].[GNTemplates]
    ([GNOrganizationId]);
GO

-- Creating foreign key on [GNGeneId] in table 'GNTemplateGenes'
ALTER TABLE [gn].[GNTemplateGenes]
ADD CONSTRAINT [FK_GNTemplateGeneGNGene]
    FOREIGN KEY ([GNGeneId])
    REFERENCES [gn].[GNGenes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTemplateGeneGNGene'
CREATE INDEX [IX_FK_GNTemplateGeneGNGene]
ON [gn].[GNTemplateGenes]
    ([GNGeneId]);
GO

-- Creating foreign key on [GNAnalysisRequestTypeCode] in table 'GNAnalysisRequests'
ALTER TABLE [gn].[GNAnalysisRequests]
ADD CONSTRAINT [FK_GNAnalysisRequestGNAnalysisRequestType]
    FOREIGN KEY ([GNAnalysisRequestTypeCode])
    REFERENCES [gn].[GNAnalysisRequestTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGNAnalysisRequestType'
CREATE INDEX [IX_FK_GNAnalysisRequestGNAnalysisRequestType]
ON [gn].[GNAnalysisRequests]
    ([GNAnalysisRequestTypeCode]);
GO

-- Creating foreign key on [GNAnalysisRequest_Id] in table 'GNTransactions'
ALTER TABLE [gn].[GNTransactions]
ADD CONSTRAINT [FK_GNTransactionGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequest_Id])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTransactionGNAnalysisRequest'
CREATE INDEX [IX_FK_GNTransactionGNAnalysisRequest]
ON [gn].[GNTransactions]
    ([GNAnalysisRequest_Id]);
GO

-- Creating foreign key on [GNProject_Id] in table 'GNTransactions'
ALTER TABLE [gn].[GNTransactions]
ADD CONSTRAINT [FK_GNTransactionGNProject]
    FOREIGN KEY ([GNProject_Id])
    REFERENCES [gn].[GNProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTransactionGNProject'
CREATE INDEX [IX_FK_GNTransactionGNProject]
ON [gn].[GNTransactions]
    ([GNProject_Id]);
GO

-- Creating foreign key on [GNTeam_Id] in table 'GNTransactions'
ALTER TABLE [gn].[GNTransactions]
ADD CONSTRAINT [FK_GNTransactionGNTeam]
    FOREIGN KEY ([GNTeam_Id])
    REFERENCES [gn].[GNTeams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNTransactionGNTeam'
CREATE INDEX [IX_FK_GNTransactionGNTeam]
ON [gn].[GNTransactions]
    ([GNTeam_Id]);
GO

-- Creating foreign key on [GNSampleQualifierGroupCode] in table 'GNSampleQualifiers'
ALTER TABLE [gn].[GNSampleQualifiers]
ADD CONSTRAINT [FK_GNSampleQualifierGNSampleQualifierGroup]
    FOREIGN KEY ([GNSampleQualifierGroupCode])
    REFERENCES [gn].[GNSampleQualifierGroups]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleQualifierGNSampleQualifierGroup'
CREATE INDEX [IX_FK_GNSampleQualifierGNSampleQualifierGroup]
ON [gn].[GNSampleQualifiers]
    ([GNSampleQualifierGroupCode]);
GO

-- Creating foreign key on [GNSampleQualifierCode] in table 'GNSamples'
ALTER TABLE [gn].[GNSamples]
ADD CONSTRAINT [FK_GNSampleQualifierGNSample]
    FOREIGN KEY ([GNSampleQualifierCode])
    REFERENCES [gn].[GNSampleQualifiers]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleQualifierGNSample'
CREATE INDEX [IX_FK_GNSampleQualifierGNSample]
ON [gn].[GNSamples]
    ([GNSampleQualifierCode]);
GO

-- Creating foreign key on [GNInvoiceId] in table 'GNSharedPurchaseOrderOrganizations'
ALTER TABLE [gn].[GNSharedPurchaseOrderOrganizations]
ADD CONSTRAINT [FK_GNSharedPurchaseOrderOrganizationGNInvoice]
    FOREIGN KEY ([GNInvoiceId])
    REFERENCES [gn].[GNInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSharedPurchaseOrderOrganizationGNInvoice'
CREATE INDEX [IX_FK_GNSharedPurchaseOrderOrganizationGNInvoice]
ON [gn].[GNSharedPurchaseOrderOrganizations]
    ([GNInvoiceId]);
GO

-- Creating foreign key on [GNPurchaseOrderId] in table 'GNSharedPurchaseOrderOrganizations'
ALTER TABLE [gn].[GNSharedPurchaseOrderOrganizations]
ADD CONSTRAINT [FK_GNSharedPurchaseOrderOrganizationGNPurchaseOrder]
    FOREIGN KEY ([GNPurchaseOrderId])
    REFERENCES [gn].[GNPurchaseOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSharedPurchaseOrderOrganizationGNPurchaseOrder'
CREATE INDEX [IX_FK_GNSharedPurchaseOrderOrganizationGNPurchaseOrder]
ON [gn].[GNSharedPurchaseOrderOrganizations]
    ([GNPurchaseOrderId]);
GO

-- Creating foreign key on [GNAnalysisRequestId] in table 'GNAnalysisRequestGroups'
ALTER TABLE [gn].[GNAnalysisRequestGroups]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupGNAnalysisRequest'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupGNAnalysisRequest]
ON [gn].[GNAnalysisRequestGroups]
    ([GNAnalysisRequestId]);
GO

-- Creating foreign key on [GNAnalysisRequestGroups_Id] in table 'GNAnalysisRequestGroupGNSample'
ALTER TABLE [gn].[GNAnalysisRequestGroupGNSample]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupGNSample_GNAnalysisRequestGroup]
    FOREIGN KEY ([GNAnalysisRequestGroups_Id])
    REFERENCES [gn].[GNAnalysisRequestGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GNSamples_Id] in table 'GNAnalysisRequestGroupGNSample'
ALTER TABLE [gn].[GNAnalysisRequestGroupGNSample]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupGNSample_GNSample]
    FOREIGN KEY ([GNSamples_Id])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupGNSample_GNSample'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupGNSample_GNSample]
ON [gn].[GNAnalysisRequestGroupGNSample]
    ([GNSamples_Id]);
GO

-- Creating foreign key on [GNAnalysisRequestControlGroupId] in table 'GNAnalysisRequestGroupComparisons'
ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupComparisonGNAnalysisRequestGroup]
    FOREIGN KEY ([GNAnalysisRequestControlGroupId])
    REFERENCES [gn].[GNAnalysisRequestGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupComparisonGNAnalysisRequestGroup'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupComparisonGNAnalysisRequestGroup]
ON [gn].[GNAnalysisRequestGroupComparisons]
    ([GNAnalysisRequestControlGroupId]);
GO

-- Creating foreign key on [GNAnalysisRequestComparisonGroupId] in table 'GNAnalysisRequestGroupComparisons'
ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupGNAnalysisRequestGroupComparison]
    FOREIGN KEY ([GNAnalysisRequestComparisonGroupId])
    REFERENCES [gn].[GNAnalysisRequestGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupGNAnalysisRequestGroupComparison'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupGNAnalysisRequestGroupComparison]
ON [gn].[GNAnalysisRequestGroupComparisons]
    ([GNAnalysisRequestComparisonGroupId]);
GO

-- Creating foreign key on [GNAnalysisRequestId] in table 'GNAnalysisRequestGroupComparisons'
ALTER TABLE [gn].[GNAnalysisRequestGroupComparisons]
ADD CONSTRAINT [FK_GNAnalysisRequestGroupComparisonGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequestId])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisRequestGroupComparisonGNAnalysisRequest'
CREATE INDEX [IX_FK_GNAnalysisRequestGroupComparisonGNAnalysisRequest]
ON [gn].[GNAnalysisRequestGroupComparisons]
    ([GNAnalysisRequestId]);
GO

-- Creating foreign key on [GNAnalysisAdaptorCode] in table 'GNAnalysisRequests'
ALTER TABLE [gn].[GNAnalysisRequests]
ADD CONSTRAINT [FK_GNAnalysisAdaptorGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisAdaptorCode])
    REFERENCES [gn].[GNAnalysisAdaptors]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNAnalysisAdaptorGNAnalysisRequest'
CREATE INDEX [IX_FK_GNAnalysisAdaptorGNAnalysisRequest]
ON [gn].[GNAnalysisRequests]
    ([GNAnalysisAdaptorCode]);
GO

-- Creating foreign key on [GNBillingPurchaseOrders_Id] in table 'GNBillingPurchaseOrderGNBillingAccount'
ALTER TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount]
ADD CONSTRAINT [FK_GNBillingPurchaseOrderGNBillingAccount_GNBillingPurchaseOrder]
    FOREIGN KEY ([GNBillingPurchaseOrders_Id])
    REFERENCES [gn].[GNBillingPurchaseOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [GNBillingAccounts_Id] in table 'GNBillingPurchaseOrderGNBillingAccount'
ALTER TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount]
ADD CONSTRAINT [FK_GNBillingPurchaseOrderGNBillingAccount_GNBillingAccount]
    FOREIGN KEY ([GNBillingAccounts_Id])
    REFERENCES [gn].[GNBillingAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPurchaseOrderGNBillingAccount_GNBillingAccount'
CREATE INDEX [IX_FK_GNBillingPurchaseOrderGNBillingAccount_GNBillingAccount]
ON [gn].[GNBillingPurchaseOrderGNBillingAccount]
    ([GNBillingAccounts_Id]);
GO

-- Creating foreign key on [GNBillingAccountId] in table 'GNBillingPaymentMethods'
ALTER TABLE [gn].[GNBillingPaymentMethods]
ADD CONSTRAINT [FK_GNBillingAccountGNBillingPaymentMethod]
    FOREIGN KEY ([GNBillingAccountId])
    REFERENCES [gn].[GNBillingAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingAccountGNBillingPaymentMethod'
CREATE INDEX [IX_FK_GNBillingAccountGNBillingPaymentMethod]
ON [gn].[GNBillingPaymentMethods]
    ([GNBillingAccountId]);
GO

-- Creating foreign key on [GNBillingInvoiceId] in table 'GNBillingPurchaseOrderInvoices'
ALTER TABLE [gn].[GNBillingPurchaseOrderInvoices]
ADD CONSTRAINT [FK_GNBillingInvoiceGNBillingPurchaseOrderInvoice]
    FOREIGN KEY ([GNBillingInvoiceId])
    REFERENCES [gn].[GNBillingInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingInvoiceGNBillingPurchaseOrderInvoice'
CREATE INDEX [IX_FK_GNBillingInvoiceGNBillingPurchaseOrderInvoice]
ON [gn].[GNBillingPurchaseOrderInvoices]
    ([GNBillingInvoiceId]);
GO

-- Creating foreign key on [GNBillingPurchaseOrderId] in table 'GNBillingPurchaseOrderInvoices'
ALTER TABLE [gn].[GNBillingPurchaseOrderInvoices]
ADD CONSTRAINT [FK_GNBillingPurchaseOrderGNBillingPurchaseOrderInvoice]
    FOREIGN KEY ([GNBillingPurchaseOrderId])
    REFERENCES [gn].[GNBillingPurchaseOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPurchaseOrderGNBillingPurchaseOrderInvoice'
CREATE INDEX [IX_FK_GNBillingPurchaseOrderGNBillingPurchaseOrderInvoice]
ON [gn].[GNBillingPurchaseOrderInvoices]
    ([GNBillingPurchaseOrderId]);
GO

-- Creating foreign key on [GNOrganizationId] in table 'GNBillingAccounts'
ALTER TABLE [gn].[GNBillingAccounts]
ADD CONSTRAINT [FK_GNBillingAccountGNOrganization]
    FOREIGN KEY ([GNOrganizationId])
    REFERENCES [gn].[GNOrganizations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingAccountGNOrganization'
CREATE INDEX [IX_FK_GNBillingAccountGNOrganization]
ON [gn].[GNBillingAccounts]
    ([GNOrganizationId]);
GO

-- Creating foreign key on [GNAccountTypeId] in table 'GNBillingAccounts'
ALTER TABLE [gn].[GNBillingAccounts]
ADD CONSTRAINT [FK_GNBillingAccountGNAccountType]
    FOREIGN KEY ([GNAccountTypeId])
    REFERENCES [gn].[GNAccountTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingAccountGNAccountType'
CREATE INDEX [IX_FK_GNBillingAccountGNAccountType]
ON [gn].[GNBillingAccounts]
    ([GNAccountTypeId]);
GO

-- Creating foreign key on [GNPaymentMethodTypeId] in table 'GNBillingPaymentMethods'
ALTER TABLE [gn].[GNBillingPaymentMethods]
ADD CONSTRAINT [FK_GNBillingPaymentMethodGNPaymentMethodType]
    FOREIGN KEY ([GNPaymentMethodTypeId])
    REFERENCES [gn].[GNPaymentMethodTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPaymentMethodGNPaymentMethodType'
CREATE INDEX [IX_FK_GNBillingPaymentMethodGNPaymentMethodType]
ON [gn].[GNBillingPaymentMethods]
    ([GNPaymentMethodTypeId]);
GO

-- Creating foreign key on [GNTransactionTypeId] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [FK_GNBillingTransactionGNTransactionType]
    FOREIGN KEY ([GNTransactionTypeId])
    REFERENCES [gn].[GNTransactionTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingTransactionGNTransactionType'
CREATE INDEX [IX_FK_GNBillingTransactionGNTransactionType]
ON [gn].[GNBillingTransactions]
    ([GNTransactionTypeId]);
GO

-- Creating foreign key on [GNTeam_Id] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [FK_GNBillingTransactionGNTeam]
    FOREIGN KEY ([GNTeam_Id])
    REFERENCES [gn].[GNTeams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingTransactionGNTeam'
CREATE INDEX [IX_FK_GNBillingTransactionGNTeam]
ON [gn].[GNBillingTransactions]
    ([GNTeam_Id]);
GO

-- Creating foreign key on [GNAnalysisRequest_Id] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [FK_GNBillingTransactionGNAnalysisRequest]
    FOREIGN KEY ([GNAnalysisRequest_Id])
    REFERENCES [gn].[GNAnalysisRequests]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingTransactionGNAnalysisRequest'
CREATE INDEX [IX_FK_GNBillingTransactionGNAnalysisRequest]
ON [gn].[GNBillingTransactions]
    ([GNAnalysisRequest_Id]);
GO

-- Creating foreign key on [GNProject_Id] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [FK_GNBillingTransactionGNProject]
    FOREIGN KEY ([GNProject_Id])
    REFERENCES [gn].[GNProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingTransactionGNProject'
CREATE INDEX [IX_FK_GNBillingTransactionGNProject]
ON [gn].[GNBillingTransactions]
    ([GNProject_Id]);
GO

-- Creating foreign key on [GNBillingContact_Id] in table 'GNBillingAccounts'
ALTER TABLE [gn].[GNBillingAccounts]
ADD CONSTRAINT [FK_GNContactGNBillingAccount]
    FOREIGN KEY ([GNBillingContact_Id])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNContactGNBillingAccount'
CREATE INDEX [IX_FK_GNContactGNBillingAccount]
ON [gn].[GNBillingAccounts]
    ([GNBillingContact_Id]);
GO

-- Creating foreign key on [GNMailingContact_Id] in table 'GNBillingAccounts'
ALTER TABLE [gn].[GNBillingAccounts]
ADD CONSTRAINT [FK_GNBillingAccountGNContact]
    FOREIGN KEY ([GNMailingContact_Id])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingAccountGNContact'
CREATE INDEX [IX_FK_GNBillingAccountGNContact]
ON [gn].[GNBillingAccounts]
    ([GNMailingContact_Id]);
GO

-- Creating foreign key on [GNBillingInvoiceId] in table 'GNBillingInvoiceDetails'
ALTER TABLE [gn].[GNBillingInvoiceDetails]
ADD CONSTRAINT [FK_GNBillingInvoiceDetailGNBillingInvoice]
    FOREIGN KEY ([GNBillingInvoiceId])
    REFERENCES [gn].[GNBillingInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingInvoiceDetailGNBillingInvoice'
CREATE INDEX [IX_FK_GNBillingInvoiceDetailGNBillingInvoice]
ON [gn].[GNBillingInvoiceDetails]
    ([GNBillingInvoiceId]);
GO

-- Creating foreign key on [GNBillingInvoiceDetailId] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [FK_GNBillingInvoiceDetailGNBillingTransaction]
    FOREIGN KEY ([GNBillingInvoiceDetailId])
    REFERENCES [gn].[GNBillingInvoiceDetails]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingInvoiceDetailGNBillingTransaction'
CREATE INDEX [IX_FK_GNBillingInvoiceDetailGNBillingTransaction]
ON [gn].[GNBillingTransactions]
    ([GNBillingInvoiceDetailId]);
GO

-- Creating foreign key on [GNProductId] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [FK_GNBillingTransactionGNProduct]
    FOREIGN KEY ([GNProductId])
    REFERENCES [gn].[GNProducts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingTransactionGNProduct'
CREATE INDEX [IX_FK_GNBillingTransactionGNProduct]
ON [gn].[GNBillingTransactions]
    ([GNProductId]);
GO

-- Creating foreign key on [GNBillingAccountId] in table 'GNBillingInvoices'
ALTER TABLE [gn].[GNBillingInvoices]
ADD CONSTRAINT [FK_GNBillingInvoiceGNBillingAccount]
    FOREIGN KEY ([GNBillingAccountId])
    REFERENCES [gn].[GNBillingAccounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingInvoiceGNBillingAccount'
CREATE INDEX [IX_FK_GNBillingInvoiceGNBillingAccount]
ON [gn].[GNBillingInvoices]
    ([GNBillingAccountId]);
GO

-- Creating foreign key on [GNPaymentMethodId] in table 'GNBillingPayments'
ALTER TABLE [gn].[GNBillingPayments]
ADD CONSTRAINT [FK_GNBillingPaymentGNPaymentMethod]
    FOREIGN KEY ([GNPaymentMethodId])
    REFERENCES [gn].[GNPaymentMethods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPaymentGNPaymentMethod'
CREATE INDEX [IX_FK_GNBillingPaymentGNPaymentMethod]
ON [gn].[GNBillingPayments]
    ([GNPaymentMethodId]);
GO

-- Creating foreign key on [GNBillingInvoiceId] in table 'GNBillingPaymentInvoices'
ALTER TABLE [gn].[GNBillingPaymentInvoices]
ADD CONSTRAINT [FK_GNBillingPaymentInvoiceGNBillingInvoice]
    FOREIGN KEY ([GNBillingInvoiceId])
    REFERENCES [gn].[GNBillingInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPaymentInvoiceGNBillingInvoice'
CREATE INDEX [IX_FK_GNBillingPaymentInvoiceGNBillingInvoice]
ON [gn].[GNBillingPaymentInvoices]
    ([GNBillingInvoiceId]);
GO

-- Creating foreign key on [GNBillingPaymentId] in table 'GNBillingPaymentInvoices'
ALTER TABLE [gn].[GNBillingPaymentInvoices]
ADD CONSTRAINT [FK_GNBillingPaymentInvoiceGNBillingPayment]
    FOREIGN KEY ([GNBillingPaymentId])
    REFERENCES [gn].[GNBillingPayments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPaymentInvoiceGNBillingPayment'
CREATE INDEX [IX_FK_GNBillingPaymentInvoiceGNBillingPayment]
ON [gn].[GNBillingPaymentInvoices]
    ([GNBillingPaymentId]);
GO

-- Creating foreign key on [GNBulkImportStatusId] in table 'GNBulkImportGroupStagings'
ALTER TABLE [gn].[GNBulkImportGroupStagings]
ADD CONSTRAINT [FK_GNBulkImportGroupStagingGNBulkImportStatus]
    FOREIGN KEY ([GNBulkImportStatusId])
    REFERENCES [gn].[GNBulkImportStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBulkImportGroupStagingGNBulkImportStatus'
CREATE INDEX [IX_FK_GNBulkImportGroupStagingGNBulkImportStatus]
ON [gn].[GNBulkImportGroupStagings]
    ([GNBulkImportStatusId]);
GO

-- Creating foreign key on [GNSampleId] in table 'GNSampleStatus'
ALTER TABLE [gn].[GNSampleStatus]
ADD CONSTRAINT [FK_GNSampleStatusGNSample]
    FOREIGN KEY ([GNSampleId])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSampleStatusGNSample'
CREATE INDEX [IX_FK_GNSampleStatusGNSample]
ON [gn].[GNSampleStatus]
    ([GNSampleId]);
GO

-- Creating foreign key on [GNNewSampleBatchId] in table 'GNNewSampleBatchStatus'
ALTER TABLE [gn].[GNNewSampleBatchStatus]
ADD CONSTRAINT [FK_GNNewSampleBatchStatusGNNewSampleBatch]
    FOREIGN KEY ([GNNewSampleBatchId])
    REFERENCES [gn].[GNNewSampleBatches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNewSampleBatchStatusGNNewSampleBatch'
CREATE INDEX [IX_FK_GNNewSampleBatchStatusGNNewSampleBatch]
ON [gn].[GNNewSampleBatchStatus]
    ([GNNewSampleBatchId]);
GO

-- Creating foreign key on [GNNewSampleBatchId] in table 'GNNewSampleBatchSamples'
ALTER TABLE [gn].[GNNewSampleBatchSamples]
ADD CONSTRAINT [FK_GNNewSampleBatchSamplesGNNewSampleBatch]
    FOREIGN KEY ([GNNewSampleBatchId])
    REFERENCES [gn].[GNNewSampleBatches]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNewSampleBatchSamplesGNNewSampleBatch'
CREATE INDEX [IX_FK_GNNewSampleBatchSamplesGNNewSampleBatch]
ON [gn].[GNNewSampleBatchSamples]
    ([GNNewSampleBatchId]);
GO

-- Creating foreign key on [GNSample_Id] in table 'GNNewSampleBatchSamples'
ALTER TABLE [gn].[GNNewSampleBatchSamples]
ADD CONSTRAINT [FK_GNNewSampleBatchSamplesGNSample]
    FOREIGN KEY ([GNSample_Id])
    REFERENCES [gn].[GNSamples]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNewSampleBatchSamplesGNSample'
CREATE INDEX [IX_FK_GNNewSampleBatchSamplesGNSample]
ON [gn].[GNNewSampleBatchSamples]
    ([GNSample_Id]);
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

-- Creating foreign key on [GNSequencerJobId] in table 'GNNewSampleBatches'
ALTER TABLE [gn].[GNNewSampleBatches]
ADD CONSTRAINT [FK_GNNewSampleBatchGNSequencerJob]
    FOREIGN KEY ([GNSequencerJobId])
    REFERENCES [gn].[GNSequencerJobs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNNewSampleBatchGNSequencerJob'
CREATE INDEX [IX_FK_GNNewSampleBatchGNSequencerJob]
ON [gn].[GNNewSampleBatches]
    ([GNSequencerJobId]);
GO

-- Creating foreign key on [GNProject_Id] in table 'GNSequencerJobs'
ALTER TABLE [gn].[GNSequencerJobs]
ADD CONSTRAINT [FK_GNSequencerJobGNProject]
    FOREIGN KEY ([GNProject_Id])
    REFERENCES [gn].[GNProjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNSequencerJobGNProject'
CREATE INDEX [IX_FK_GNSequencerJobGNProject]
ON [gn].[GNSequencerJobs]
    ([GNProject_Id]);
GO

-- Creating foreign key on [GNReplicate_Id] in table 'GNSamples'
ALTER TABLE [gn].[GNSamples]
ADD CONSTRAINT [FK_GNReplicateGNSample]
    FOREIGN KEY ([GNReplicate_Id])
    REFERENCES [gn].[GNReplicates]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNReplicateGNSample'
CREATE INDEX [IX_FK_GNReplicateGNSample]
ON [gn].[GNSamples]
    ([GNReplicate_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------