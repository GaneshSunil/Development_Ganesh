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
    public class InviteCodeService : GNEntityService<GNInviteCode>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public InviteCodeService(GNEntityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<GNInviteCode>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            List<GNInviteCode> entities =
                await db.GNInviteCodes
                .ToListAsync();

            return entities;
        }

        public override async Task<GNInviteCode> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.GNInviteCodes.FindAsync(keys);
        }

        public bool ValidateInviteCode(string inviteCode)
        {
            bool isValid = false;

            GNInviteCode inviteCodeObj = db.GNInviteCodes.Find(inviteCode);

            if(inviteCodeObj != null)
            {
                if (!inviteCodeObj.UseMaxAllowed.HasValue && !inviteCodeObj.ExpireDate.HasValue)
                {
                    isValid = true;
                }
                else if (!inviteCodeObj.ExpireDate.HasValue 
                    && (inviteCodeObj.UseMaxAllowed.HasValue && inviteCodeObj.UseMaxAllowed.Value > inviteCodeObj.UseCount))
                {
                    isValid = true;
                }
                else if (!inviteCodeObj.UseMaxAllowed.HasValue 
                    && (inviteCodeObj.ExpireDate.HasValue && inviteCodeObj.ExpireDate.Value.CompareTo(DateTime.Now) > 0))
                {
                    isValid = true;
                }
                else if ((inviteCodeObj.UseMaxAllowed.HasValue && inviteCodeObj.UseMaxAllowed.Value > inviteCodeObj.UseCount)
                    && (inviteCodeObj.ExpireDate.HasValue && inviteCodeObj.ExpireDate.Value.CompareTo(DateTime.Now) > 0))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        public GNInviteCode RedeemInviteCode(string inviteCode)
        {
            GNInviteCode inviteCodeObj = null;

            bool isValid = ValidateInviteCode(inviteCode);

            if(isValid)
            {
                inviteCodeObj = db.GNInviteCodes.Find(inviteCode);

                inviteCodeObj.UseCount++;

                int result = db.SaveChanges();

                if (result == 0)
                {
                    inviteCodeObj = null;
                }
            }

            return inviteCodeObj;
        }

        public void UnRedeemInviteCode(string inviteCode)
        {
            GNInviteCode inviteCodeObj = db.GNInviteCodes.Find(inviteCode);

            inviteCodeObj.UseCount--;

            db.SaveChanges();
        }
    }
}