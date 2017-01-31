
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
    
public partial class GNBillingPaymentMethod
{

    public System.Guid Id { get; set; }

    public System.Guid GNBillingAccountId { get; set; }

    public int GNPaymentMethodTypeId { get; set; }

    public string Description { get; set; }

    public string LastFourDigits { get; set; }

    public string PCITokenId { get; set; }

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public bool UsedForRecurrentPayments { get; set; }

    public System.DateTime ExpirationDate { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNBillingAccount GNBillingAccount { get; set; }

    public virtual GNPaymentMethodType GNPaymentMethodType { get; set; }

}

}
