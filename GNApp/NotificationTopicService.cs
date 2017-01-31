using GenomeNext.Data.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using GenomeNext.Utility;
using System.Reflection;
using GenomeNext.Data;

namespace GenomeNext.App
{
    public class NotificationTopicService : GNEntityService<GNNotificationTopic>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NotificationTopicService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNNotificationTopic>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNNotificationTopic> entities =
                await db.GNNotificationTopics
                .ToListAsync();

            return entities;
        }

        public override async Task<GNNotificationTopic> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNNotificationTopics.FindAsync(keys);
        }

    }



    public class NotificationTopicAddresseeService : GNEntityService<GNNotificationTopicAddressee>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NotificationTopicAddresseeService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }


        public bool Edit(GNNotificationTopic gnNotificationTopic)
        {
            var tx = db.Database.BeginTransaction();
            Guid createdBy = Guid.Parse(gnNotificationTopic.CreatedBy.ToString());

            try
            {
                if (gnNotificationTopic.RemoveToRoles != null)
                {
                    foreach (var addressee in db.GNNotificationTopicAddressees.Where(a => gnNotificationTopic.RemoveToRoles.Contains(a.AspNetRoleId) && a.AddresseeType == "TO"))
                    {
                        db.GNNotificationTopicAddressees.Remove(addressee);
                        List<GNNotificationTopicSubscriber> Subscribers = db.GNNotificationTopicSubscribers.Where(a => a.AddresseeType == "TO" && a.GNNotificationTopicId == gnNotificationTopic.Id && a.IsSubscribed == "Y").ToList();
                        db.GNNotificationTopicSubscribers.RemoveRange(Subscribers);
                        db.SaveChanges();
                    }
                }

                if (gnNotificationTopic.AddToRoles != null)
                {
                    foreach (var role in gnNotificationTopic.AddToRoles)
                    {
                        db.GNNotificationTopicAddressees.Add(new GNNotificationTopicAddressee
                        {
                            AspNetRoleId = role,
                            GNNotificationTopicId = gnNotificationTopic.Id,
                            AddresseeType = "TO",
                            CreateDateTime = DateTime.Now,
                            CreatedBy = createdBy
                        });

                        List<GNContactRole> Contacts = db.GNContactRoles.Where(a => a.AspNetRoleId.Equals(role)).ToList();
                        List<GNNotificationTopicSubscriber> Subscribers = db.GNNotificationTopicSubscribers.Where(a => a.AddresseeType == "TO" && a.GNNotificationTopicId == gnNotificationTopic.Id).ToList();
                        foreach (var contact in Contacts)
                        {
                            if (Subscribers.Where(a => a.GNContactId.Equals(contact.GNContactId)).Count() == 0)
                            {
                                db.GNNotificationTopicSubscribers.Add(new GNNotificationTopicSubscriber
                                {
                                    AddresseeType = "TO",
                                    GNContactId = contact.GNContactId,
                                    GNNotificationTopicId = gnNotificationTopic.Id,
                                    IsSubscribed = "Y",
                                    CreateDateTime = DateTime.Now,
                                    CreatedBy = createdBy
                                });
                            }
                        }

                        db.SaveChanges();
                    }
                }

                if (gnNotificationTopic.RemoveCcRoles != null)
                {
                    foreach (var addressee in db.GNNotificationTopicAddressees.Where(a => gnNotificationTopic.RemoveCcRoles.Contains(a.AspNetRoleId) && a.AddresseeType == "CC"))
                    {
                        db.GNNotificationTopicAddressees.Remove(addressee);
                        List<GNNotificationTopicSubscriber> Subscribers = db.GNNotificationTopicSubscribers.Where(a => a.AddresseeType == "CC" && a.GNNotificationTopicId == gnNotificationTopic.Id && a.IsSubscribed == "Y").ToList();
                        db.GNNotificationTopicSubscribers.RemoveRange(Subscribers);
                        db.SaveChanges();
                    }
                }

                if (gnNotificationTopic.AddCcRoles != null)
                {
                    foreach (var role in gnNotificationTopic.AddCcRoles)
                    {
                        db.GNNotificationTopicAddressees.Add(new GNNotificationTopicAddressee
                        {
                            AspNetRoleId = role,
                            GNNotificationTopicId = gnNotificationTopic.Id,
                            AddresseeType = "CC",
                            CreateDateTime = DateTime.Now,
                            CreatedBy = createdBy
                        });

                        List<GNContactRole> Contacts = db.GNContactRoles.Where(a => a.AspNetRoleId.Equals(role)).ToList();
                        List<GNNotificationTopicSubscriber> Subscribers = db.GNNotificationTopicSubscribers.Where(a => a.AddresseeType == "CC" && a.GNNotificationTopicId == gnNotificationTopic.Id).ToList();
                        foreach (var contact in Contacts)
                        {
                            if (Subscribers.Where(a => a.GNContactId.Equals(contact.GNContactId)).Count() == 0)
                            {
                                db.GNNotificationTopicSubscribers.Add(new GNNotificationTopicSubscriber
                                {
                                    AddresseeType = "CC",
                                    GNContactId = contact.GNContactId,
                                    GNNotificationTopicId = gnNotificationTopic.Id,
                                    IsSubscribed = "Y",
                                    CreateDateTime = DateTime.Now,
                                    CreatedBy = createdBy
                                });
                            }
                        }
                        db.SaveChanges();
                    }
                }

                if (gnNotificationTopic.RemoveBccRoles != null)
                {
                    foreach (var addressee in db.GNNotificationTopicAddressees.Where(a => gnNotificationTopic.RemoveBccRoles.Contains(a.AspNetRoleId) && a.AddresseeType == "BCC"))
                    {
                        db.GNNotificationTopicAddressees.Remove(addressee);
                        List<GNNotificationTopicSubscriber> Subscribers = db.GNNotificationTopicSubscribers.Where(a => a.AddresseeType == "BCC" && a.GNNotificationTopicId == gnNotificationTopic.Id && a.IsSubscribed == "Y").ToList();
                        db.GNNotificationTopicSubscribers.RemoveRange(Subscribers);
                        db.SaveChanges();
                    }
                }

                if (gnNotificationTopic.AddBccRoles != null)
                {
                    foreach (var role in gnNotificationTopic.AddBccRoles)
                    {
                        db.GNNotificationTopicAddressees.Add(new GNNotificationTopicAddressee
                        {
                            AspNetRoleId = role,
                            GNNotificationTopicId = gnNotificationTopic.Id,
                            AddresseeType = "BCC",
                            CreateDateTime = DateTime.Now,
                            CreatedBy = createdBy
                        });

                        List<GNContactRole> Contacts = db.GNContactRoles.Where(a => a.AspNetRoleId.Equals(role)).ToList();
                        List<GNNotificationTopicSubscriber> Subscribers = db.GNNotificationTopicSubscribers.Where(a => a.AddresseeType == "BCC" && a.GNNotificationTopicId == gnNotificationTopic.Id).ToList();
                        foreach (var contact in Contacts)
                        {
                            if (Subscribers.Where(a => a.GNContactId.Equals(contact.GNContactId)).Count() == 0)
                            {
                                db.GNNotificationTopicSubscribers.Add(new GNNotificationTopicSubscriber
                                {
                                    AddresseeType = "BCC",
                                    GNContactId = contact.GNContactId,
                                    GNNotificationTopicId = gnNotificationTopic.Id,
                                    IsSubscribed = "Y",
                                    CreateDateTime = DateTime.Now,
                                    CreatedBy = createdBy
                                });
                            }
                        }
                        
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {

                LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
                throw;
            }

            tx.Commit();
            return true;
        }
        
        public override async Task<List<GNNotificationTopicAddressee>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNNotificationTopicAddressee> entities =
                await db.GNNotificationTopicAddressees
                .ToListAsync();

            return entities;
        }

        public override async Task<GNNotificationTopicAddressee> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNNotificationTopicAddressees.FindAsync(keys);
        }
    }


    public class NotificationSenderService : GNEntityService<GNNotificationSender>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NotificationSenderService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNNotificationSender>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<GNNotificationSender> entities =
                await db.GNNotificationSenders
                .ToListAsync();

            return entities;
        }

        public override async Task<GNNotificationSender> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNNotificationSenders.FindAsync(keys);
        }
    }
    
    public class NotificationLogService : GNEntityService<GNNotificationLog>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NotificationLogService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNNotificationLog>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNNotificationLog> NotificationLogs = db.GNNotificationLogs;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Topic"))
                {
                    filterVal = filters["Topic"].ToString().Trim().ToUpper();
                    NotificationLogs = NotificationLogs.Where(t => t.GNNotificationTopic.Code.ToUpper().Contains(filterVal));
                }

                if (filters.ContainsKey("Sender"))
                {
                    filterVal = filters["Sender"].ToString().Trim().ToUpper();
                    NotificationLogs = NotificationLogs.Where(t => t.Sender.ToUpper().Contains(filterVal));
                }

                if (filters.ContainsKey("Addressee"))
                {
                    filterVal = filters["Addressee"].ToString().Trim().ToUpper();
                    NotificationLogs = NotificationLogs.Where(t => t.Addressee.ToUpper().Contains(filterVal));
                }

                if (filters.ContainsKey("Subject"))
                {
                    filterVal = filters["Subject"].ToString().Trim().ToUpper();
                    NotificationLogs = NotificationLogs.Where(t => t.Subject.ToUpper().Contains(filterVal));
                }

                if (filters.ContainsKey("NotificationServiceResponse"))
                {
                    filterVal = filters["NotificationServiceResponse"].ToString().Trim().ToUpper();
                    NotificationLogs = NotificationLogs.Where(t => t.NotificationServiceResponse.ToUpper().Contains(filterVal));
                }

                if (filters.ContainsKey("Date"))
                {
                    filterVal = filters["Date"].ToString().Trim().ToUpper();
                    NotificationLogs = NotificationLogs.Where(t => t.CreateDateTime.ToString().Contains(filterVal));
                }

                if (filters.ContainsKey("GNNotificationTopicId"))
                {
                    var intFilterVal = Convert.ToInt32(filters["GNNotificationTopicId"]);
                    NotificationLogs = NotificationLogs.Where(t => t.GNNotificationTopicId == intFilterVal);
                }

            }

            //Order By Results
            NotificationLogs = NotificationLogs
                .OrderByDescending(t => t.CreateDateTime);

            //Limit Result Size
            NotificationLogs = NotificationLogs.Skip(start).Take(end - start);

            return NotificationLogs.ToList();
        }

        public override async Task<GNNotificationLog> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNNotificationLogs.FindAsync(keys);
        }
    }

    public class NotificationTopicSubscriberService : GNEntityService<GNNotificationTopicSubscriber>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NotificationTopicSubscriberService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNNotificationTopicSubscriber>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNNotificationTopicSubscriber> NotificationSubscribers = db.GNNotificationTopicSubscribers;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("GNContactId"))
                {
                    filterVal = filters["GNContactId"].ToString();
                    NotificationSubscribers = NotificationSubscribers.Where(t => t.GNContactId.ToString() == filterVal);
                }

                if (filters.ContainsKey("Organization"))
                {
                    filterVal = filters["Organization"].ToString().ToLower();
                    NotificationSubscribers = NotificationSubscribers.Where(t => t.GNContact.GNOrganization.Name.ToLower().Contains(filterVal));
                }

                if (filters.ContainsKey("GNNotificationTopicId"))
                {
                    var filterValInt = int.Parse(filters["GNNotificationTopicId"].ToString());
                    NotificationSubscribers = NotificationSubscribers.Where(t => t.GNNotificationTopicId == filterValInt);
                }

                if (filters.ContainsKey("IsSubscribed"))
                {
                    filterVal = filters["IsSubscribed"].ToString();
                    NotificationSubscribers = NotificationSubscribers.Where(t => t.IsSubscribed == filterVal);
                }

                if (filters.ContainsKey("IsSubscriptionOptional"))
                {
                    String sFilterVal = filters["IsSubscriptionOptional"].ToString();
                    NotificationSubscribers = NotificationSubscribers.Where(t => t.GNNotificationTopic.IsSubscriptionOptional.Equals(sFilterVal));
                }
            }

            return NotificationSubscribers.ToList();
        }

        public override async Task<GNNotificationTopicSubscriber> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNNotificationTopicSubscribers.FindAsync(keys);
        }
    }

    public class NotificationSuppressionListsService : GNEntityService<GNNotificationSuppressionList>
    {
               private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

               public NotificationSuppressionListsService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNNotificationSuppressionList>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<GNNotificationSuppressionList> NotificationSuppressionLists = db.GNNotificationSuppressionLists;

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Email"))
                {
                    filterVal = filters["Email"].ToString();
                    NotificationSuppressionLists = NotificationSuppressionLists.Where(t => t.Email.ToString().Contains(filterVal));
                }

                if (filters.ContainsKey("Category"))
                {
                    filterVal = filters["Category"].ToString().ToLower();
                    NotificationSuppressionLists = NotificationSuppressionLists.Where(t => t.Category.ToString().Contains(filterVal));
                }

                if (filters.ContainsKey("Type"))
                {
                    filterVal = filters["Type"].ToString().ToLower();
                    NotificationSuppressionLists = NotificationSuppressionLists.Where(t => t.Type.ToString().Contains(filterVal));
                }

                if (filters.ContainsKey("Subtype"))
                {
                    filterVal = filters["Subtype"].ToString().ToLower();
                    NotificationSuppressionLists = NotificationSuppressionLists.Where(t => t.Subtype.ToString().Contains(filterVal));
                }
            }

            return NotificationSuppressionLists.ToList();
        }

        public override async Task<GNNotificationSuppressionList> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNNotificationSuppressionLists.FindAsync(keys);
        }
    }
}
