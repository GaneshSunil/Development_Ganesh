alter table gn.GNInvoices add [InvoiceCycle] nvarchar(6)  NOT NULL;

update gn.GNInvoices set InvoiceCycle = LEFT(CONVERT(varchar, InvoiceStartDate,112),6) where InvoiceCycle is null

