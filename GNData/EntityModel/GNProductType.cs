
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
    
public partial class GNProductType
{

    public GNProductType()
    {

        this.Products = new HashSet<GNProduct>();

    }


    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool IsEligibleForDiscount { get; set; }

    public bool CanSubscribe { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual ICollection<GNProduct> Products { get; set; }

}

}
