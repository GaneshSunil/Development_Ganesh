using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using GenomeNext.Cloud.CloudNoSQL;
using System.Reflection;
using System.Data.SqlClient;
using GenomeNext.Data;
using GenomeNext.Data.Metadata;

namespace GenomeNext.App
{
    public class ContactService : GNEntityService<GNContact>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "CONTACT";

        public AspNetRoleService aspNetRoleService { get; set; }
        public AspNetUserRoleService aspNetUserRolesService { get; set; }

        public ContactService(GNEntityModelContainer db, IdentityModelContainer identityDB)
        {
            base.db = db;

            this.aspNetRoleService = new AspNetRoleService(identityDB);
            this.aspNetUserRolesService = new AspNetUserRoleService(identityDB);
        }

        public async Task<List<GNContact>> FindAllByOrg(GNContact userContact, int start, int end, Dictionary<string, object> filters, Guid organizationId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNContact> contacts = FindAllContacts(userContact, start, end, filters, organizationId);

            return EvalEntityListSecurity(userContact, await contacts.ToListAsync());
        }

        public override async Task<List<GNContact>> FindAll(GNContact userContact, int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNContact> contacts = FindAllContacts(userContact, start, end, filters, Guid.Empty);

            return EvalEntityListSecurity(userContact, await contacts.ToListAsync());
        }

        private IQueryable<GNContact> FindAllContacts(GNContact userContact, int start, int end, Dictionary<string, object> filters, Guid organizationId)
        {
            IQueryable<GNContact> contacts = null;

            //Filter by Role
            //GN_ADMIN
            if (aspNetRoleService.IsUserContactAdmin(userContact))
            {
                if(organizationId != Guid.Empty)
                {
                    contacts = db.GNContacts.Where(c => c.GNOrganizationId == organizationId);
                }
                else
                {
                    contacts = db.GNContacts;
                }
            }
            //ORG_MANAGER, TEAM_MANAGER, PROJECT_MANAGER, TEAM_MEMBER
            else
            {
                contacts = db.GNContacts
                    .Where(c => c.GNOrganizationId == userContact.GNOrganizationId);
            }

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Organization"))
                {
                    filterVal = (string)filters["Organization"];
                    contacts = contacts.Where(c => c.GNOrganization.Name.Contains(filterVal));
                }

                if (filters.ContainsKey("Name"))
                {
                    filterVal = (string)filters["Name"];
                    contacts = contacts.Where(c => (
                        c.LastName.Contains(filterVal)
                        || c.FirstName.Contains(filterVal)));
                }

                if (filters.ContainsKey("Email"))
                {
                    filterVal = (string)filters["Email"];
                    contacts = contacts.Where(c => c.Email.Contains(filterVal));
                }

                if (filters.ContainsKey("All"))
                {
                    filterVal = (string)filters["All"];
                    contacts = contacts
                        .Where(c => c.GNOrganization.Name.Contains(filterVal)
                            || c.LastName.Contains(filterVal)
                            || c.FirstName.Contains(filterVal)
                            || c.Email.Contains(filterVal));
                }
            }

            //Order By Results
            contacts = contacts
                .OrderBy(c => c.Email)
                .OrderBy(c => c.FirstName)
                .OrderBy(c => c.LastName)
                .OrderBy(c => c.GNOrganization.Name)
                .OrderByDescending(c => c.CreateDateTime);

            //Limit Result Size
            contacts = contacts.Skip(start).Take(end - start);
            return contacts;
        }

        public override async Task<GNContact> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNContacts.FindAsync(keys);
        }

        public override async Task<GNContact> Insert(object entity)
        {
            GNContact contact = (GNContact)entity;

            bool isExistingOrgContact = (this.db.GNContacts
                .Count(c => c.GNOrganizationId == contact.GNOrganizationId 
                    && c.Email == contact.Email) == 0) ? false : true;

            if (!isExistingOrgContact)
            {
                List<GNContactRole> contactRoles = null;
                if (contact.GNContactRoles != null && contact.GNContactRoles.Count != 0)
                {
                    contactRoles = new List<GNContactRole>(contact.GNContactRoles);
                    contact.GNContactRoles = null;
                }

                contact = await base.Insert(entity);

                contact = this.db.GNContacts.Find(contact.Id);

                contact.GNContactRoles = contactRoles;
                this.db.SaveChanges();
            }
            else
            {
                throw new Exception("User with this email is already a contact for this organization.");
            }

            bool auditResult = new GNCloudNoSQLService().LogEvent(contact.CreatedByContact, contact.Id, this.ENTITY, "N/A", "EVENT_INSERT");

            return contact;
        }

        public override async Task<GNContact> Update(GNContact userContact, object entity)
        {
            GNContact contact = (GNContact)entity;

            if(contact.IsSubscribedForNewsletters == null)
            {
                contact.IsSubscribedForNewsletters = false;
            }

            List<GNContactRole> contactRoles = null;
            if (contact.GNContactRoles != null && contact.GNContactRoles.Count != 0)
            {
                contactRoles = new List<GNContactRole>(contact.GNContactRoles.ToList());
            }

            contact = await base.Update(userContact, entity);

            var tx = db.Database.BeginTransaction();

            db.Database.ExecuteSqlCommand(
                "DELETE FROM [gn].[GNContactRoles] " +
                "WHERE [GNContactId] = @contactId ",
                new SqlParameter("@contactId", contact.Id));

            if (contactRoles != null)
            {
                foreach (var contactRole in contactRoles)
                {
                    db.Database.ExecuteSqlCommand(
                        "INSERT INTO [gn].[GNContactRoles] ([GNContactId],[AspNetRoleId],[CreatedBy],[CreateDateTime]) " +
                        "VALUES(@contactId,@aspNetRoleId,@createdBy,@createDateTime) ",
                        new SqlParameter("@contactId", contact.Id),
                        new SqlParameter("@aspNetRoleId", contactRole.AspNetRoleId),
                        new SqlParameter("@createdBy", userContact.Id),
                        new SqlParameter("@createDateTime", DateTime.Now));
                }
            }

            //tfrege 10/14/14
            List<GNNotificationTopicSubscriber> TopicSubscriptions = contact.GNNotificationTopicSubscribers.ToList();
            foreach (var subscription in TopicSubscriptions)
            {
                if(db.GNNotificationTopicSubscribers.Where(a => a.GNNotificationTopicId == subscription.GNNotificationTopicId && subscription.GNContactId.Equals(contact.Id)).Count() == 0)
                {
                    db.GNNotificationTopicSubscribers.Add(subscription);
                    db.SaveChanges();
                }
            }

            tx.Commit();

            contact = this.db.GNContacts.Find(contact.Id);

            bool auditResult = new GNCloudNoSQLService().LogEvent(userContact, contact.Id, this.ENTITY, "N/A", "EVENT_UPDATE");

            return contact;
        }

        public override async Task<int> Delete(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            int result = 0;

            try
            {
                GNContact contact = await Find(keys);

                var contactTeamMembers = db.GNTeamMembers.Where(tm => tm.GNContactId == contact.Id);
                if(contactTeamMembers != null && contactTeamMembers.Count() != 0)
                {
                    db.GNTeamMembers.RemoveRange(contactTeamMembers);
                    LogUtil.Info(logger, "Deleting contact team memberships");
                    result = await db.SaveChangesAsync();
                }

                contact.GNContactRoles.Clear();
                LogUtil.Info(logger, "Deleting contact roles");
                result = await db.SaveChangesAsync();

                contact.GNNotificationTopicSubscribers.Clear();
                LogUtil.Info(logger, "Deleting contact subscriptions");
                result = await db.SaveChangesAsync();

                db.Entry(contact).State = EntityState.Deleted;
                LogUtil.Info(logger, "Deleting contact");
                result = await db.SaveChangesAsync();
            }
            catch (Exception e1)
            {
                Exception e2 = new Exception("Unable to delete contact.", e1);
                LogUtil.Error(logger, e2.Message, e2);
                throw e2;
            }

            LogUtil.Info(logger, "Deleted contact");

            return result;
        }

        public override GNContact EvalEntitySecurity(GNContact userContact, GNContact contact)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (contact != null)
            {
                bool isContactOwnedByOrganization = IsContactOwnedByOrganization(userContact, contact);

                //GN_ADMIN
                if (aspNetRoleService.IsUserContactAdmin(userContact))
                {
                    contact.CanCreate = true;
                    contact.CanView = true;
                    contact.CanEdit = true;
                    contact.CanDelete = true;
                }
                //ORG_MANAGER
                else if (userContact.IsInRole("ORG_MANAGER"))
                {
                    contact.CanCreate = true;

                    if (isContactOwnedByOrganization)
                    {
                        contact.CanView = true;

                        if (!aspNetRoleService.IsUserContactAdmin(contact) 
                            && !contact.IsInRole("ORG_MANAGER"))
                        {
                            contact.CanEdit = true;
                            contact.CanDelete = true;
                        }
                        else
                        {
                            contact.CanEdit = false;
                            contact.CanDelete = false;
                        }
                    }
                }
                //TEAM_MANAGER
                else if (userContact.IsInRole("TEAM_MANAGER"))
                {
                    contact.CanCreate = true;

                    if (isContactOwnedByOrganization)
                    {
                        contact.CanView = true;

                        if (!aspNetRoleService.IsUserContactAdmin(contact)
                            && !contact.IsInRole("ORG_MANAGER")
                            && !contact.IsInRole("TEAM_MANAGER"))
                        {
                            contact.CanEdit = true;
                            contact.CanDelete = true;
                        }
                        else
                        {
                            contact.CanEdit = false;
                            contact.CanDelete = false;
                        }
                    }
                }
                //PROJECT_MANAGER, TEAM_MEMBER
                else
                {
                    contact.CanCreate = false;
                    contact.CanView = false;
                    contact.CanEdit = false;
                    contact.CanDelete = false;
                }

                //Prevent Deletion of contacts with associated entities
                /*
                if (contact.CanDelete)
                {
                    contact.CanDelete = true;
                }
                else
                {
                    contact.CanDelete = false;
                }
                */
            }

            return contact;
        }

        private bool IsContactOwnedByOrganization(GNContact userContact, GNContact contact)
        {
            return userContact.GNOrganizationId == contact.GNOrganizationId;
        }

    }
}
