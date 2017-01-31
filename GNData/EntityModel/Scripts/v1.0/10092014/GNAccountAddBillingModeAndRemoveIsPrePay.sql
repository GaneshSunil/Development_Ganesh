ALTER TABLE [gn].[GNAccounts] DROP COLUMN [IsPrePay];
GO

ALTER TABLE [gn].[GNAccounts]
    ADD [BillingMode] NCHAR (1) DEFAULT 'C' NOT NULL;
GO
