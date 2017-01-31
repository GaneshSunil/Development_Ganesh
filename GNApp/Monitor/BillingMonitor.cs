using GenomeNext.App.Console;
using GenomeNext.Billing;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenomeNext.App.Monitor
{
    class BillingMonitor : IConsoleApp
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int DAY_TO_PROCESS_STORAGE_CARRYOVER_FEES = 1;
        private const int DAY_TO_CREATE_INVOICES = 1;        

        public void Init()
        {
            InitServices();
        }

        public void Run()
        {
            Monitor();
        }

        private void InitServices()
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            LogUtil.Info(logger, "BillingMonitor.InitServices()...");
            System.Console.WriteLine("\nBillingMonitor.InitServices()...");
        }

        private void Monitor()
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            LogUtil.Info(logger, "BillingListener.Monitor()...");
            System.Console.WriteLine("\nBillingListener.Monitor()...");

            try
            {
                var t = Task.Run(async delegate
                {
                    await CreateNewInvoices();
                    return await ProcessStorageCarryOverFees();
                });

                int result = t.Result;

                LogUtil.Info(logger, "result = " + result);
                System.Console.WriteLine("\nresult = " + result);
            }
            catch (Exception ex)
            {
                LogUtil.Warn(logger, ex.Message, ex);
                System.Console.WriteLine(ex.Message);
            }
        }

        private async Task<int> ProcessStorageCarryOverFees()
        {
            int result = 1;

            if (DateTime.Now.Day == DAY_TO_PROCESS_STORAGE_CARRYOVER_FEES)
            {
                LogUtil.Info(logger, "ProcessStorageCarryOverFees()...");
                System.Console.WriteLine("\nProcessStorageCarryOverFees()...");

                var db = new GNEntityModelContainer();
                InvoiceService invoiceService = new InvoiceService(db);
                InvoiceDetailService invoiceDetailService = new InvoiceDetailService(db);
                TransactionService transactionService = new TransactionService(db);
                AccountService orgAccountService = new AccountService(db);

                foreach (var orgAccount in await orgAccountService.FindAll())
                {
                    try
                    {
                        LogUtil.Info(logger, "Account = " + orgAccount.Organization.Name);
                        System.Console.WriteLine("\nAccount = " + orgAccount.Organization.Name);

                        //get user contact
                        GNContact userContact = orgAccount.AccountOwner;
                        if (userContact == null)
                        {
                            userContact = db.GNContacts.Where(c => c.GNOrganizationId == orgAccount.Organization.Id).FirstOrDefault();
                        }

                        if(userContact != null)
                        {
                            //get last month invoice
                            GNInvoice lastMonthInvoice = invoiceService.GetInvoiceForLastMonth(userContact);

                            if (lastMonthInvoice != null)
                            {
                                string txnKey = "STORAGE_S3_CARRYOVER";

                                //get invoice to update
                                GNInvoice invoiceToUpdate = null;
                                if (lastMonthInvoice.Status != GNInvoice.InvoiceStatus.PAID.ToString()
                                    && lastMonthInvoice.Status != GNInvoice.InvoiceStatus.VOID.ToString())
                                {
                                    invoiceToUpdate = lastMonthInvoice;
                                }
                                else
                                {
                                    invoiceToUpdate = await invoiceService.GetInvoiceForCurrentMonth(orgAccount.AccountOwner);
                                }

                                if (invoiceToUpdate != null)
                                {
                                    //get storage carryover detail
                                    GNInvoiceDetail storageCarryOverInvoiceDetail =
                                        db.GNInvoiceDetails
                                        .Where(invd => (invd.GNInvoiceId == invoiceToUpdate.Id && invd.Description == txnKey))
                                        .FirstOrDefault();

                                    //add invoice detail, if missing
                                    if (storageCarryOverInvoiceDetail == null)
                                    {
                                        storageCarryOverInvoiceDetail =
                                            db.GNInvoiceDetails.Add(new GNInvoiceDetail
                                            {
                                                Id = Guid.NewGuid(),
                                                Description = txnKey,
                                                GNInvoiceId = invoiceToUpdate.Id,
                                                Quantity = 0.0,
                                                SubTotal = 0.0,
                                                DiscountAmount = orgAccount.DefaultDiscountAmount,
                                                DiscountType = orgAccount.DefaultDiscountType,
                                                Total = 0.0,
                                                UnitCost = 0.0,
                                                UnitPrice = 0.0,
                                                CreateDateTime = DateTime.Now,
                                                CreatedBy = orgAccount.AccountOwner.Id
                                            });

                                        await db.SaveChangesAsync();

                                        storageCarryOverInvoiceDetail = db.GNInvoiceDetails.Find(storageCarryOverInvoiceDetail.Id);
                                    }

                                    if (storageCarryOverInvoiceDetail != null)
                                    {
                                        //get txn count
                                        int txnCount =
                                            db.GNTransactions.Count(t =>
                                                (t.GNInvoiceDetailId == storageCarryOverInvoiceDetail.Id
                                                && t.TransactionType.Name == txnKey));

                                        if (txnCount == 0)
                                        {
                                            //get storage balance for month
                                            //all uploads minus all downloads up until end of last month
                                            var storageUsed = orgAccountService.CalcStorageUsed(orgAccount.Id, lastMonthInvoice.InvoiceEndDate.AddDays(1));

                                            //get storage carryover product
                                            GNProduct storageCarryOverProduct = db.GNProducts.Where(p => p.Name == txnKey).FirstOrDefault();

                                            //calc storage carryover cost
                                            var totalCarryOverCost = storageUsed * storageCarryOverProduct.Price;

                                            //add storage cost transaction
                                            string txnTypeKey = txnKey;
                                            string description = storageUsed + "GB";
                                            double valueUsed = storageUsed;
                                            string valueUnits = "GB";

                                            int updateResult = 0;

                                            GNTransaction txn =
                                                await transactionService.CreateTransaction(
                                                    userContact, txnTypeKey, description, valueUsed, valueUnits,
                                                    targetInvoice: invoiceToUpdate);

                                            if (txn != null)
                                            {
                                                updateResult = 1;
                                            }
                                            //update invoice totals
                                            //int updateResult = await invoiceDetailService.UpdateInvoiceDetailTotals(
                                            //    invDetailToUpdate.Id, invDetailToUpdate.GNInvoiceId, invDetailToUpdate.Invoice.GNAccountId);

                                            LogUtil.Info(logger, "Update result = " + updateResult);
                                            System.Console.WriteLine("\nUpdate result = " + updateResult);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        result = 0;
                        LogUtil.Warn(logger, ex.Message, ex);
                        System.Console.WriteLine(ex.Message);
                    }
                }
            }

            return result;
        }



        private async Task<int> CreateNewInvoices()
        {
            int result = 1;

            if (DateTime.Now.Day == DAY_TO_CREATE_INVOICES)
            {
                LogUtil.Info(logger, "CreateNewInvoices()...");
                System.Console.WriteLine("\nCreateNewInvoices()...");

                var db = new GNEntityModelContainer();
                InvoiceService invoiceService = new InvoiceService(db);
                InvoiceDetailService invoiceDetailService = new InvoiceDetailService(db);
                TransactionService transactionService = new TransactionService(db);
                AccountService orgAccountService = new AccountService(db);

                foreach (var orgAccount in await orgAccountService.FindAll())
                {
                    try
                    {
                        LogUtil.Info(logger, "Account = " + orgAccount.Organization.Name);
                        System.Console.WriteLine("\nAccount = " + orgAccount.Organization.Name);

                        //get user contact
                        GNContact userContact = orgAccount.AccountOwner;
                        if (userContact == null)
                        {
                            userContact = db.GNContacts.Where(c => c.GNOrganizationId == orgAccount.Organization.Id).FirstOrDefault();
                        }

                        if (userContact != null)
                        {
                            string thisCycle = String.Format("{0:yyyyMM}", DateTime.Now);
                            GNInvoice newInvoiceMonth = invoiceService.GetInvoiceForDateTime(userContact, thisCycle);

                            if (newInvoiceMonth == null)
                            {
                                newInvoiceMonth = await invoiceService.CreateInvoiceForCurrentMonth(userContact, orgAccount);
                            }

                            System.Console.WriteLine("\nInvoice for Cycle " + thisCycle + " for Organization " + orgAccount.Organization.Name + " is " + newInvoiceMonth.Id);
                            LogUtil.Info(logger, "Invoice for Cycle " + thisCycle + " for Organization " + orgAccount.Organization.Name + " is " + newInvoiceMonth.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        result = 0;
                        LogUtil.Warn(logger, ex.Message, ex);
                        System.Console.WriteLine(ex.Message);
                    }
                }
            }

            return result;
        }

    }
}
