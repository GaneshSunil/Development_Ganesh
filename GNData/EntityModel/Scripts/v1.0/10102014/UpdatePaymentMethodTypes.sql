USE [gn_db]
GO

UPDATE [gn].[GNPaymentMethodTypes]
   SET [Name] = 'PURCHASE_ORDER'
      ,[Description] = 'Purchase Order'
 WHERE [Id] = 1
GO


