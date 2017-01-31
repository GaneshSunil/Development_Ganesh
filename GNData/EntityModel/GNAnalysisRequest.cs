
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
    
public partial class GNAnalysisRequest
{

    public GNAnalysisRequest()
    {

        this.LatestRequestPhase = "SECONDARY";

        this.AnalysisStatus = new HashSet<GNAnalysisStatus>();

        this.GNAnalysisRequestGNSamples = new HashSet<GNAnalysisRequestGNSample>();

        this.GNAnalysisRequestGNTemplates = new HashSet<GNAnalysisRequestGNTemplate>();

        this.GNTransactions = new HashSet<GNTransaction>();

        this.GNAnalysisRequestGroups = new HashSet<GNAnalysisRequestGroup>();

        this.GNAnalysisRequestGroupComparisons = new HashSet<GNAnalysisRequestGroupComparison>();

        this.GNBillingTransactions = new HashSet<GNBillingTransaction>();

    }


    public System.Guid Id { get; set; }

    public string Description { get; set; }

    public Nullable<double> RequestProgress { get; set; }

    public string LatestRequestPhase { get; set; }

    public Nullable<System.DateTime> RequestDateTime { get; set; }

    public System.Guid GNProjectId { get; set; }

    public string AWSRegionSystemName { get; set; }

    public string GNAnalysisRequestTypeCode { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }

    public string GNAnalysisAdaptorCode { get; set; }



    public virtual GNAnalysisType AnalysisType { get; set; }

    public virtual GNProject Project { get; set; }

    public virtual AWSRegion AWSRegion { get; set; }

    public virtual ICollection<GNAnalysisStatus> AnalysisStatus { get; set; }

    public virtual GNAnalysisResult AnalysisResult { get; set; }

    public virtual ICollection<GNAnalysisRequestGNSample> GNAnalysisRequestGNSamples { get; set; }

    public virtual ICollection<GNAnalysisRequestGNTemplate> GNAnalysisRequestGNTemplates { get; set; }

    public virtual GNAnalysisRequestType GNAnalysisRequestType { get; set; }

    public virtual ICollection<GNTransaction> GNTransactions { get; set; }

    public virtual ICollection<GNAnalysisRequestGroup> GNAnalysisRequestGroups { get; set; }

    public virtual ICollection<GNAnalysisRequestGroupComparison> GNAnalysisRequestGroupComparisons { get; set; }

    public virtual GNAnalysisAdaptor GNAnalysisAdaptor { get; set; }

    public virtual ICollection<GNBillingTransaction> GNBillingTransactions { get; set; }

}

}
