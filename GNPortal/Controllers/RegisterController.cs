using GenomeNext.App;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Notification;
using GenomeNext.Portal.Models;
using GenomeNext.Utility;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using CaptchaMvc.HtmlHelpers;

namespace GenomeNext.Portal.Controllers
{
    [AllowAnonymous]
    public class RegisterController : BaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OrganizationService organizationService { get; set; }
        public ContactService contactService { get; set; }
        public InviteCodeService inviteCodeService { get; set; }

        public RegisterController()
        {
            organizationService = new OrganizationService(base.db);
            contactService = new ContactService(base.db, base.identityDB);
            inviteCodeService = new InviteCodeService(base.db);
        }

        /////////////////////////////////////////////////////////////////////////////////
        // Register Contact / User
        /////////////////////////////////////////////////////////////////////////////////

        // GET: Register/Contact
        public async Task<ActionResult> Contact(Guid organizationId, Guid contactId)
        {
            GNOrganization organization = await organizationService.Find(organizationId);
            GNContact contact = await contactService.Find(contactId);

            if (contact != null && contact.IsInviteAccepted.HasValue && contact.IsInviteAccepted.Value)
            {
                return RedirectToAction("ContactComplete", new { id=contact.Id});
            }

            RegisterContactViewModel model = new RegisterContactViewModel
            {
                OrgId = organization.Id,
                OrgName = organization.Name,
                ContactId = contact.Id,
                Email = contact.Email,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Title = contact.Title,
                Phone = contact.Phone,
                Fax = contact.Fax,
                Website = contact.Website,
                StreetAddress1 = contact.StreetAddress1,
                StreetAddress2 = contact.StreetAddress2,
                City = contact.City,
                State = contact.State,
                Zip = contact.Zip,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactConfirm(RegisterContactViewModel model)
        {
            AspNetUser user = contactService.aspNetRoleService.db.AspNetUsers.Where(u => u.Email == model.Email).FirstOrDefault();

            if(user != null)
            {
                model.IsExistingUser = true;
            }

            model.Password = "";
            model.PasswordConfirm = "";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactSubmit(RegisterContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                string orgId = model.OrgId.ToString();
            
                //Auth or Create User
                AspNetUser aspNetUser = await AuthCreateUser(model.Email,model.Password,model.IsExistingUser,
                    verifyEmail: false, signUpForNews: model.SignUpForNewsAndProducts, organizationId: orgId);

                if (ModelState.IsValid && aspNetUser != null)
                {
                    GNContact contact = await contactService.Find(model.ContactId);

                    contact.AspNetUserId = aspNetUser.Id;

                    contact.Email = model.Email;
                    contact.FirstName = model.FirstName;
                    contact.LastName = model.LastName;
                    contact.Title = model.Title;
                    contact.Phone = model.Phone;
                    contact.Fax = model.Fax;
                    contact.Website = model.Website;
                    contact.StreetAddress1 = model.StreetAddress1;
                    contact.StreetAddress2 = model.StreetAddress2;
                    contact.City = model.City;
                    contact.State = model.State;
                    contact.Zip = model.Zip;

                    contact.IsInviteAccepted = true;
                    contact.IsSubscribedForNewsletters = model.SignUpForNewsAndProducts;
                    contact.TermsAcceptDateTime = DateTime.Now;
                    contact.PrivacyPolicyAcceptDateTime = DateTime.Now;

                    contact = await contactService.Update(contact);

                    if (contact != null)
                    {
                        return RedirectToAction("ContactComplete", new { id = contact.Id });
                    }
                }
            }

            return View(model);
        }

        public async Task<ActionResult> ContactComplete(Guid? id)
        {
            GNContact contact = await contactService.Find(id);

            ViewBag.userForRegContact = this.identityDB.AspNetUsers.Find(contact.AspNetUserId);

            return View(contact);
        }


        /////////////////////////////////////////////////////////////////////////////////
        // Register Account / Organization
        /////////////////////////////////////////////////////////////////////////////////
        
        private void LoadTimeZones(RegisterAccountViewModel model)
        {
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

            //load sample types (with pre-selected) into select list
            if (model != null)
            {
                ViewBag.UTCOffsetDescription = new SelectList(timeZones, "DisplayName", "DisplayName", model.UTCOffsetDescription.Trim());
            }
            else
            {
                ViewBag.UTCOffsetDescription = new SelectList(timeZones, "DisplayName", "DisplayName");
            }
        }

        // GET: Register/Account
        public ActionResult Account()
        {
            LoadTimeZones(null);
            return View();
        }

        // GET: Register/GEN2015
        public ActionResult GEN2015()
        {
            LoadTimeZones(null);
            return View();
        }


        public JsonResult ValidateInviteCode(string inviteCode)
        {
            bool isValidInviteCode = inviteCodeService.ValidateInviteCode(inviteCode);

            return Json(new Dictionary<string,bool>{{"isValidInviteCode",isValidInviteCode}},JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountConfirm(RegisterAccountViewModel model)
        {
            string captchaErrorMsg = "Incorrect Captcha Response";
            bool captchaValid = this.IsCaptchaValid(captchaErrorMsg);

            if (!captchaValid)
            {
                ModelState.AddModelError("CaptchaCode", captchaErrorMsg);
                LoadTimeZones(null);
                return View("Account");
            }
            else if (ModelState.IsValid) 
            {
                AspNetUser user = contactService.aspNetRoleService.db.AspNetUsers.Where(u => u.Email == model.Email).FirstOrDefault();

                if (user != null)
                {
                    model.IsExistingUser = true;
                }

                model.Password = "";
                model.PasswordConfirm = "";
            }

            LoadTimeZones(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountSubmit(RegisterAccountViewModel model)
        {
            if(ModelState.IsValid)
            {
                GNInviteCode inviteCodeRedemption = null;

                try
                {
                    //redeem invite code
                    inviteCodeRedemption = inviteCodeService.RedeemInviteCode(model.InviteCode);
                    
                    if (inviteCodeRedemption != null)
                    {
                        //Auth or Create User
                        AspNetUser aspNetUser = await AuthCreateUser(model.Email, model.Password, model.IsExistingUser,
                            verifyEmail:true, signUpForNews:model.SignUpForNewsAndProducts, organizationId: null);

                        if (ModelState.IsValid && aspNetUser != null)
                        {
                            //Insert New Org
                            GNOrganization org = await InsertNewOrganization(model);

                            if(org != null)
                            {
                                //Insert Main Org Contact
                                GNContact mainOrgContact = await InsertMainOrgContact(model, aspNetUser, org);

                                if (mainOrgContact != null)
                                {
                                    double maxBalanceAllowed = 0.0;
                                    bool validBillingAgreementRequired = false;

                                    //set free credit allowance based on invite code redemption
                                    if(inviteCodeRedemption.FreeCreditAllowance > 0.0)
                                    {
                                        maxBalanceAllowed = inviteCodeRedemption.FreeCreditAllowance;
                                    }

                                    GNAccount orgAccount = 
                                        await InsertNewOrgAccount(org, mainOrgContact,maxBalanceAllowed,validBillingAgreementRequired);


                                    
                                    if (inviteCodeRedemption.InviteCode.Equals("GENandGEN2015"))
                                    {
                                        org.GNSettingsTemplate = db.GNSettingsTemplates.Where(a => a.Name == "GN_GENOMICS2015").FirstOrDefault();
                                        db.SaveChanges();
                                    }



                                    if (orgAccount != null)
                                    {
                                        if(model.IsExistingUser)
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

                                        return RedirectToAction("AccountComplete", new { id = org.Id });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("InviteCode", "Unable to redeem Invite Code.");
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(logger, "Unable to perform Account Registration", ex);
                    ModelState.AddModelError("", "Unable to perform Account Registration");
                }

                if (inviteCodeRedemption != null)
                {
                    inviteCodeService.UnRedeemInviteCode(model.InviteCode);
                }
            }

            return View(model);
        }

        private async Task<GNAccount> InsertNewOrgAccount(GNOrganization org, GNContact mainOrgContact,
            double maxBalanceAllowed = 0.0, bool validBillingAgreementRequired = true)
        {
            GNAccount acct = null;

            try
            {
                acct = new GNAccount
                {
                    Id = Guid.NewGuid(),
                    Organization = org,
                    GNAccountTypeId = this.db.GNAccountTypes.Where(t => t.Name == "INDUSTRY").Select(t => t.Id).FirstOrDefault(),
                    AccountOwner = mainOrgContact,
                    BillingContact = mainOrgContact,
                    MailingContact = mainOrgContact,
                    CreateDateTime = DateTime.Now,
                    CreatedBy = mainOrgContact.Id,
                    DefaultDiscountAmount = 0.0,
                    DefaultDiscountType = GNAccount.DiscountType.FLAT.GetCode(),
                    BillingMode = GNAccount.BillingModeType.INVOICE.GetCode(),
                    TotalAmountOwed = 0,
                    TotalAmountPaid = 0,
                    MaxBalanceAllowed = maxBalanceAllowed,
                    ValidBillingAgreementRequired = validBillingAgreementRequired
                };

                acct = this.db.GNAccounts.Add(acct);
                int result = await this.db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var ex2 = new Exception("Unable to add Account for Organization.", ex);
                LogUtil.Error(logger, ex2.Message, ex2);
                throw ex2;
            } 
            
            return acct;
        }

        private async Task<GNOrganization> InsertNewOrganization(RegisterAccountViewModel model)
        {
            GNOrganization org = null;

            try
            {
                //insert new Org
                org = new GNOrganization
                {
                    Name = model.OrgName,
                    UTCOffsetDescription = model.UTCOffsetDescription,
                    AWSConfigId = base.db.AWSConfigs.FirstOrDefault().Id,
                    CreateDateTime = DateTime.Now,
                };
                org = await organizationService.Insert(org);

            }
            catch (Exception ex)
            {
                var ex2 = new Exception("Unable to add Organization.", ex);
                LogUtil.Error(logger, ex2.Message, ex2);
                throw ex2;
            } 
            
            return org;
        }

        private async Task<GNContact> InsertMainOrgContact(RegisterAccountViewModel model, AspNetUser aspNetUser, GNOrganization org)
        {
            GNContact mainOrgContact = null;

            try
            {
                //find ORG_MANAGER role
                AspNetRole aspNetRole = base.identityDB.AspNetRoles.Where(r => r.Name == "ORG_MANAGER").FirstOrDefault();

                //insert Main Org Contact with ORG_MANAGER role
                mainOrgContact = new GNContact
                {
                    AspNetUserId = aspNetUser.Id,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Title = model.Title,
                    Phone = model.Phone,
                    Fax = model.Fax,
                    Website = model.Website,
                    StreetAddress1 = model.StreetAddress1,
                    StreetAddress2 = model.StreetAddress2,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    TermsAcceptDateTime = DateTime.Now,
                    PrivacyPolicyAcceptDateTime = DateTime.Now,
                    CreateDateTime = DateTime.Now,
                    GNOrganizationId = org.Id,
                    IsSubscribedForNewsletters = model.SignUpForNewsAndProducts
                };
                mainOrgContact = await contactService.Insert(mainOrgContact);

                //Update Org
                org.GNContactId = mainOrgContact.Id;
                org.CreatedBy = mainOrgContact.Id;
                org = await organizationService.Update(org);

                if (org != null)
                {
                    mainOrgContact.CreatedBy = mainOrgContact.Id;
                    mainOrgContact.GNOrganizationId = org.Id;
                    mainOrgContact.GNContactRoles = new List<GNContactRole>();
                    mainOrgContact.GNContactRoles.Add(new GNContactRole
                    {
                        AspNetRoleId = aspNetRole.Id,
                        CreatedBy = mainOrgContact.Id,
                        CreateDateTime = DateTime.Now
                    });

                    mainOrgContact = await contactService.Update(org.OrgMainContact);
                }

            }
            catch (Exception ex)
            {
                var ex2 = new Exception("Unable to add Main Organization Contact.", ex);
                LogUtil.Error(logger, ex2.Message, ex2);
                throw ex2;
            }

            return mainOrgContact;
        }

        public async Task<ActionResult> AccountComplete(Guid? id)
        {
            GNOrganization org = await organizationService.Find(id);

            ViewBag.userForMainOrgContact = this.identityDB.AspNetUsers.Find(org.OrgMainContact.AspNetUserId);

            return View(org);
        }

        //////////////////////////////////////////////////////////////////////////////
        // Common Methods
        //////////////////////////////////////////////////////////////////////////////

        private async Task<AspNetUser> AuthCreateUser(string email, string password, bool isExistingUser, bool verifyEmail, bool signUpForNews, string organizationId)
        {
            AspNetUser aspNetUser = base.identityDB.AspNetUsers.Where(u => u.Email == email).FirstOrDefault();

            //auth User, if existing
            if (isExistingUser)
            {
                if (aspNetUser != null)
                {
                    ApplicationUser user = new ApplicationUser() { UserName = email, Email = email };
                    user = await base.UserManager.FindAsync(user.UserName, password);

                    if (user == null)
                    {
                        aspNetUser = null;
                        ModelState.AddModelError("Password", "User Email and/or Password is not valid");
                    }
                }
                else
                {
                    ModelState.AddModelError("IsExistingUser", "User does not exist");
                }
            }
            //else create User, send out verification email
            else
            {
                if (aspNetUser == null)
                {
                    ApplicationUser user = new ApplicationUser() { UserName = email, Email = email };
                    IdentityResult result = await UserManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        aspNetUser = identityDB.AspNetUsers.SingleOrDefault(u => u.Email == email);
                        user = await UserManager.FindByEmailAsync(user.Email);


                        if (organizationId != null && aspNetUser.DefaultOrganizationId == null)
                        {
                            aspNetUser.DefaultOrganizationId = organizationId;
                            db.SaveChanges();
                        }

                        //send email verification notification to user
                        if(verifyEmail)
                        {
                            //generate callback url
                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account",
                                new { userId = user.Id, code = code }, protocol: GetURLScheme());
                            
                            //send notification
                            bool notifySuccess =
                                new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                    "USER_ACCOUNT_CONFIRM_EMAIL",
                                    user.Email,
                                    "User:" + user.Id.ToString(),
                                    new Dictionary<string, string>
                                {
                                    {"InvitationUrl",callbackUrl},
                                    {"Email",user.Email}
                                });
                        }
                        //set email as already confirmed
                        else
                        {
                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            result = await UserManager.ConfirmEmailAsync(user.Id, token: code);
                        }

                        //subscribe user to news
                        if (signUpForNews)
                        {
                            //Subscribe the user for news
                            GNNotificationTopic NewsTopic = db.GNNotificationTopics.Where(a => a.Code.Equals("GENOMENEXT_NEWS_AND_PRODUCTS") && a.Status == "ACTIVE").FirstOrDefault();
                            GNContact UserContact = db.GNContacts.Where(a => a.Email.Equals(aspNetUser.Email)).FirstOrDefault();

                            if (UserContact != null)
                            {
                                UserContact.IsSubscribedForNewsletters = true;
                            }
                            
                            db.SaveChanges();
                            
                            if (NewsTopic != null && UserContact != null)
                            {
                                UserContact.GNNotificationTopicSubscribers.Add(new GNNotificationTopicSubscriber
                                {
                                    AddresseeType = "TO",
                                    GNContactId = UserContact.Id,
                                    GNNotificationTopicId = NewsTopic.Id,
                                    IsSubscribed = "Y",
                                    CreateDateTime = DateTime.Now,
                                    CreatedBy = UserContact.Id
                                });
                            }
                        }
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "User with this Email Address already exists");
                }
            }
            return aspNetUser;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}