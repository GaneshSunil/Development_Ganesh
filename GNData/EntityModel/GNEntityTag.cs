
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
    
public partial class GNEntityTag
{

    public long Id { get; set; }

    public System.Guid EntityId { get; set; }

    public int GNEntityTypeId { get; set; }

    public string Tag { get; set; }

    public System.DateTime TagDate { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNEntityType GNEntityType { get; set; }

}

}
