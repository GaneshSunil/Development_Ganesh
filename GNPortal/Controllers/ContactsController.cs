using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Portal.Models;
using GenomeNext.Portal.ControllerExtensions;
using GenomeNext.App;
using GenomeNext.Utility;
using GenomeNext.Portal.Attributes;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect]
    public class ContactsController : GNEntityController<GNContact>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string ENTITY = "CONTACT";

        public TeamMemberService teamMemberService { get; set; }

        public ContactsController()
            : base()
        {
            entityService = new ContactService(base.db,base.identityDB);
            teamMemberService = new TeamMemberService(base.db, base.identityDB);
        }

        public override async Task<ActionResult> Index()
        {
            EvalCanCreate();

            auditResult = audit.LogEvent(UserContact, Guid.Empty, this.ENTITY, this.Request.UserHostAddress, EVENT_LOAD_INDEX_UI);

            List<GNContact> contacts = null;

            GNTeam team = null;
            if (!string.IsNullOrEmpty(Request["teamId"]))
            {
                team = entityService.db.GNTeams.Find(Guid.Parse(Request["teamId"]));
                ViewBag.Team = team;
            }

            GNOrganization org = null;
            if (!string.IsNullOrEmpty(Request["organizationId"]) || team != null)
            {
                if(team != null)
                {
                    org = team.Organization;
                }
                else
                {
                    org = entityService.db.GNOrganizations.Find(Guid.Parse(Request["organizationId"]));
                }

                ViewBag.Organization = org;

                contacts = await ((ContactService)this.entityService).FindAllByOrg(UserContact, IndexStart(), IndexEnd(), IndexFilters(), org.Id);
            }
            else
            {
                contacts = await this.entityService.FindAll(UserContact, IndexStart(), IndexEnd(), IndexFilters());
            }

            return View(contacts);
        }

        public async Task<ActionResult> DownloadSubscribedContacts()
        {
            List<GNContact> GNContacts = db.GNContacts.Where(a => a.IsSubscribedForNewsletters == true).ToList();

            string reportFileName = "SubscribedContacts_" + DateTime.Now + "_" + UserContact.Email + ".xls";
            GridView gv = new GridView();

            gv.DataSource = GNContacts;

            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + reportFileName);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return await Index();
        }

        public override void EvalCanCreate()
        {
            //eval CanCreate
            GNContact contact = new GNContact
            {
                GNOrganizationId = UserContact.GNOrganizationId
            };
            contact = ((ContactService)entityService).EvalEntitySecurity(UserContact, contact);
            ViewBag.CanCreate = contact.CanCreate;
        }


        public override GNContact PopulateSelectLists(GNContact entity)
        {
            string[] selectedRoles = null;
            if (entity != null)
            {
                if (entity.GNContactRoles != null)
                {
                    selectedRoles = entity.GNContactRoles.Select(r => r.AspNetRoleId).ToArray();
                }
            }
            
            IQueryable<AspNetRole> aspNetRoles = null;

            int UserMinHierarchyOrder = UserContact.GetUserMinHierarchyOrder();

            if (UserContact.IsInRole("GN_ADMIN") || UserContact.IsInRole("ORG_MANAGER"))
            {
                aspNetRoles = this.identityDB.AspNetRoles.Where(r => r.Name != "GN_ADMIN" && !r.Name.Contains("GN_"));
            }
            else if (UserContact.IsInRole("TEAM_MANAGER"))
            {
                aspNetRoles = this.identityDB.AspNetRoles.Where(r => (!r.Name.Contains("GN_") && r.Name != "ORG_MANAGER") && r.HierarchyOrder > UserMinHierarchyOrder);
            }
            else
            {
                aspNetRoles = this.identityDB.AspNetRoles.Where(r => r.Name == "TEAM_MEMBER" && r.HierarchyOrder > UserMinHierarchyOrder);
            }

            ViewBag.AspNetRoleList = new MultiSelectList(aspNetRoles, "Id", "Name", selectedRoles);

            if (!string.IsNullOrEmpty(Request["organizationId"]) && entity != null)
            {
                entity.GNOrganization = db.GNOrganizations.Find(Guid.Parse(Request["organizationId"]));
                entity.GNOrganizationId = entity.GNOrganization.Id;
            }
            else if (!string.IsNullOrEmpty(Request["teamId"]) && entity != null)
            {
                entity.GNOrganization = db.GNTeams.Find(Guid.Parse(Request["teamId"])).Organization;
                entity.GNOrganizationId = entity.GNOrganization.Id;
            }
            else if (entity != null && entity.GNOrganizationId != Guid.Empty)
            {
                entity.GNOrganization = db.GNOrganizations.Find(entity.GNOrganizationId);
            }

            if (!string.IsNullOrEmpty(Request["aspNetUserId"]) && Guid.Parse(Request["aspNetUserId"]) != Guid.Empty)
            {
                if (entity == null)
                {
                    entity = new GNContact();
                }

                AspNetUser aspNetUser = identityDB.AspNetUsers.Find(Request["aspNetUserId"]);
                if (aspNetUser != null)
                {
                    entity.User = aspNetUser;
                    entity.AspNetUserId = aspNetUser.Id;
                    entity.Email = aspNetUser.Email;
                }
            }

            return base.PopulateSelectLists(entity);
        }

        public override GNContact DetailsOnLoad(GNContact entity)
        {
            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_LOAD_DETAILS_UI);

            if (entity.GNContactRoles != null)
            {
                foreach (var contactRole in entity.GNContactRoles)
                {
                    contactRole.AspNetRole = base.identityDB.AspNetRoles.Find(contactRole.AspNetRoleId);
                }
            }

            Guid createdBy = Guid.Parse(entity.Id.ToString());
            List<GNSample> samples = db.GNSamples.Where(a => a.CreatedBy == entity.Id).ToList();
            List<GNAnalysisRequest> analyses = db.GNAnalysisRequests.Where(a => a.CreatedBy == entity.Id).ToList();

            ViewBag.SamplesByContact = samples.Count();
            ViewBag.AnalysesByContact = analyses.Count();

            return base.DetailsOnLoad(entity);
        }

        public override GNContact CreateOnLoad()
        {
            GNContact contact = base.CreateOnLoad();

            if(contact == null)
            {
                contact = new GNContact();
            }

            if (!string.IsNullOrEmpty(Request["organizationId"]))
            {
                contact.GNOrganization = db.GNOrganizations.Find(Guid.Parse(Request["organizationId"]));
                contact.GNOrganizationId = contact.GNOrganization.Id;
            }
            else if (!string.IsNullOrEmpty(Request["teamId"]))
            {
                contact.GNOrganization = db.GNTeams.Find(Guid.Parse(Request["teamId"])).Organization;
                contact.GNOrganizationId = contact.GNOrganization.Id;
            }
            else
            {
                contact.GNOrganization = db.GNOrganizations.Find(UserContact.GNOrganizationId);
                contact.GNOrganizationId = UserContact.GNOrganizationId;
            }

            return contact;
        }

        public override GNContact CreateOnSubmit(GNContact entity)
        {
            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_INSERT);

            if (!string.IsNullOrEmpty(Request["AspNetRoleList"]))
            {
                string[] selectedRoles = Request["AspNetRoleList"].Split(',');

                entity.CreateDateTime = DateTime.Now;
                entity.CreatedBy = UserContact.Id;
                entity.GNContactRoles = new List<GNContactRole>();
                foreach (var role in selectedRoles)
                {
                    entity.GNContactRoles.Add(new GNContactRole { 
                        AspNetRoleId = role,
                        CreateDateTime = DateTime.Now,
                        CreatedBy = UserContact.Id
                    });
                }
            }
            
            return base.CreateOnSubmit(entity);
        }

        public override ActionResult CreateOnSuccess(GNContact contact)
        {
            if (!string.IsNullOrEmpty(Request["teamId"]))
            {
                DoAddContactToTeam(Guid.Parse(Request["teamId"]), contact.Id);
            }

            /*
            List<GNNotificationTopicAddressee> NotificationTopicsList = new List<GNNotificationTopicAddressee>();
            List<GNNotificationTopicSubscriber> SubscriptionsForContact = new List<GNNotificationTopicSubscriber>();
            foreach (var role in entity.GNContactRoles)
            {
                try
                {
                    NotificationTopicsList = db.GNNotificationTopicAddressees.Where(t => t.AspNetRoleId.Equals(role.AspNetRoleId)).ToList();
                    foreach (var topicItem in NotificationTopicsList)
                    {
                        SubscriptionsForContact = db.GNNotificationTopicSubscribers.Where(a => a.GNContactId == UserContact.Id).ToList();
                        if (SubscriptionsForContact.Where(a => a.GNNotificationTopicId == topicItem.GNNotificationTopicId && a.GNContactId == UserContact.Id).Count() == 0)
                        {
                            entity.GNNotificationTopicSubscribers.Add(new GNNotificationTopicSubscriber
                            {
                                GNNotificationTopicId = topicItem.GNNotificationTopicId,
                                GNNotificationTopic = topicItem.GNNotificationTopic,
                                GNContactId = entity.Id,
                                GNContact = entity,
                                AddresseeType = topicItem.AddresseeType,
                                CreateDateTime = DateTime.Now,
                                CreatedBy = UserContact.Id
                            });
                        }
                    }
                }
                catch (Exception topicsEx)
                {
                    continue;
                }
            }
            this.entityService.db.SaveChanges();
            */

            
            if(Request["NotifyContact"] != null && bool.Parse(Request["NotifyContact"]) == true)
            {
                var org = entityService.db.GNOrganizations.Find(contact.GNOrganizationId);

                try
                {
                    //generate callback url
                    var callbackUrl = Url.Action("Contact", "Register",
                        new { organizationId = org.Id, contactId = contact.Id }, GetURLScheme());

                    //send notification via SQS
                    bool notifySuccess = new NotificationCloudMessageService().NotifyGNContact(
                                            "USER_ACCOUNT_SEND_INVITATION",
                                            contact.Email,
                                            "Contact:"+contact.Id.ToString(),
                                            new Dictionary<string, string>
                                            {
                                                {"OrganizationName",org.Name.Trim()},
                                                {"InvitedByName",UserContact.FullName},
                                                {"InvitationUrl",callbackUrl},
                                                {"FirstName",contact.FirstName}
                                            });
                }
                catch (Exception e)
                {
                    LogUtil.Error(logger, "Unable to send Invite Contact notification", e);
                    throw;
                }
            }

            return RedirectToAction("Details", new { 
                id = contact.Id,
                organizationId = Request["organizationId"],
                teamId = Request["teamId"],
                aspNetUserId = Request["aspNetUserId"]
            });
        }

        public ActionResult Invite()
        {
            return View(CreateOnLoad());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Invite(GNContact contact)
        {
            auditResult = audit.LogEvent(UserContact, contact.Id, this.ENTITY, this.Request.UserHostAddress, "INVITE");

            contact = CreateOnSubmit(contact);

            if (ModelState.IsValid)
            {
                try
                {
                    bool sendInviteEmail = false;
                    
                    GNContact existingContact = entityService.db.GNContacts
                        .Where(c => (c.Email == contact.Email && c.GNOrganizationId == contact.GNOrganizationId))
                        .FirstOrDefault();

                    if(existingContact != null)
                    {
                        if (existingContact.IsInviteAccepted.HasValue 
                            && existingContact.IsInviteAccepted.Value == true)
                        {
                            ModelState.AddModelError("Email", "A user with this email has already accepted an invitation to this Organization.");
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "A user with this email has already been invited to this Organization. The email invitation has been re-sent.");

                            //update contact
                            existingContact.IsInviteAccepted = false;
                            int result = this.entityService.db.SaveChanges();
                            contact = existingContact;
                            sendInviteEmail = true;
                        }
                    }
                    else
                    {
                        //insert contact
                        contact.AspNetUserId = Guid.Empty.ToString();
                        contact.IsInviteAccepted = false;
                        contact.IsSubscribedForNewsletters = false;
                        contact = await ((ContactService)entityService).Insert(UserContact, contact);

                        if (contact != null)
                        {
                            sendInviteEmail = true;
                        }
                    }

                    if (sendInviteEmail)
                    {
                        //fetch org
                        var org = entityService.db.GNOrganizations.Find(contact.GNOrganizationId);

                        try
                        {
                            //generate callback url
                            var callbackUrl = Url.Action("Contact", "Register",
                                new { organizationId = org.Id, contactId = contact.Id }, GetURLScheme());
                            
                            //send notification
                            bool notifySuccess =
                                                new GenomeNext.App.NotificationCloudMessageService().NotifyGNContact(
                                                    "USER_ACCOUNT_SEND_INVITATION",
                                                    contact.Email,
                                                    "Contact:" + contact.Id.ToString(),
                                                    new Dictionary<string, string>
                                                        {
                                                            {"OrganizationName",org.Name.Trim()},
                                                            {"InvitedByName",UserContact.FullName},
                                                            {"InvitationUrl",callbackUrl},
                                                            {"FirstName",contact.FirstName}
                                                        });
                        }
                        catch (Exception e)
                        {
                            LogUtil.Error(logger, "Unable to send Invite Contact notification", e);
                            throw;
                        }
                         
                        if(ModelState.IsValid)
                        {
                            return CreateOnSuccess(contact);
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.AddModelStateErrorsFromException(db, ModelState, ex);
                }
            }

            contact = CreateOnModelStateInValid(contact);

            return View(contact);
        }

        public override GNContact EditOnSubmit(GNContact entity)
        {
            auditResult = audit.LogEvent(UserContact, entity.Id, this.ENTITY, this.Request.UserHostAddress, EVENT_UPDATE);

            if (!string.IsNullOrEmpty(Request["AspNetRoleList"]))
            {
                string[] selectedRoles = Request["AspNetRoleList"].Split(',');

                entity.GNContactRoles = new List<GNContactRole>();
                foreach (var role in selectedRoles)
                {
                    entity.GNContactRoles.Add(new GNContactRole
                    {
                        AspNetRoleId = role,
                        CreateDateTime = DateTime.Now,
                        CreatedBy = UserContact.Id,
                        GNContactId = entity.Id
                    });
                }
            }
            
            return base.EditOnSubmit(entity);
        }

        public override ActionResult EditOnSuccess(GNContact entity)
        {
            try
            {
                //Get all the current subscriptions where subscribed = Y
                List<GNNotificationTopicSubscriber> Subscriptions = db.GNNotificationTopicSubscribers.Where(a => a.GNContactId.Equals(entity.Id) && a.IsSubscribed == "Y").ToList();
                int[] CurrentSubscriptions = Subscriptions.Select(b => b.GNNotificationTopicId).ToArray();

                List<GNNotificationTopicSubscriber> Unsubscriptions = db.GNNotificationTopicSubscribers.Where(a => a.GNContactId.Equals(entity.Id) && a.IsSubscribed == "N").ToList();
                int[] CurrentUnubscriptions = Subscriptions.Select(b => b.GNNotificationTopicId).ToArray();

                //Get all the roles for the contact
                foreach (var role in entity.GNContactRoles)
                {
                    //Get all the notifications set up for the roles
                    List<GNNotificationTopicAddressee> TopicsForRole = db.GNNotificationTopicAddressees.Where(a => a.AspNetRoleId.Equals(role.AspNetRoleId)).ToList();
                    int[] NewSubscriptions = TopicsForRole.Select(b => b.GNNotificationTopicId).ToArray();
                    int[] RemoveSubscriptions = CurrentSubscriptions.Except(NewSubscriptions).ToArray();
                    int[] NewSubscriptionsMinusUnubscriptions = NewSubscriptions.Except(CurrentUnubscriptions).ToArray();

                    // --> for each: if topic id is not in notifications, remove
                    foreach (var topic in RemoveSubscriptions)
                    {
                        db.GNNotificationTopicSubscribers.Remove(Subscriptions.Where(a => a.GNNotificationTopicId == topic).FirstOrDefault());
                        db.SaveChanges();
                    }

                    foreach (var topic in TopicsForRole.Where(a => NewSubscriptionsMinusUnubscriptions.Contains(a.GNNotificationTopicId)))
                    {
                        //If subscription doesn't exist yet with IsSubscribed = "N", add
                        db.GNNotificationTopicSubscribers.Add(new GNNotificationTopicSubscriber
                                                            {
                                                                AddresseeType = topic.AddresseeType,
                                                                GNContactId = entity.Id,
                                                                GNNotificationTopicId = topic.GNNotificationTopicId,
                                                                IsSubscribed = "Y",
                                                                CreateDateTime = DateTime.Now,
                                                                CreatedBy = UserContact.Id
                                                            });
                        db.SaveChanges();
                    }
                }

            }
            catch(Exception ex)
            {

            }


            return RedirectToAction("Details", new
            {
                id = entity.Id,
                organizationId = Request["organizationId"],
                teamId = Request["teamId"],
                aspNetUserId = Request["aspNetUserId"]
            });
        }




        public override ActionResult DeleteOnSuccess(string id = null)
        {
            return RedirectToAction("Index", new
            {
                organizationId = Request["organizationId"],
                teamId = Request["teamId"],
                aspNetUserId = Request["aspNetUserId"]
            });
        }

        public ActionResult AddContactToTeam(Guid teamId, Guid contactId)
        {
            auditResult = audit.LogEvent(UserContact, contactId, this.ENTITY, this.Request.UserHostAddress, "ADD_CONTACT_TO_TEAM " + teamId.ToString());

            GNTeamMember teamMember = DoAddContactToTeam(teamId, contactId);

            if (teamMember != null)
            {
                return RedirectToAction("Index", "Contacts", new { teamId = teamId });
            }
            else
            {
                throw new Exception("Unable to add team member.");
            }
        }

        private GNTeamMember DoAddContactToTeam(Guid teamId, Guid contactId)
        {
            GNTeamMember teamMember =
                new GNTeamMember
                {
                    GNContactId = contactId,
                    GNTeamId = teamId,
                    AssignDate = DateTime.Now,
                    CreatedBy = UserContact.Id,
                    CreateDateTime = DateTime.Now
                };

            this.db.GNTeamMembers.Add(teamMember);
            
            this.db.SaveChanges();

            return teamMember;
        }

        public async Task<ActionResult> RemoveContactFromTeam(Guid teamId, Guid contactId)
        {
            auditResult = audit.LogEvent(UserContact, contactId, this.ENTITY, this.Request.UserHostAddress, "REMOVE_CONTACT_FROM_TEAM " + teamId.ToString());

            int result = await this.teamMemberService.Delete(UserContact, new object[] { teamId, contactId });

            if (result != 0)
            {
                return RedirectToAction("Index", "Contacts", new { teamId = teamId });
            }
            else
            {
                throw new Exception("Unable to remove team member.");
            }
        }

        public override async Task<ActionResult> Delete(string id)
        {
            auditResult = audit.LogEvent(UserContact, Guid.Parse(id), this.ENTITY, this.Request.UserHostAddress, EVENT_DELETE);

            return await base.DeleteConfirmed(id);
        }

    }
}
