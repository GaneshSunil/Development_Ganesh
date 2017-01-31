USE [gn_db]
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_GENOME', N'Analysis Request for Whole Genome', 2, 0, 800, 800, 0, 0, N'N/A', 2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:51.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_UPLOAD', N'Upload to S3 Storage (1 GB)', 1, 0, 0.03, 0.03, 0, 0, N'GB', 2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-02 17:34:52.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'ANALYSIS_REQUEST_EXOME', N'Analysis Request for Exome', 2, 0, 600, 600, 0, 0, N'N/A', 2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-09-23 10:58:07.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_CARRYOVER', N'Carry-Over of Files on S3 Storage', 1, 0, 0.03, 0.03, 0, 0, N'GB', 2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 10:21:46.000' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_DELETE', N'Delete from S3 Storage', 1, 0, 0, 0, 1, 1, N'GB', 2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-02 17:36:01.987' AS DateTime))
GO
INSERT [gn].[GNProducts] ([Id], [Name], [Description], [GNProductTypeId], [SubscribeFrequency], [Cost], [Price], [MinRangeValue], [MaxRangeValue], [ValueUnits], [GNAccountTypeId], [CreatedBy], [CreateDateTime]) 
VALUES (NEWID(), N'STORAGE_S3_DOWNLOAD', N'Download from S3 Storage', 1, 0, 0.12, 0.12, 0, 0, N'GB', 2, N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-10-03 09:22:45.000' AS DateTime))
GO
