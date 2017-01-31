using GenomeNext.Data.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data.EntityModel
{
    [MetadataType(typeof(GNAccountMetadata))]
    public partial class GNAccount : GenomeNext.Data.Metadata.Audit.AuditModel
    {
        [DataType(DataType.Currency)]
        public double Balance { get { return TotalAmountOwed - TotalAmountPaid - TotalAppliedToPOs; } }

        [DataType(DataType.Currency)]
        public double AvailableCreditsDisplay { get { return Math.Round(this.AvailableCredits, 2); } }

        [DataType(DataType.Currency)]
        public double TotalAppliedToPOs { get; set; }

        public Guid AccountOwnerId { get; set; }
        public Guid MailingContactId { get; set; }
        public Guid BillingContactId { get; set; }

        public sealed class DiscountType
        {
            private readonly string code;
            private readonly string name;

            public static readonly DiscountType PERCENT = new DiscountType("P","Percent");
            public static readonly DiscountType FLAT = new DiscountType("F", "Flat");

            private DiscountType(string code, string name)
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

    public partial class GNAccountMetadata : GenomeNext.Data.Metadata.Audit.AuditModelMetadata
    {

        public System.Guid Id { get; set; }
        [Display(Name = "Total Amount Paid")]
        [DataType(DataType.Currency)]
        public double TotalAmountPaid { get; set; }
        [Display(Name = "Total Amount Owed")]
        [DataType(DataType.Currency)]
        public double TotalAmountOwed { get; set; }
        [Display(Name = "Default Discount Type")]
        public string DefaultDiscountType { get; set; }
        [Display(Name = "Default Discount Amount")]
        public double DefaultDiscountAmount { get; set; }
        [Display(Name = "Billing Mode")]
        public string BillingMode { get; set; }
        [Required]
        [Display(Name = "Max Balance Allowed")]
        [DataType(DataType.Currency)]
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
