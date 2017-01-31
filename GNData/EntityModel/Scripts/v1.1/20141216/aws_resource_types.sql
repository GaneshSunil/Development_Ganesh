USE [gn_db]
GO
SET IDENTITY_INSERT [gn].[AWSResourceTypes] ON 

GO
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (1, N'S3 Bucket', NULL, NULL)
GO
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (2, N'SQS Queue', NULL, NULL)
GO
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (3, N'SNS Topic', NULL, NULL)
GO
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (4, N'EC2 Instance', NULL, NULL)
GO
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (5, N'AMI', NULL, NULL)
GO
INSERT [gn].[AWSResourceTypes] ([Id], [Name], [CreatedBy], [CreateDateTime]) VALUES (6, N'S3 Bucket for Bulk Import', N'4d488c52-5dec-4d16-8d53-98684b2901c8', CAST(N'2014-12-12 10:01:22.237' AS DateTime))
GO
SET IDENTITY_INSERT [gn].[AWSResourceTypes] OFF