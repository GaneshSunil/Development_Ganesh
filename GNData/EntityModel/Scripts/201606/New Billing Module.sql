
-- Creating table 'GNBillingAccounts'
CREATE TABLE [gn].[GNBillingAccounts] (
    [Id] uniqueidentifier  NOT NULL,
    [GNAccountTypeId] int  NOT NULL,
    [GNBillingContactId] uniqueidentifier  NULL,
    [GNMailingContactId] uniqueidentifier  NULL,
    [GNOrganizationId] uniqueidentifier  NOT NULL,
    [BillingMode] nchar(1)  NOT NULL,
    [MaxBalanceAllowed] float  NOT NULL,
    [TotalAmountOwed] float  NOT NULL,
	[AvailableCredits] float  NOT NULL,
    [ValidBillingAgreementRequired] bit  NOT NULL,
    [CreateDateTime] datetime  NULL,
    [CreatedBy] uniqueidentifier  NULL
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


-- Creating table 'GNBillingPaymentMethods'
CREATE TABLE [gn].[GNBillingPaymentMethods] (
    [Id] uniqueidentifier  NOT NULL,
    [GNAccountId] uniqueidentifier  NOT NULL,
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

-- Creating table 'GNBillingInvoiceDetails'
CREATE TABLE [gn].[GNBillingInvoiceDetails] (
    [Id] uniqueidentifier  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [UnitCost] float  NOT NULL,
    [UnitPrice] float  NOT NULL,
    [Quantity] float  NOT NULL,
    [DiscountType] nchar(1)  NULL,
    [DiscountAmount] float  NOT NULL,
    [SubTotal] float  NOT NULL,
    [Total] float  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [GNBillingInvoiceId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GNBillingTransactions'
CREATE TABLE [gn].[GNBillingTransactions] (
    [Id] uniqueidentifier  NOT NULL,
    [GNProductId] uniqueidentifier  NULL,
    [Description] varchar(255)  NULL,
    [ValueUsed] float  NOT NULL,
    [ValueUnits] nvarchar(100)  NOT NULL,
    [TotalCost] float  NOT NULL,
    [TotalPrice] float  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL,
    [GNTransactionTypeId] int  NOT NULL,
    [GNBillingInvoiceDetailId] uniqueidentifier  NOT NULL,
    [GNAnalysisRequest_Id] uniqueidentifier  NULL,
    [GNProject_Id] uniqueidentifier  NULL,
    [GNTeam_Id] uniqueidentifier  NULL
);
GO

-- Creating table 'GNBillingPayments'
CREATE TABLE [gn].[GNBillingPayments] (
    [Id] uniqueidentifier  NOT NULL,
    [GNBillingPaymentMethodId] uniqueidentifier  NOT NULL,
    [TotalAmount] float  NOT NULL,
    [PaymentDate] datetime  NOT NULL,
    [ExternalTxnId] nvarchar(100)  NOT NULL,
    [Status] nvarchar(20)  NULL,
    [GNPaymentMethodId] uniqueidentifier  NOT NULL,
    [CreatedBy] uniqueidentifier  NULL,
    [CreateDateTime] datetime  NULL
);
GO

-- Creating table 'GNBillingAccountPurchaseOrderHistories'
CREATE TABLE [gn].[GNBillingAccountPurchaseOrderHistories] (
    [Id] uniqueidentifier  NOT NULL,
    [UnitsApplied] float  NOT NULL
);
GO

-- Creating table 'GNBillingPurchaseOrderInvoices'
CREATE TABLE [gn].[GNBillingPurchaseOrderInvoices] (
    [Id] uniqueidentifier  NOT NULL,
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
    [GNBillingPaymentId] uniqueidentifier  NOT NULL,
    [GNBillingInvoiceId] uniqueidentifier  NOT NULL,
    [TotalApplied] float  NOT NULL
);
GO

-- Creating table 'GNBillingPaymentMethods'
CREATE TABLE [gn].[GNBillingPaymentMethods] (
    [Id] uniqueidentifier  NOT NULL,
    [GNAccountId] uniqueidentifier  NOT NULL,
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

-- Creating primary key on [Id] in table 'GNBillingPaymentMethods'
ALTER TABLE [gn].[GNBillingPaymentMethods]
ADD CONSTRAINT [PK_GNBillingPaymentMethods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating table 'GNBillingPurchaseOrderGNBillingAccount'
CREATE TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount] (
    [GNBillingPurchaseOrders_Id] uniqueidentifier  NOT NULL,
    [GNBillingAccounts_Id] uniqueidentifier  NOT NULL
);
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

-- Creating primary key on [Id] in table 'GNBillingPurchaseOrders'
ALTER TABLE [gn].[GNBillingPurchaseOrders]
ADD CONSTRAINT [PK_GNBillingPurchaseOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingInvoiceDetails'
ALTER TABLE [gn].[GNBillingInvoiceDetails]
ADD CONSTRAINT [PK_GNBillingInvoiceDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [PK_GNBillingTransactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingPayments'
ALTER TABLE [gn].[GNBillingPayments]
ADD CONSTRAINT [PK_GNBillingPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GNBillingAccountPurchaseOrderHistories'
ALTER TABLE [gn].[GNBillingAccountPurchaseOrderHistories]
ADD CONSTRAINT [PK_GNBillingAccountPurchaseOrderHistories]
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

-- Creating primary key on [GNBillingPurchaseOrders_Id], [GNBillingAccounts_Id] in table 'GNBillingPurchaseOrderGNBillingAccount'
ALTER TABLE [gn].[GNBillingPurchaseOrderGNBillingAccount]
ADD CONSTRAINT [PK_GNBillingPurchaseOrderGNBillingAccount]
    PRIMARY KEY CLUSTERED ([GNBillingPurchaseOrders_Id], [GNBillingAccounts_Id] ASC);
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

-- Creating foreign key on [GNBillingContactId] in table 'GNBillingAccounts'
ALTER TABLE [gn].[GNBillingAccounts]
ADD CONSTRAINT [FK_GNBillingAccountGNContact]
    FOREIGN KEY ([GNBillingContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingAccountGNContact'
CREATE INDEX [IX_FK_GNBillingAccountGNContact]
ON [gn].[GNBillingAccounts]
    ([GNBillingContactId]);
GO

-- Creating foreign key on [GNMailingContactId] in table 'GNBillingAccounts'
ALTER TABLE [gn].[GNBillingAccounts]
ADD CONSTRAINT [FK_GNBillingAccountGNContact1]
    FOREIGN KEY ([GNMailingContactId])
    REFERENCES [gn].[GNContacts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingAccountGNContact1'
CREATE INDEX [IX_FK_GNBillingAccountGNContact1]
ON [gn].[GNBillingAccounts]
    ([GNMailingContactId]);
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

-- Creating foreign key on [GNBillingInvoiceDetailId] in table 'GNBillingTransactions'
ALTER TABLE [gn].[GNBillingTransactions]
ADD CONSTRAINT [FK_GNBillingTransactionGNBillingInvoiceDetail]
    FOREIGN KEY ([GNBillingInvoiceDetailId])
    REFERENCES [gn].[GNBillingInvoiceDetails]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingTransactionGNBillingInvoiceDetail'
CREATE INDEX [IX_FK_GNBillingTransactionGNBillingInvoiceDetail]
ON [gn].[GNBillingTransactions]
    ([GNBillingInvoiceDetailId]);
GO

-- Creating foreign key on [GNBillingPaymentId] in table 'GNBillingPaymentInvoices'
ALTER TABLE [gn].[GNBillingPaymentInvoices]
ADD CONSTRAINT [FK_GNBillingPaymentGNBillingPaymentInvoice]
    FOREIGN KEY ([GNBillingPaymentId])
    REFERENCES [gn].[GNBillingPayments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPaymentGNBillingPaymentInvoice'
CREATE INDEX [IX_FK_GNBillingPaymentGNBillingPaymentInvoice]
ON [gn].[GNBillingPaymentInvoices]
    ([GNBillingPaymentId]);
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

-- Creating foreign key on [GNBillingPaymentMethodId] in table 'GNBillingPayments'
ALTER TABLE [gn].[GNBillingPayments]
ADD CONSTRAINT [FK_GNBillingPaymentGNBillingPaymentMethod]
    FOREIGN KEY ([GNBillingPaymentMethodId])
    REFERENCES [gn].[GNBillingPaymentMethods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPaymentGNBillingPaymentMethod'
CREATE INDEX [IX_FK_GNBillingPaymentGNBillingPaymentMethod]
ON [gn].[GNBillingPayments]
    ([GNBillingPaymentMethodId]);
GO

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

-- Creating primary key on [Id] in table 'GNBillingPaymentMethods'
ALTER TABLE [gn].[GNBillingPaymentMethods]
ADD CONSTRAINT [PK_GNBillingPaymentMethods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

 Creating foreign key on [GNBillingAccountId] in table 'GNBillingPaymentMethods'
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

-- Creating foreign key on [GNBillingPaymentMethodId] in table 'GNBillingPayments'
ALTER TABLE [gn].[GNBillingPayments]
ADD CONSTRAINT [FK_GNBillingPaymentGNBillingPaymentMethod]
    FOREIGN KEY ([GNBillingPaymentMethodId])
    REFERENCES [gn].[GNBillingPaymentMethods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GNBillingPaymentGNBillingPaymentMethod'
CREATE INDEX [IX_FK_GNBillingPaymentGNBillingPaymentMethod]
ON [gn].[GNBillingPayments]
    ([GNBillingPaymentMethodId]);
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

    
-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

