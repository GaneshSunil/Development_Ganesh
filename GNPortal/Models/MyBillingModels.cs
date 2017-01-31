using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GenomeNext.Portal.Models
{
    public class MyBillingOverviewModel
    {
        public GNAccount MyAccount { get; set; }
        public GNInvoice MyCurrentInvoice { get; set; }
        public GNInvoice MyLastInvoice { get; set; }

        [DataType(DataType.Currency)]
        public double MaxApprovedSpend { get; set; }

        [DataType(DataType.Currency)]
        public double CurrentMonthSpend { get; set; }

        [DataType(DataType.Currency)]
        public double OtherDebitsCredits { get; set; }

        [DataType(DataType.Currency)]
        public double MyRemainingBudget { get; set; }

        [DataType(DataType.Currency)]
        public double MyBalancePastDue { get; set; }

        public string StorageUnits { get; set; }
        [DataType(DataType.Currency)]
        public double StoragePricePerUnit { get; set; }
        public double MyStorageUsed { get; set; }
        [DataType(DataType.Currency)]
        public double MyStorageTotalCurrentCost  { get; set; }
    }
    public class MyBillingBuyCreditsModel
    {
        public GNAccount MyAccount { get; set; }
        public IEnumerable<GNProduct> MyProducts { get; set; }
        public double MinPurchaseAmount { get; set; }
    }

    public class MyBillingPaymentMethodsModel
    {
        public GNAccount MyAccount { get; set; }
    }

    public class MyBillingPaymentsModel
    {
        public GNAccount MyAccount { get; set; }
        public IList<GNPayment> MyPayments { get; set; }
    }

    public class MyBillingBillsModel
    {
        public GNAccount MyAccount { get; set; }
    }

    public class MyBillingBillDetailModel
    {
        public GNAccount MyAccount { get; set; }
        public GNInvoice MyBill { get; set; }
    }

    public class MyBillingBillDetailItemDetailModel
    {
        public GNAccount MyAccount { get; set; }
        public GNInvoiceDetail MyBillDetail { get; set; }
    }
    
}