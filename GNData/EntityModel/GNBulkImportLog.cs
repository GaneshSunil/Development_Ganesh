
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
    
public partial class GNBulkImportLog
{

    public System.Guid Id { get; set; }

    public string RecordRowId { get; set; }

    public bool IsError { get; set; }

    public bool IsDuplicate { get; set; }

    public string Message { get; set; }

    public System.Guid GNBulkImportStatusId { get; set; }

    public Nullable<System.Guid> CreatedBy { get; set; }

    public Nullable<System.DateTime> CreateDateTime { get; set; }



    public virtual GNBulkImportStatus BulkImportStatus { get; set; }

}

}
