
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
    
public partial class GNReplicate
{

    public GNReplicate()
    {

        this.GNSamples = new HashSet<GNSample>();

    }


    public string Code { get; set; }

    public string Name { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }

    public System.Guid Id { get; set; }



    public virtual ICollection<GNSample> GNSamples { get; set; }

}

}
