IF OBJECT_ID(N'[gn].[FK_GNPurchaseOrderGNInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNInvoices] DROP CONSTRAINT [FK_GNPurchaseOrderGNInvoice];
GO
IF OBJECT_ID(N'[gn].[FK_GNPurchaseOrderGNInvoice_GNPurchaseOrder]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPurchaseOrderGNInvoice] DROP CONSTRAINT [FK_GNPurchaseOrderGNInvoice_GNPurchaseOrder];
GO
IF OBJECT_ID(N'[gn].[FK_GNPurchaseOrderGNInvoice_GNInvoice]', 'F') IS NOT NULL
    ALTER TABLE [gn].[GNPurchaseOrderGNInvoice] DROP CONSTRAINT [FK_GNPurchaseOrderGNInvoice_GNInvoice];
GO

DROP INDEX [IX_FK_GNPurchaseOrderGNInvoice]
    ON [gn].[GNInvoices];
GO

ALTER TABLE [gn].[GNInvoices] DROP COLUMN [GNPurchaseOrderId];
GO

IF OBJECT_ID(N'[gn].[GNPurchaseOrderGNInvoice]', 'U') IS NOT NULL
    DROP TABLE [gn].[GNPurchaseOrderGNInvoice];
GO

-- Creating table 'GNPurchaseOrderGNInvoice'
CREATE TABLE [gn].[GNPurchaseOrderGNInvoice] (
    [PurchaseOrders_Id] uniqueidentifier  NOT NULL,
    [Invoices_Id] uniqueidentifier  NOT NULL,
    [TotalApplied] float  NOT NULL
);
GO

-- Creating primary key on [PurchaseOrders_Id], [Invoices_Id] in table 'GNPurchaseOrderGNInvoice'
ALTER TABLE [gn].[GNPurchaseOrderGNInvoice]
ADD CONSTRAINT [PK_GNPurchaseOrderGNInvoice]
    PRIMARY KEY CLUSTERED ([PurchaseOrders_Id], [Invoices_Id] ASC);
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

