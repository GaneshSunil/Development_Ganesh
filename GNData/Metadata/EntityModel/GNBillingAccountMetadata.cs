using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNBillingAccountMetadata))]
    public partial class GNBillingAccount : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        [DataType(DataType.Currency)]
        public double AvailableCreditsDisplay { get { return Math.Round(this.AvailableCredits, 2); } }


        public Guid MailingContactId { get; set; }
        public Guid BillingContactId { get; set; }


        public sealed class BillingModeType
        {
            private readonly string code;
            private readonly string name;

            public static readonly BillingModeType INVOICE = new BillingModeType("I", "Invoice");

            private BillingModeType(string code, string name)
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

    public partial class GNBillingAccountMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {

        public double MaxBalanceAllowed { get; set; }
        [Display(Name = "Is Valid Billing Agreement Required?")]
        public bool ValidBillingAgreementRequired { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDateTime { get; set; }
        [Display(Name = "Account Type")]
        public int GNAccountTypeId { get; set; }

        [Display(Name = "Available Credits")]
        [DataType(DataType.Currency)]
        public virtual double AvailableCreditsDisplay { get; set; }

        [Display(Name = "Account Owner")]
        public virtual GNContact AccountOwner { get; set; }
        [Display(Name = "Mailing Contact")]
        public virtual GNContact MailingContact { get; set; }
        [Display(Name = "Billing Contact")]
        public virtual GNContact BillingContact { get; set; }
        [Display(Name = "Payment Methods")]
        public virtual ICollection<GNPaymentMethod> PaymentMethods { get; set; }
        public virtual ICollection<GNInvoice> Invoices { get; set; }
        [Display(Name = "Organization")]
        public virtual GNOrganization Organization { get; set; }
        [Display(Name = "Product Subscriptions")]
        public virtual ICollection<GNAccountProductSubscription> AccountProductSubscriptions { get; set; }
        [Display(Name = "Account Type")]
        public virtual GNAccountType AccountType { get; set; }
        [Display(Name = "Purchase Orders")]
        public virtual ICollection<GNPurchaseOrder> PurchaseOrders { get; set; }
        [Display(Name = "Account Thresholds")]
        public virtual ICollection<GNAccountThreshold> AccountThresholds { get; set; }
    }
}
