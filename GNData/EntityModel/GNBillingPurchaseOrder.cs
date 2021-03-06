
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
    
public partial class GNBillingPurchaseOrder
{

    public GNBillingPurchaseOrder()
    {

        this.GNBillingAccounts = new HashSet<GNBillingAccount>();

        this.GNBillingPurchaseOrderInvoices = new HashSet<GNBillingPurchaseOrderInvoice>();

    }


    public System.Guid Id { get; set; }

    public string ExternalPONum { get; set; }

    public double TotalCredits { get; set; }

    public double Balance { get; set; }

    public System.DateTime StartDate { get; set; }

    public double ExpirationDate { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }



    public virtual ICollection<GNBillingAccount> GNBillingAccounts { get; set; }

    public virtual ICollection<GNBillingPurchaseOrderInvoice> GNBillingPurchaseOrderInvoices { get; set; }

}

}
