
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
    
public partial class GNAnalysisStatus
{

    public GNAnalysisStatus()
    {

        this.AnalysisPhase = "SECONDARY";

    }


    public int Id { get; set; }

    public string Status { get; set; }

    public string Message { get; set; }

    public int PercentComplete { get; set; }

    public bool IsError { get; set; }

    public Nullable<System.Guid> GNAnalysisResultId { get; set; }

    public Nullable<System.Guid> GNAnalysisRequestId { get; set; }

    public string AnalysisPhase { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNAnalysisResult GNAnalysisResult { get; set; }

    public virtual GNAnalysisRequest GNAnalysisRequest { get; set; }

}

}
