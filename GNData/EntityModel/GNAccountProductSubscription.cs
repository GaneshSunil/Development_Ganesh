
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
    
public partial class GNAccountProductSubscription
{

    public GNAccountProductSubscription()
    {

        this.CurrentValueUsed = 0D;

    }


    public int Id { get; set; }

    public System.Guid GNAccountId { get; set; }

    public System.Guid GNProductId { get; set; }

    public double CurrentValueUsed { get; set; }

    public System.DateTime StartDate { get; set; }

    public System.DateTime EndDate { get; set; }

    public int SubscribeFrequency { get; set; }

    public bool IsActive { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNAccount Account { get; set; }

    public virtual GNProduct Product { get; set; }

}

}
