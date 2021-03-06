
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
    
public partial class GNAnalysisRequestGroup
{

    public GNAnalysisRequestGroup()
    {

        this.GNSamples = new HashSet<GNSample>();

        this.GNAnalysisRequestControlGroups = new HashSet<GNAnalysisRequestGroupComparison>();

        this.GNAnalysisRequestComparisonGroups = new HashSet<GNAnalysisRequestGroupComparison>();

    }


    public System.Guid Id { get; set; }

    public string Name { get; set; }

    public System.Guid GNAnalysisRequestId { get; set; }

    public System.Guid CreatedBy { get; set; }

    public System.DateTime CreateDateTime { get; set; }



    public virtual GNAnalysisRequest GNAnalysisRequest { get; set; }

    public virtual ICollection<GNSample> GNSamples { get; set; }

    public virtual ICollection<GNAnalysisRequestGroupComparison> GNAnalysisRequestControlGroups { get; set; }

    public virtual ICollection<GNAnalysisRequestGroupComparison> GNAnalysisRequestComparisonGroups { get; set; }

}

}
