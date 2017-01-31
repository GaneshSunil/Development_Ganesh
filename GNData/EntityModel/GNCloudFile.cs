
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
    
public partial class GNCloudFile
{

    public System.Guid Id { get; set; }

    public string FileURL { get; set; }

    public string Volume { get; set; }

    public string FileName { get; set; }

    public string FolderPath { get; set; }

    public long FileSize { get; set; }

    public string Description { get; set; }

    public int GNCloudFileCategoryId { get; set; }

    public string AWSRegionSystemName { get; set; }

    public Nullable<bool> QcStatsAvailable { get; set; }

    public string QcStatsReportLocation { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNCloudFileCategory CloudFileCategory { get; set; }

    public virtual AWSRegion AWSRegion { get; set; }

}

}
