
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
    
public partial class GNBillingPurchaseOrderInvoice
{

    public string Id { get; set; }

    public System.Guid GNBillingPurchaseOrderId { get; set; }

    public System.Guid GNBillingInvoiceId { get; set; }

    public double TotalApplied { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNBillingInvoice GNBillingInvoice { get; set; }

    public virtual GNBillingPurchaseOrder GNBillingPurchaseOrder { get; set; }

}

}
