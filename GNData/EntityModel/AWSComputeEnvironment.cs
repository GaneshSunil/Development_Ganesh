
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
    
public partial class AWSComputeEnvironment
{

    public AWSComputeEnvironment()
    {

        this.MaxInstanceRequiredPerAnalysis = 0;

        this.IPAvailCount = 0;

        this.InstanceRunningCount = 0;

        this.InstancePendingCount = 0;

    }


    public string Id { get; set; }

    public string VPC { get; set; }

    public string Subnet { get; set; }

    public string AvailZone { get; set; }

    public int MaxInstanceRequiredPerAnalysis { get; set; }

    public int MaxInstanceExpectedCount { get; set; }

    public int IPAvailCount { get; set; }

    public int InstanceRunningCount { get; set; }

    public int InstancePendingCount { get; set; }

    public System.Guid AWSConfigId { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual AWSConfig AWSConfig { get; set; }

}

}
