﻿using System;
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

using GenomeNext.Cloud.Messaging;

namespace GenomeNext.Portal.Controllers
{
    public class NotificationSuppressionListsController : GNEntityController<GNNotificationSuppressionList>
    {
        public NotificationSuppressionListsController()
            : base()
        {
            entityService = new NotificationSuppressionListsService(base.db);
        }        

    }
}

