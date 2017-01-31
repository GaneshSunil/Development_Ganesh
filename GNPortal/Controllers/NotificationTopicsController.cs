using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GenomeNext.Data.EntityModel;
using GenomeNext.App;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Cloud.Messaging;
using GenomeNext.Notification;
using GenomeNext.Portal.Attributes;

namespace GenomeNext.Portal.Controllers
{
    [AuthorizeRedirect(Roles = "GN_ADMIN")]
    public class NotificationTopicsController : GNEntityController<GNNotificationTopic>
    {
        public NotificationTopicsController()
            : base()
        {
            entityService = new NotificationTopicService(base.db);
        }

        [HttpPost, ValidateInput(false)]
        public override async Task<ActionResult> Create(GNNotificationTopic gnNotificationTopic)
        {
            if (Request.Form.GetValues("NotifyObjectCreatorBool") != null && Boolean.Parse(Request.Form.GetValues("NotifyObjectCreatorBool")[0]) == true)
            {
                gnNotificationTopic.NotifyObjectCreator = "Y";
            }
            else
            {
                gnNotificationTopic.NotifyObjectCreator = "N";
            }

            if (!string.IsNullOrEmpty(Request["ToRolesList"]))
            {
                string[] selectedToRoles = Request["ToRolesList"].Split(',');

                foreach (var role in selectedToRoles)
                {
                    gnNotificationTopic.GNNotificationTopicAddressees.Add(new GNNotificationTopicAddressee
                    {
                        AspNetRoleId = role,
                        GNNotificationTopicId = gnNotificationTopic.Id,
                        AddresseeType = "TO",
                        CreateDateTime = DateTime.Now,
                        CreatedBy = UserContact.Id
                    });

                    //Update subscribers
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
                                CreatedBy = UserContact.Id
                            });
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(Request["CCRolesList"]))
            {
                string[] selectedCcRoles = Request["CCRolesList"].Split(',');

                foreach (var role in selectedCcRoles)
                {
                    gnNotificationTopic.GNNotificationTopicAddressees.Add(new GNNotificationTopicAddressee
                    {
                        AspNetRoleId = role,
                        GNNotificationTopicId = gnNotificationTopic.Id,
                        AddresseeType = "CC",
                        CreateDateTime = DateTime.Now,
                        CreatedBy = UserContact.Id
                    });

                    //Update subscribers
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
                                CreatedBy = UserContact.Id
                            });
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(Request["BCCRolesList"]))
            {
                string[] selectedBccRoles = Request["BCCRolesList"].Split(',');

                foreach (var role in selectedBccRoles)
                {
                    gnNotificationTopic.GNNotificationTopicAddressees.Add(new GNNotificationTopicAddressee
                    {
                        AspNetRoleId = role,
                        GNNotificationTopicId = gnNotificationTopic.Id,
                        AddresseeType = "BCC",
                        CreateDateTime = DateTime.Now,
                        CreatedBy = UserContact.Id
                    });

                    //Update subscribers
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
                                CreatedBy = UserContact.Id
                            });
                        }
                    }

                }
            }

            return await base.Create(gnNotificationTopic);
        }
        
        [HttpPost, ValidateInput(false)]
        public override async Task<ActionResult> Edit(GNNotificationTopic gnNotificationTopic)
        {
            gnNotificationTopic.CreatedBy = UserContact.Id;
            gnNotificationTopic.CreateDateTime = DateTime.Now;

            if (Request.Form.GetValues("NotifyObjectCreatorBool") != null && Boolean.Parse(Request.Form.GetValues("NotifyObjectCreatorBool")[0]) == true)
            {
                gnNotificationTopic.NotifyObjectCreator = "Y";
            }
            else
            {
                gnNotificationTopic.NotifyObjectCreator = "N";
            }


            if (!string.IsNullOrEmpty(Request["ToRolesList"]))
            {
                string[] selectedToRoles = Request["ToRolesList"].Split(',');
                string[] currentToRoles = db.GNNotificationTopicAddressees.Where(a => a.GNNotificationTopicId == gnNotificationTopic.Id && a.AddresseeType == "TO").ToList().Select(b => b.AspNetRoleId).ToArray();

                gnNotificationTopic.AddToRoles = selectedToRoles.Except(currentToRoles).ToArray();
                gnNotificationTopic.RemoveToRoles = currentToRoles.Except(selectedToRoles).ToArray();
            }

            if (!string.IsNullOrEmpty(Request["CcRolesList"]))
            {
                string[] selectedCcRoles = Request["CcRolesList"].Split(',');
                string[] currentCcRoles = db.GNNotificationTopicAddressees.Where(a => a.GNNotificationTopicId == gnNotificationTopic.Id && a.AddresseeType == "CC").ToList().Select(b => b.AspNetRoleId).ToArray();
                
                gnNotificationTopic.AddCcRoles = selectedCcRoles.Except(currentCcRoles).ToArray();
                gnNotificationTopic.RemoveCcRoles = currentCcRoles.Except(selectedCcRoles).ToArray();
            }

            if (!string.IsNullOrEmpty(Request["BccRolesList"]))
            {
                string[] selectedBccRoles = Request["BccRolesList"].Split(',');
                string[] currentBccRoles = db.GNNotificationTopicAddressees.Where(a => a.GNNotificationTopicId == gnNotificationTopic.Id && a.AddresseeType == "BCC").ToList().Select(b => b.AspNetRoleId).ToArray();

                gnNotificationTopic.AddBccRoles = selectedBccRoles.Except(currentBccRoles).ToArray();
                gnNotificationTopic.RemoveBccRoles = currentBccRoles.Except(selectedBccRoles).ToArray();
            }

            return await base.Edit(gnNotificationTopic);
        }

        public override ActionResult EditOnSuccess(GNNotificationTopic entity)
        {
            //update addressees
            NotificationTopicAddresseeService NotificationAddresseeService = new NotificationTopicAddresseeService(db);
            NotificationAddresseeService.Edit(entity);

            //update subscribers
            //to do

            return RedirectToAction("Details", new{ id = entity.Id });
        }
        
        public async Task<ActionResult> SendSample()
        {
            int NotificationTopicId = Int32.Parse(Request["Id"]);
            string Email = Request["EmailForSample"];

            GNNotificationTopic Topic = db.GNNotificationTopics.Where(a => a.Id == NotificationTopicId).FirstOrDefault();
            GNCloudEmailService email = new GNCloudEmailService();
            email.SendSampleEmail(Topic, Email);

            return RedirectToAction("Details", new { id = Request["Id"], SampleSent = "1", SampleEmail = Email });
            //return await base.Details(Request["Id"]);
        }

        public override async Task<ActionResult> Details(string id)
        {
            int Id = Int32.Parse(id);
            GNNotificationTopic notificationTopic = db.GNNotificationTopics.Where(t => t.Id == Id).FirstOrDefault();
                       
            List<string> ToAddressees = notificationTopic.GNNotificationTopicAddressees.Where(t => t.AddresseeType.Equals("TO")).Select(a => a.AspNetRoleId).ToList();
            ViewBag.SelectedToRolesList = new SelectList(identityDB.AspNetRoles.Where(r => ToAddressees.Contains(r.Id)).Select(a => a.Name));

            List<string> CcAddressees = notificationTopic.GNNotificationTopicAddressees.Where(t => t.AddresseeType.Equals("CC")).Select(a => a.AspNetRoleId).ToList();
            ViewBag.SelectedCcRolesList = new SelectList(identityDB.AspNetRoles.Where(r => CcAddressees.Contains(r.Id)).Select(a => a.Name));

            List<string> BccAddressees = notificationTopic.GNNotificationTopicAddressees.Where(t => t.AddresseeType.Equals("BCC")).Select(a => a.AspNetRoleId).ToList();
            ViewBag.SelectedBccRolesList = new SelectList(identityDB.AspNetRoles.Where(r => BccAddressees.Contains(r.Id)).Select(a => a.Name));

            ViewBag.SenderName = db.GNNotificationSenders.Where(a => a.Sender.Equals(notificationTopic.Sender)).Select(b => b.Name).FirstOrDefault();
            
            return await base.Details(id);
        }

        public override GNNotificationTopic PopulateSelectLists(GNNotificationTopic notificationTopic = null)
        {
            notificationTopic = base.PopulateSelectLists(notificationTopic);

            ViewBag.Sender = new SelectList(db.GNNotificationSenders.OrderBy(a => a.Name).ToList(), "Sender", "Name", (notificationTopic != null ? notificationTopic.Sender : null));
            
            string[] selectedToRoles = null;
            string[] selectedCcRoles = null;
            string[] selectedBccRoles = null;

            if (notificationTopic!=null)
            {
                selectedToRoles = notificationTopic.GNNotificationTopicAddressees.Where(r => r.AddresseeType.Trim().Equals("TO")).Select(a => a.AspNetRoleId).ToArray();
                selectedCcRoles = notificationTopic.GNNotificationTopicAddressees.Where(r => r.AddresseeType.Trim().Equals("CC")).Select(a => a.AspNetRoleId).ToArray();
                selectedBccRoles = notificationTopic.GNNotificationTopicAddressees.Where(r => r.AddresseeType.Trim().Equals("BCC")).Select(a => a.AspNetRoleId).ToArray();
            }
            
            List<AspNetRole> AspRoles = identityDB.AspNetRoles.OrderBy(a => a.Name).ToList();

            ViewBag.ToRolesList = new MultiSelectList(AspRoles, "Id", "Name", selectedToRoles);
            ViewBag.CCRolesList = new MultiSelectList(AspRoles, "Id", "Name", selectedCcRoles);
            ViewBag.BCCRolesList = new MultiSelectList(AspRoles, "Id", "Name", selectedBccRoles);
   
            return notificationTopic;
        }


    }

}

