USE [gn_db]
GO

UPDATE [gn].[GNPaymentMethodTypes]
   SET [Name] = 'CHECK'
      ,[Description] = 'Check'
 WHERE [Id] = 1
GO


UPDATE [gn].[GNPaymentMethodTypes]
   SET [Name] = 'CREDIT_CARD'
      ,[Description] = 'Credit Card'
 WHERE [Id] = 2
GO

DELETE FROM [gn].[GNPaymentMethodTypes]
 WHERE [Id] = 3

SET IDENTITY_INSERT [gn].[GNPaymentMethodTypes] ON
INSERT INTO [gn].[GNPaymentMethodTypes] ([Id], [Name], [Description], [CreatedBy], [CreateDateTime]) VALUES (3, N'TRANSFER', N'Transfer', N'4d488c52-5dec-4d16-8d53-98684b2901c8', N'2014-09-23 10:41:40')
SET IDENTITY_INSERT [gn].[GNPaymentMethodTypes] OFF
