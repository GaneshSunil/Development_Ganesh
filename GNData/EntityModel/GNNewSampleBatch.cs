
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
    
public partial class GNNewSampleBatch
{

    public GNNewSampleBatch()
    {

        this.Qualifier = "\'DNA\'";

        this.GNNewSampleBatchStatus = new HashSet<GNNewSampleBatchStatus>();

        this.GNNewSampleBatchSamples = new HashSet<GNNewSampleBatchSamples>();

    }


    public System.Guid Id { get; set; }

    public string BatchId { get; set; }

    public System.Guid GNSequencerJobId { get; set; }

    public string Repository { get; set; }

    public string Project { get; set; }

    public string Type { get; set; }

    public string Qualifier { get; set; }

    public string ReadType { get; set; }

    public bool CreateAnalysisPerSample { get; set; }

    public bool AutoStartAnalysis { get; set; }

    public int TotalSamples { get; set; }

    public int TotalSamplesCompleted { get; set; }

    public int TotalNumberOfFastqFiles { get; set; }

    public int TotalNumberOfFastqFilesCompleted { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual ICollection<GNNewSampleBatchStatus> GNNewSampleBatchStatus { get; set; }

    public virtual ICollection<GNNewSampleBatchSamples> GNNewSampleBatchSamples { get; set; }

    public virtual GNSequencerJob GNSequencerJob { get; set; }

}

}
