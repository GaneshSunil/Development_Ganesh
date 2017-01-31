using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using GenomeNext.Portal.Models;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data.EntityModel;
using GenomeNext.App;
using GenomeNext.Utility;
using GenomeNext.Notification;
using GenomeNext.Billing;
using GenomeNext.Portal.Attributes;

using ExpertPdf.HtmlToPdf;


namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class AccountController : BaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string ENTITY = "ACCOUNT";

        public TransactionService transactionService { get; set; }
        public ContactService contactService { get; set; }

        public AccountController():base()
        {
            transactionService = new TransactionService(base.db);
            contactService = new ContactService(base.db, base.identityDB);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            auditResult = audit.LogEvent(Guid.Empty.ToString(), model.Email, Guid.Empty.ToString(), Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOGIN_ATTEMPT");

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    if (await UserManager.IsEmailConfirmedAsync(user.Id))
                    {
                        var aspNetUser = this.identityDB.AspNetUsers.Where(u => u.Id == user.Id).FirstOrDefault();

                        if (aspNetUser != null 
                            && this.db.GNContacts.Count(c => (c.AspNetUserId == user.Id)) != 0)
                        {
                            if(string.IsNullOrEmpty(aspNetUser.DefaultOrganizationId)
                                || aspNetUser.DefaultOrganizationId == Guid.Empty.ToString())
                            {
                                var firstContact = this.db.GNContacts.Where(c => c.AspNetUserId == user.Id).FirstOrDefault();

                                if (firstContact != null)
                                {
                                    aspNetUser.DefaultOrganizationId = firstContact.GNOrganizationId.ToString();
                                    aspNetUser.Password = ".";
                                    aspNetUser.PasswordConfirm = ".";
                                    this.identityDB.SaveChanges();
                                    aspNetUser = this.identityDB.AspNetUsers.Where(u => u.Id == user.Id).FirstOrDefault();
                                }
                            }

                            if (!string.IsNullOrEmpty(aspNetUser.DefaultOrganizationId)
                                && aspNetUser.DefaultOrganizationId != Guid.Empty.ToString())
                            {
                                GNContact userContact = db.GNContacts.Where(a => a.Email.Equals(user.Email) && a.GNOrganizationId.ToString().Equals(aspNetUser.DefaultOrganizationId)).FirstOrDefault();
                                bool result = await (new InvoiceService(db)).fetchCurrentInvoice(userContact);

                            //    auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "LOGIN_SUCCESSFUL");
                                await SignInAsync(user, model.RememberMe);
                                return RedirectToLocal(returnUrl);
                            }
                            else
                            {
                              //  auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "LOGIN_UNSUCCESSFUL_NO_ORGANIZATION_ASSOCIATED");
                                ModelState.AddModelError("", "This login needs a default Organization associated. Please contact Technical Support.");
                            }
                        }
                        else
                        {
                            auditResult = audit.LogEvent(Guid.Empty.ToString(), model.Email, Guid.Empty.ToString(), Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOGIN_UNSUCCESSFUL_NO_ORGANIZATION_ASSOCIATED");
                            
                            ModelState.AddModelError("", "This login is not associated with an Organization.");
                        }
                    }
                    else
                    {
                        auditResult = audit.LogEvent(Guid.Empty.ToString(), model.Email, Guid.Empty.ToString(), Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOGIN_UNSUCCESSFUL_UNCONFIRMED_EMAIL");
                            
                    //    auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "LOGIN_UNSUCCESSFUL_UNCONFIRMED_EMAIL");
                        ModelState.AddModelError("", "You must have a confirmed email to log on.");
                    }
                }
                else
                {
                    auditResult = audit.LogEvent(Guid.Empty.ToString(), model.Email, Guid.Empty.ToString(), Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOGIN_UNSUCCESSFUL_INVALID_USERNAME_PASSWORD");
                            
                //    auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "LOGIN_UNSUCCESSFUL_INVALID_USERNAME_PASSWORD");
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "LOGOFF");
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
           // auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "CONFIRM_EMAIL");

            if (userId == null || code == null) 
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                AspNetUser user = this.identityDB.AspNetUsers.Find(userId);
                if(user != null)
                {
                    SendOrgAccountRegCompleteNotification(user);
                }

                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        private void SendOrgAccountRegCompleteNotification(AspNetUser user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.DefaultOrganizationId))
                {
                    Guid orgGuid = Guid.Parse(user.DefaultOrganizationId);
                    GNAccount orgAccount = this.db.GNAccounts.Where(a => a.Organization.Id == orgGuid).FirstOrDefault();

                    if (orgAccount != null)
                    {
                        GNContact mainOrgContact = orgAccount.Organization.OrgMainContact;

                        if (mainOrgContact.AspNetUserId == user.Id)
                        {
                            //send notification
                            bool notifySuccess =
                                new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                                    "ORG_ACCOUNT_REGISTRATION_COMPLETE",
                                                    mainOrgContact.Email,
                                                    "Account:"+mainOrgContact.Id.ToString(),
                                                    new Dictionary<string, string>
                                                        {
                                                            {"OrganizationName",orgAccount.Organization.Name},
                                                            {"FirstName",mainOrgContact.FirstName},
                                                            {"LastName",mainOrgContact.LastName}
                                                        });
                        }
                    }
                }

            }
            catch (Exception e)
            {
                LogUtil.Error(logger,"Unable to send org account registration complete notification after Email Confirmation: " + e.Message,e);
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }
                
                //generate callback url
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, code = code }, protocol: GetURLScheme());		

                //send notification
                bool notifySuccess =
                    new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                        "USER_ACCOUNT_RESET_PASSWORD",
                        model.Email,
                        "Account:" + user.Id.ToString(),
                        new Dictionary<string, string>
                                {
                                    {"ResetPasswordUrl",callbackUrl}
                                });

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
	
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null) 
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    //send notification
                    bool notifySuccess =
                        new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                                        "USER_ACCOUNT_CHANGE_PASSWORD",
                                                        user.Email,
                                                        "User:"+user.Id.ToString(),
                                                        new Dictionary<string, string>
                                                                    {
                                                                        {"Email",user.Email}
                                                                    });

                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            //auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "RESET_PASSWORD");

            return View();
        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        public async Task<ActionResult> ResetPasswordAsAdmin(string Email)
        {
            var user = await UserManager.FindByNameAsync(Email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                return View();
            }
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPasswordAsAdmin", "Account",
                new { userId = user.Id, code = code }, protocol: GetURLScheme());

            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Code = code;
            model.Email = Email;
            return View(model);

        }

        [AuthorizeRedirect(Roles = "GN_ADMIN")]
        public async Task<ActionResult> ExecuteResetPasswordAsAdmin(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    //send notification
                    bool notifySuccess =
                        new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                                        "USER_ACCOUNT_CHANGE_PASSWORD_BY_ADMIN",
                                                        UserContact.Email,
                                                        "User:"+user.Id.ToString(),
                                                        new Dictionary<string, string>
                                                                    {
                                                                        {"UserEmail",model.Email}
                                                                    });

                    ViewBag.userId = user.Id;
                    return RedirectToAction("Details", "Users", new { id = user.Id });
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            return View();
            
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        // My Account - Controller Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region MyAccountControllerMethods

        private void EvalCanViewContact()
        {
            GNContact contact = new GNContact();
            contact = contactService.EvalEntitySecurity(UserContact, contact);
            ViewBag.CanViewContact = contact.CanView;
        }

        public ActionResult MyOrganization()
        {
            auditResult = audit.LogEvent(UserContact, UserContact.Id, this.ENTITY, this.Request.UserHostAddress, "VIEW_MY_ORGANIZATION");

            var contact = (GNContact)ViewBag.ContactForUser;
            GNOrganization organization = db.GNOrganizations.Find(contact.GNOrganizationId);
            if (organization == null)
            {
                return HttpNotFound();
            }

            //eval CanInvite
            GNContact contactObj = new GNContact
            {
                GNOrganizationId = UserContact.GNOrganizationId
            };
            contactObj = contactService.EvalEntitySecurity(UserContact, contactObj);
            ViewBag.CanInvite = contactObj.CanCreate;

            EvalCanViewContact();

            return View(organization);
        }

        public ActionResult SetDefaultOrganization()
        {
            string organizationId = Request["organizationId"];
            string aspNetUserId = Request["aspNetUserId"];
            //string returnUrl = Request["returnUrl"];

            //2016.02.12 tfrege. To avoid showing the user the 404 page when s/he is not supposed to see a Sample/Analysis on the new organization,
            //now s/he will be automatically redirected to the Dashboard.
            string returnUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            
            var aspNetUser = identityDB.AspNetUsers.Find(aspNetUserId);

            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "SET_DEFAULT_ORGANIZATION");

            if(aspNetUser != null)
            {
                aspNetUser.DefaultOrganizationId = organizationId;
                identityDB.Entry(aspNetUser).State = System.Data.Entity.EntityState.Modified;
                aspNetUser.Password = ".";
                aspNetUser.PasswordConfirm = ".";
                identityDB.SaveChanges();
            }

            if(!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("MyOrganization");
            }
        }

        public ActionResult SetAsAccountOwner()
        {
            string organizationId = Request["organizationId"];
            string contactId = Request["contactId"];

            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "SET_AS_ACCOUNT_OWNER: " + contactId);

            var org = this.db.GNOrganizations.Find(Guid.Parse(organizationId));
            var contact = this.db.GNContacts.Find(Guid.Parse(contactId));

            if(org != null && contact != null)
            {
                org.Account.AccountOwner = contact;
                this.db.SaveChanges();
            }

            return RedirectToAction("MyOrganization");
        }

        public ActionResult SetAsBillingContact()
        {
            string organizationId = Request["organizationId"];
            string contactId = Request["contactId"];

            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "SET_AS_BILLING_CONTACT: " + contactId);

            var org = this.db.GNOrganizations.Find(Guid.Parse(organizationId));
            var contact = this.db.GNContacts.Find(Guid.Parse(contactId));

            if (org != null && contact != null)
            {
                org.Account.BillingContact = contact;
                this.db.SaveChanges();
            }

            return RedirectToAction("MyOrganization");
        }

        public ActionResult SetAsMailingContact()
        {
            string organizationId = Request["organizationId"];
            string contactId = Request["contactId"];

            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "SET_AS_MAILING_CONTACT: " + contactId);

            var org = this.db.GNOrganizations.Find(Guid.Parse(organizationId));
            var contact = this.db.GNContacts.Find(Guid.Parse(contactId));

            if (org != null && contact != null)
            {
                org.Account.MailingContact = contact;
                this.db.SaveChanges();
            }

            return RedirectToAction("MyOrganization");
        }

        public ActionResult MyProfile()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_PROFILE_UI");

            var contact = (GNContact)ViewBag.ContactForUser; 
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }


        public ActionResult EditMyProfile()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_EDIT_MY_PROFILE_UI");

            var contact = (GNContact)ViewBag.ContactForUser;
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyProfile(GNContact contact)
        {
            if (ModelState.IsValid)
            {
                GNContact myProfileContact = this.db.GNContacts.Find(contact.Id);

                myProfileContact.FirstName = contact.FirstName;
                myProfileContact.LastName = contact.LastName;
                myProfileContact.Title = contact.Title;
                myProfileContact.Phone = contact.Phone;
                myProfileContact.Fax = contact.Fax;
                myProfileContact.Website = contact.Website;
                myProfileContact.StreetAddress1 = contact.StreetAddress1;
                myProfileContact.StreetAddress2 = contact.StreetAddress2;
                myProfileContact.City = contact.City;
                myProfileContact.State = contact.State;
                myProfileContact.Zip = contact.Zip;

                this.db.SaveChanges();

                return RedirectToAction("MyProfile");
            }
            
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "EDIT_MY_PROFILE");

            return View(contact);
        }

        //
        // GET: /Account/MyCredentials
        public ActionResult MyCredentials(ManageMessageId? message)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_CREDENTIALS_UI_GET_METHOD");

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("MyCredentials");
            return View();
        }

        //
        // POST: /Account/MyCredentials
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MyCredentials(ManageUserViewModel model)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_CREDENTIALS_UI_POST_METHOD");

            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("MyCredentials");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                        //send notification
                        bool notifySuccess =
                            new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                            "USER_ACCOUNT_CHANGE_PASSWORD",
                                            user.Email,
                                            "User:"+user.Id.ToString(),
                                            new Dictionary<string, string>
                                                    {
                                                        {"Email",user.Email}
                                                    });

                        await SignInAsync(user, isPersistent: false);

                        return RedirectToAction("MyCredentials", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("MyCredentials", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public async Task<ActionResult> MyBillingOverview()
        {

            if (UserContact != null && UserContact.IsInRole("GN_ADMIN"))
            {
                var remainingBudgets = await (new AccountService(db)).GetRemainingBudgetForAllAccounts();
            }



            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_BILLING_OVERVIEW_UI");

            MyBillingCanAccess();

            MyBillingOverviewModel model = new MyBillingOverviewModel();

            InvoiceService invoiceService = new InvoiceService(this.db);
            AccountService accountService = new AccountService(this.db);

            model.MyAccount = GetMyAccount();
            
            model.MyCurrentInvoice = await invoiceService.GetInvoiceForCurrentMonth(UserContact);
            model.MyLastInvoice = invoiceService.GetInvoiceForLastMonth(UserContact);
            
            model.MyBalancePastDue = accountService.GetBalancePastDue(UserContact, model.MyAccount, model.MyCurrentInvoice, model.MyLastInvoice);
            model.MaxApprovedSpend = accountService.GetTotalApprovedToSpend(UserContact, model.MyAccount);
            model.CurrentMonthSpend = -1 * model.MyCurrentInvoice.Total;
            model.MyRemainingBudget = (double)ViewBag.MyRemainingBudget;

            if (model.MyCurrentInvoice != null)
            {
                model.MyCurrentInvoice.TotalAppliedToPOs = invoiceService.GetInvoiceTotalPOApplied(model.MyCurrentInvoice.Id);
                model.MyAccount.TotalAppliedToPOs = model.MyCurrentInvoice.TotalAppliedToPOs;
            }

            if (model.MyLastInvoice != null)
            {
                model.MyLastInvoice.TotalAppliedToPOs = invoiceService.GetInvoiceTotalPOApplied(model.MyLastInvoice.Id);
            }

            //calc storage used
            model.MyStorageUsed = accountService.CalcStorageUsed(model.MyAccount.Id, DateTime.Now); 

            //get storage carry over product info from DB
            GNProduct storageCarryOverProduct = this.db.GNProducts.Where(p => p.Name == "STORAGE_S3_CARRYOVER").FirstOrDefault();
            model.StorageUnits = storageCarryOverProduct.ValueUnits;
            model.StoragePricePerUnit = storageCarryOverProduct.Price;
            model.MyStorageTotalCurrentCost = model.MyStorageUsed * model.StoragePricePerUnit;

            return View(model);
        }

        public ActionResult MyBillingBuyCredits()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_BILLING_BUY_CREDITS_UI");

            MyBillingCanAccess();

            MyBillingBuyCreditsModel model = new MyBillingBuyCreditsModel();
            model.MyAccount = GetMyAccount();
            model.MyProducts = this.db.GNProducts
                .Where(p => (p.AccountType.Id == UserContact.GNOrganization.Account.AccountType.Id && p.ProductType.Name != "STORAGE"));

            if(model.MyProducts != null && model.MyProducts.Count() != 0)
            {
                model.MinPurchaseAmount = model.MyProducts.Min(p => p.Price);
            }

            return View(model);
        }

        public ActionResult MyBillingPayments()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_BILLING_PAYMENTS_UI");

            MyBillingCanAccess();

            MyBillingPaymentsModel model = new MyBillingPaymentsModel();

            model.MyAccount = GetMyAccount();
            model.MyPayments = new List<GNPayment>();

            foreach (ICollection<GNPayment> pymts in this.db.GNInvoices.Where(i => i.Account.Id == model.MyAccount.Id).Select(i => i.Payments))
            {
                foreach (var pymt in pymts)
                {
                    model.MyPayments.Add(pymt);                    
                }
            }

            model.MyPayments = model.MyPayments.OrderByDescending(p => p.PaymentDate).ToList();

            return View(model);
        }

        public ActionResult MyBillingBills()
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_BILLING_BILLS_UI");

            MyBillingCanAccess();

            MyBillingBillsModel model = new MyBillingBillsModel();

            model.MyAccount = GetMyAccount();

            InvoiceService invoiceService = new InvoiceService(this.db);

            foreach (var invoice in model.MyAccount.Invoices)
            {
                invoice.TotalAppliedToPOs = invoiceService.GetInvoiceTotalPOApplied(invoice.Id);
                invoice.CreditsTotal = invoice.TotalAppliedToPOs + (-1 * invoiceService.GetInvoiceTotalCredits(invoice.Id));
                invoice.DebitsTotal = invoiceService.GetInvoiceTotalDebits(invoice.Id);
            }

            return View(model);
        }

        public ActionResult MyBillingBillDetail(Guid id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_BILLING_BILL_DETAILS_UI");


            //new PdfConverter().SavePdfFromUrlToFile("http://www.google.com", "test.pdf");


            MyBillingCanAccess();

            MyBillingBillDetailModel model = new MyBillingBillDetailModel();

            model.MyAccount = GetMyAccount();
            model.MyBill = this.db.GNInvoices.Find(id);

            InvoiceService invoiceService = new InvoiceService(this.db);
            model.MyBill.TotalAppliedToPOs = invoiceService.GetInvoiceTotalPOApplied(model.MyBill.Id);
            
            //tfrege 2015.10.19 added payments total
            model.MyBill.CreditsTotal = invoiceService.GetInvoiceTotalCredits(model.MyBill.Id) - model.MyBill.TotalAppliedToPOs + model.MyBill.PaymentsTotal;
            model.MyBill.DebitsTotal = invoiceService.GetInvoiceTotalDebits(model.MyBill.Id);

            foreach (var invoiceDetail in model.MyBill.InvoiceDetails)
            {
                string friendlyDescription = this.db.GNProducts
                    .Where(p => p.Name == invoiceDetail.Description)
                    .Select(p => p.Description)
                    .FirstOrDefault();

                if(!string.IsNullOrEmpty(friendlyDescription))
                {
                    invoiceDetail.Description = friendlyDescription;
                }
            }

            return View(model);
        }


        public ActionResult MyBillingBillDetailItemDetail(Guid id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, "LOAD_MY_BILLING_BILL_ITEM_DETAILS_UI");

            MyBillingCanAccess();

            MyBillingBillDetailItemDetailModel model = new MyBillingBillDetailItemDetailModel();

            model.MyAccount = GetMyAccount();
            model.MyBillDetail = this.db.GNInvoiceDetails.Find(id);

            return View(model);
        }

        private GNAccount GetMyAccount()
        {
            return base.db.GNAccounts
                .Where(a => a.Organization.Id == UserContact.GNOrganizationId)
                .FirstOrDefault();
        }

        private ActionResult MyBillingCanAccess()
        {
            bool canAccess = false;

            if (UserContact.IsInRole("ORG_MANAGER") || UserContact.IsInRole("GN_ADMIN"))
            {
                canAccess = true;
            }

            if (!canAccess)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Helper Methods
        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(await user.GenerateUserIdentityAsync(UserManager));
            //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}