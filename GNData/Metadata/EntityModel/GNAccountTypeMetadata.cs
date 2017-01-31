using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAccountTypeMetadata))]
    public partial class GNAccountType : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNAccountTypeMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual ICollection<GNAccount> Accounts { get; set; }
        public virtual ICollection<GNProduct> Products { get; set; }
    }
}
