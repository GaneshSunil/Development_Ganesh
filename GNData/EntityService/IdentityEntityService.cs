using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenomeNext.Data
{
    public class IdentityEntityService<T> : BaseEntityService<T>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public new IdentityModelContainer db
        {
            get
            {
                LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
                return (IdentityModelContainer)base.db;
            }
            set
            {
                LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
                base.db = value;
            }
        }

        public IdentityEntityService()
            : base()
        {
        }

        public IdentityEntityService(IdentityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public bool IsUserContactAdmin(GNContact userContact)
        {
            return this.db.AspNetUserRoles.Count(r => (r.UserId == userContact.AspNetUserId && r.AspNetRole.Name == "GN_ADMIN")) != 0;
        }
    }

}
