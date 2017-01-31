
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace GenomeNext.Data.EntityModel
{

using System;
    using System.Collections.Generic;
    
public partial class GNTransaction
{

    public System.Guid Id { get; set; }

    public System.Guid GNInvoiceDetailId { get; set; }

    public int GNTransactionTypeId { get; set; }

    public Nullable<System.Guid> GNProductId { get; set; }

    public string Description { get; set; }

    public string ExternalTxnId { get; set; }

    public double ValueUsed { get; set; }

    public string ValueUnits { get; set; }

    public double TotalCost { get; set; }

    public double TotalPrice { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNInvoiceDetail InvoiceDetail { get; set; }

    public virtual GNTransactionType TransactionType { get; set; }

    public virtual GNProduct Product { get; set; }

    public virtual GNAnalysisRequest GNAnalysisRequest { get; set; }

    public virtual GNProject GNProject { get; set; }

    public virtual GNTeam GNTeam { get; set; }

}

}
