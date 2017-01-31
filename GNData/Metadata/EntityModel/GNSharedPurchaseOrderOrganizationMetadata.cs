using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNSharedPurchaseOrderOrganization))]
    public partial class GNSharedPurchaseOrderOrganization : GenomeNext.Data.Metadata.Audit.AuditModel
    {
    }
                         
    public partial class GNSharedPurchaseOrderOrganizationMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Required]
        [Display(Name="Purchase Order")]
        public Guid GNPurchaseOrderId { get; set; }

        [Required]
        [Display(Name = "Invoice")]
        public Guid GNInvoiceId { get; set; }

        [Required]
        [Display(Name = "Amount Billed")]
        public double AmountBilledOnInvoice { get; set; }
    }
}
