USE [gn_db]
GO
SET IDENTITY_INSERT [gn].[GNAccountTypes] ON 

GO
INSERT [gn].[GNAccountTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (1, N'INDUSTRY', N'Industry', NULL, NULL)
GO
INSERT [gn].[GNAccountTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (2, N'ACADEMIC', N'Academic / Non-Profit', NULL, NULL)
GO
INSERT [gn].[GNAccountTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (3, N'NCH Academic', N'NCH Academic', N'a30fed0d-6891-4748-8c9c-350b7a97895d', CAST(N'2014-12-10 14:49:57.027' AS DateTime))
GO
INSERT [gn].[GNAccountTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (4, N'NCH Commercial', N'NCH Commercial', N'a30fed0d-6891-4748-8c9c-350b7a97895d', CAST(N'2014-12-10 14:50:15.100' AS DateTime))
GO
SET IDENTITY_INSERT [gn].[GNAccountTypes] OFF
GO

-- add NCH Academic products (minus panel)
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_GENOME', N'Analysis Request for Whole Genome', 2, 0, 147, 147, 0, 0, N'N/A', 3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:51.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_UPLOAD', N'Upload to S3 Storage (1 GB)', 1, 0, 0.03, 0.03, 0, 0, N'GB', 3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-02 17:34:52.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_EXOME', N'Analysis Request for Exome', 2, 0, 13, 13, 0, 0, N'N/A', 3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:07.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_CARRYOVER', N'Carry-Over of Files on S3 Storage', 1, 0, 0.03, 0.03, 0, 0, N'GB', 3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 10:21:46.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_DELETE', N'Delete from S3 Storage', 1, 0, 0, 0, 1, 1, N'GB', 3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-02 17:36:01.987' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_DOWNLOAD', N'Download from S3 Storage', 1, 0, 0.12, 0.12, 0, 0, N'GB', 3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 09:22:45.000' AS DateTime))
GO

-- add NCH Commercial products (minus panel)
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_GENOME', N'Analysis Request for Whole Genome', 2, 0, 350, 350, 0, 0, N'N/A', 4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:51.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_UPLOAD', N'Upload to S3 Storage (1 GB)', 1, 0, 0.03, 0.03, 0, 0, N'GB', 4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-02 17:34:52.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_EXOME', N'Analysis Request for Exome', 2, 0, 150, 150, 0, 0, N'N/A', 4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:07.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_CARRYOVER', N'Carry-Over of Files on S3 Storage', 1, 0, 0.03, 0.03, 0, 0, N'GB', 4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 10:21:46.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_DELETE', N'Delete from S3 Storage', 1, 0, 0, 0, 1, 1, N'GB', 4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-02 17:36:01.987' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_DOWNLOAD', N'Download from S3 Storage', 1, 0, 0.12, 0.12, 0, 0, N'GB', 4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 09:22:45.000' AS DateTime))
GO

-- add panel transaction type
SET IDENTITY_INSERT [gn].[GNTransactionTypes] ON 
INSERT [gn].[GNTransactionTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (11, N'ANALYSIS_REQUEST_PANEL', N'Analysis Request for Targeted Panel', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:24:22.000' AS DateTime))
GO
SET IDENTITY_INSERT [gn].[GNTransactionTypes] OFF
GO

-- add targeted panel products
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_PANEL', N'Analysis Request for Targeted Panel', 2, 0, 250, 250, 0, 0, N'N/A', 1, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:07.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_PANEL', N'Analysis Request for Targeted Panel', 2, 0, 50, 50, 0, 0, N'N/A', 2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:07.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_PANEL', N'Analysis Request for Targeted Panel', 2, 0, 6, 6, 0, 0, N'N/A', 3, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:07.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_PANEL', N'Analysis Request for Targeted Panel', 2, 0, 50, 50, 0, 0, N'N/A', 4, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:07.000' AS DateTime))
GO

-- update industry product prices
UPDATE [gn].[GNProducts] SET [Cost] = 500, [Price] = 500
WHERE [Name] = 'ANALYSIS_REQUEST_EXOME' AND [GNAccountTypeId] = 1
GO

UPDATE [gn].[GNProducts] SET [Cost] = 800, [Price] = 800
WHERE [Name] = 'ANALYSIS_REQUEST_GENOME' AND [GNAccountTypeId] = 1
GO

-- update academic product prices
UPDATE [gn].[GNProducts] SET [Cost] = 150, [Price] = 150
WHERE [Name] = 'ANALYSIS_REQUEST_EXOME' AND [GNAccountTypeId] = 2
GO

UPDATE [gn].[GNProducts] SET [Cost] = 350, [Price] = 350
WHERE [Name] = 'ANALYSIS_REQUEST_GENOME' AND [GNAccountTypeId] = 2
GO
