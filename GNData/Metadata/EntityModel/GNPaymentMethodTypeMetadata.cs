using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNPaymentMethodTypeMetadata))]
    public partial class GNPaymentMethodType : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public sealed class Types
        {
            private readonly string code;
            private readonly string name;

            public static readonly Types CREDIT_CARD = new Types("CREDIT_CARD", "Credit Card");
            public static readonly Types CHECK = new Types("CHECK", "Check");
            public static readonly Types TRANSFER = new Types("TRANSFER", "Direct Transfer");

            private Types(string code, string name)
            {
                this.code = code;
                this.name = name;
            }

            public string GetCode()
            {
                return code;
            }

            public string GetName()
            {
                return name;
            }
        }
    }

    public partial class GNPaymentMethodTypeMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }

        public virtual ICollection<GNPaymentMethod> PaymentMethods { get; set; }
    }
}
