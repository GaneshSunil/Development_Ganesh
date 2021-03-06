
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
    
public partial class GNTemplate
{

    public GNTemplate()
    {

        this.GNAnalysisRequestGNTemplates = new HashSet<GNAnalysisRequestGNTemplate>();

        this.GNTemplateGenes = new HashSet<GNTemplateGene>();

    }


    public int Id { get; set; }

    public string Code { get; set; }

    public int Version { get; set; }

    public string Name { get; set; }

    public System.Guid GNOrganizationId { get; set; }

    public System.Guid CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual ICollection<GNAnalysisRequestGNTemplate> GNAnalysisRequestGNTemplates { get; set; }

    public virtual ICollection<GNTemplateGene> GNTemplateGenes { get; set; }

    public virtual GNOrganization GNOrganization { get; set; }

}

}
