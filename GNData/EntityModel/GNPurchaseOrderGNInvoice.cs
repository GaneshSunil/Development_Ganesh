
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
    
public partial class GNPurchaseOrderGNInvoice
{

    public System.Guid PurchaseOrders_Id { get; set; }

    public System.Guid Invoices_Id { get; set; }

    public double TotalApplied { get; set; }



    public virtual GNPurchaseOrder PurchaseOrder { get; set; }

    public virtual GNInvoice Invoice { get; set; }

}

}
