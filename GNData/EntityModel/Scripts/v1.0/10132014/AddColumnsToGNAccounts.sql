ALTER TABLE [gn].[GNAccounts]
    ADD [MaxBalanceAllowed]             FLOAT (53) DEFAULT 500.0 NOT NULL,
        [ValidBillingAgreementRequired] BIT        DEFAULT 1 NOT NULL;
GO
