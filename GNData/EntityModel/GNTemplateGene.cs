
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
    
public partial class GNTemplateGene
{

    public int Id { get; set; }

    public int GNTemplateId { get; set; }

    public string GeneCode { get; set; }

    public System.Guid CreatedBy { get; set; }

    public System.DateTime CreatedDateTime { get; set; }

    public long GNGeneId { get; set; }



    public virtual GNTemplate GNTemplate { get; set; }

    public virtual GNGene GNGene { get; set; }

}

}
