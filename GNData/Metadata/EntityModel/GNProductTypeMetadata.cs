using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNProductTypeMetadata))]
    public partial class GNProductType : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNProductTypeMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Is Eligible For Discount")]
        public bool IsEligibleForDiscount { get; set; }
        [Display(Name = "Can Subscribe?")]
        public bool CanSubscribe { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual ICollection<GNProduct> Products { get; set; }
    }
}
