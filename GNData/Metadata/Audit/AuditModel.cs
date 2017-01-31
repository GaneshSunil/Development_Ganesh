using GenomeNext.Data.EntityModel;
using GenomeNext.Data.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.Metadata.Audit
{
    [MetadataType(typeof(AuditModelMetadata))]
    public abstract partial class AuditModel : SecureModel
    {
        public Nullable<System.Guid> CreatedBy { get; set; }
        public GNContact CreatedByContact { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public Nullable<System.DateTime> CreateDateTimeOnTimeZone 
        {
            get {
                return this.CreateDateTime;// +TimeSpan.Parse("04:00"); //remove hardcoded value
            }
        }


    }

    public abstract partial class AuditModelMetadata
    {
        [Display(Name = "Created By")]
        public Nullable<System.Guid> CreatedBy { get; set; }

        [Display(Name = "Created By")]
        public GNContact CreatedByContact { get; set; }

        [Display(Name = "Create Date/Time")]
        public Nullable<System.DateTime> CreateDateTime { get; set; }
    }
}
