
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
    
public partial class GNEntityAudit
{

    public long Id { get; set; }

    public string EntityId { get; set; }

    public int GNEntityTypeId { get; set; }

    public int GNEntityAuditTypeId { get; set; }

    public System.Guid GNContactId { get; set; }

    public string AuditDate { get; set; }

    public string ValueOld { get; set; }

    public string ValueNew { get; set; }



    public virtual GNEntityType GNEntityType { get; set; }

    public virtual GNEntityAuditType GNEntityAuditType { get; set; }

    public virtual GNContact GNContact { get; set; }

}

}
