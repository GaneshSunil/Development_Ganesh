using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNInviteCodeMetadata))]
    public partial class GNInviteCode : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }

    public partial class GNInviteCodeMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Required]
        [Display(Name="Invite Code")]
        public string InviteCode { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Free Credit Allowance")]
        public double FreeCreditAllowance { get; set; }

        [Display(Name = "Used Count")]
        public int UseCount { get; set; }

        [Display(Name = "Max Allowed")]
        public Nullable<int> UseMaxAllowed { get; set; }

        [Display(Name = "Expire Date")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> ExpireDate { get; set; }

        [Display(Name = "Created By")]
        public Nullable<System.Guid> CreatedBy { get; set; }

        [Display(Name = "Create Date/Time")]
        public Nullable<System.DateTime> CreateDateTime { get; set; }
    }
}
