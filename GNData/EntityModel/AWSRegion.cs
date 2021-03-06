
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
    
public partial class AWSRegion
{

    public AWSRegion()
    {

        this.AWSConfigs = new HashSet<AWSConfig>();

        this.GNCloudFiles = new HashSet<GNCloudFile>();

        this.GNAnalysisRequests = new HashSet<GNAnalysisRequest>();

        this.GNAnalysisResults = new HashSet<GNAnalysisResult>();

    }


    public string AWSRegionSystemName { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual ICollection<AWSConfig> AWSConfigs { get; set; }

    public virtual ICollection<GNCloudFile> GNCloudFiles { get; set; }

    public virtual ICollection<GNAnalysisRequest> GNAnalysisRequests { get; set; }

    public virtual ICollection<GNAnalysisResult> GNAnalysisResults { get; set; }

}

}
