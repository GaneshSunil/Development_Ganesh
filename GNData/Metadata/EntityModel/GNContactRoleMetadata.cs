using GenomeNext.Data.IdentityModel;
using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNContactRoleMetadata))]
    public partial class GNContactRole : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public AspNetRole AspNetRole { get; set; }
        
    }

    public class GNContactRoleMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public System.Guid GNContactId { get; set; }
        public string AspNetRoleId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual GNContact GNContact { get; set; }
    }
}
