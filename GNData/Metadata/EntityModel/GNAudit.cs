using GenomeNext.Data.IdentityModel;
using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.Metadata.Audit
{
    public partial class GNAudit //: GenomeNext.Data.Metadata.Audit.AuditModel
    {
        public string Id {get; set; }        
        public string Action {get; set; }
        public Guid OrganizationId { get; set; }
        public String OrganizationName { get; set; } //redundancy for the excel file
        public Guid ActorId {get; set; }
        public string ActorEmail { get; set; }
        public string ActorName { get; set; } //redundancy for the excel file
        public GNContact Actor { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string IP { get; set; }
        public DateTime Timestamp { get; set; }
        public long TimestampNumeric { get; set; }

        public GNOrganization ActorOrganization { get; set; }
        public GNContact Contact { get; set; }
        public GNCloudFile CloudFile { get; set; }
        public AspNetUser User { get; set; }
        public GNOrganization Organization { get; set; }
        public GNTeam Team { get; set; }
        public GNProject Project { get; set; }
        public GNSample Sample { get; set; }
        public GNAnalysisRequest Analysis { get; set; }
        public GNAccount Account { get; set; }
        public GNInvoice Invoice { get; set; }
        public GNInvoiceDetail InvoiceDetail { get; set; }
        public GNPayment Payment { get; set; }
        public GNProduct Product { get; set; }
        public GNPurchaseOrder PurchaseOrder { get; set; }
        public GNSampleRelationship Pedigree { get; set; }
        public GNSettingsTemplate SettingsTemplate { get; set; }
        public GNSettingsTemplateConfig SettingsTemplateConfig { get; set; }
    }

    public partial class GNAuditMetadata //: GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {
        [Display(Name = "Entity Type")]
        public string EntityType { get; set; }
        [Display(Name = "Actor & IP")]
        public string Actor { get; set; }
        [Display(Name = "Date & Event ID")]
        public string Id { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
