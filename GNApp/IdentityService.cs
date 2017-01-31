using GenomeNext.Data;
using GenomeNext.Data.EntityModel;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
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

namespace GenomeNext.App
{
    /// <summary>
    /// AspNetUserService
    /// </summary>
    public class AspNetUserService : IdentityEntityService<AspNetUser>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ApplicationUserManager userManager { get; set; }

        public AspNetUserService(IdentityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<AspNetUser>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            IQueryable<AspNetUser> users = db.AspNetUsers.Include(u=>u.AspNetUserRoles);

            //Filter by Filter Keys
            if (filters != null && filters.Count != 0)
            {
                string filterVal = null;

                if (filters.ContainsKey("Username"))
                {
                    filterVal = (string)filters["Username"];
                    users = users.Where(u => u.UserName.Contains(filterVal));
                }

                if (filters.ContainsKey("Email"))
                {
                    filterVal = (string)filters["Email"];
                    users = users.Where(u =>u.Email.Contains(filterVal));
                }

                if (filters.ContainsKey("All"))
                {
                    filterVal = (string)filters["All"];
                    users = users
                        .Where(u => u.UserName.Contains(filterVal)
                            || u.Email.Contains(filterVal));
                }
            }

            //Order By Results
            users = users
                .OrderBy(u => u.Email)
                .OrderBy(u => u.UserName);

            //Limit Result Size
            users = users.Skip(start).Take(end - start);

            return await users.ToListAsync();
        }

        public override async Task<AspNetUser> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            string userId = (string)keys[0];
            return await db.AspNetUsers
                .Include(u=>u.AspNetUserRoles)
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
        }

        public override async Task<AspNetUser> Insert(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            //return await Task.Factory.StartNew<AspNetUser>(() =>
            //{
                ApplicationUser user = null;
                AspNetUser aspNetUser = base.db.AspNetUsers.SingleOrDefault(u => u.Email == ((AspNetUser)entity).Email);

                if (aspNetUser == null)
                {
                    user = new ApplicationUser() { UserName = ((AspNetUser)entity).Email, Email = ((AspNetUser)entity).Email };
                    string password = ((AspNetUser)entity).Password;
                    IdentityResult result = userManager.Create(user, password);

                    if (result.Succeeded)
                    {
                        //get user
                        aspNetUser = base.db.AspNetUsers.SingleOrDefault(u => u.Email == ((AspNetUser)entity).Email);

                        var tx = base.db.Database.BeginTransaction();

                        //save org
                        if (!string.IsNullOrEmpty(((AspNetUser)entity).DefaultOrganizationId))
                        {
                            aspNetUser.DefaultOrganizationId = ((AspNetUser)entity).DefaultOrganizationId;

                            base.db.Database.ExecuteSqlCommand(
                                "UPDATE [dbo].[AspNetUsers] SET [DefaultOrganizationId] = @orgId " +
                                "WHERE [Id] = @userId",
                                new SqlParameter("@userId", aspNetUser.Id),
                                new SqlParameter("@orgId", aspNetUser.DefaultOrganizationId));
                        }

                        //save user role ids
                        if (((AspNetUser)entity).RoleIds != null)
                        {
                            foreach (string roleId in ((AspNetUser)entity).RoleIds)
                            {
                                base.db.Database.ExecuteSqlCommand(
                                    "INSERT INTO [dbo].[AspNetUserRoles]([UserId],[RoleId]) " +
                                    "VALUES(@userId,@roleId)",
                                    new SqlParameter("@userId", user.Id),
                                    new SqlParameter("@roleId", roleId));
                            }
                        }

                        tx.Commit();

                        return aspNetUser;
                    }
                    else
                    {
                        string ListOfErrors = string.Join(", ", result.Errors); 
                        throw new Exception("User creation failed: " + ListOfErrors);
                    }
                }
                else
                {
                    throw new Exception("User already exists.");
                }
            //});
        }

        public override async Task<AspNetUser> Update(object entity)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await Task.Factory.StartNew<AspNetUser>(() =>
            {
                int result = 0;

                AspNetUser user = (AspNetUser)entity;

                base.db.Entry(user).State = EntityState.Modified;
                result = base.db.SaveChanges();

                if (result != 0)
                {
                    var tx = base.db.Database.BeginTransaction();

                    base.db.Database.ExecuteSqlCommand(
                        "DELETE FROM [dbo].[AspNetUserRoles] " +
                        "WHERE [UserId] = @userId",
                        new SqlParameter("@userId", user.Id));

                    if (user.RoleIds != null)
                    {
                        foreach (string roleId in user.RoleIds)
                        {
                            base.db.Database.ExecuteSqlCommand(
                                "INSERT INTO [dbo].[AspNetUserRoles]([UserId],[RoleId]) " +
                                "VALUES(@userId,@roleId)",
                                new SqlParameter("@userId", user.Id),
                                new SqlParameter("@roleId", roleId));
                        }
                    }

                    tx.Commit();
                }

                return user;
            });
        }

        public override async Task<int> Delete(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await Task.Factory.StartNew<int>(() =>
            {
                int result = 0;

                var tx = base.db.Database.BeginTransaction();

                result = base.db.Database.ExecuteSqlCommand(
                    "DELETE FROM [dbo].[AspNetUserRoles] " +
                    "WHERE [UserId] = @userId",
                    new SqlParameter("@userId", keys[0]));

                result = base.db.Database.ExecuteSqlCommand(
                    "DELETE FROM [dbo].[AspNetUsers] " +
                    "WHERE [Id] = @userId",
                    new SqlParameter("@userId", keys[0]));

                tx.Commit();

                return result;
            });
        }

        public override AspNetUser EvalEntitySecurity(GNContact userContact, AspNetUser user)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            if (user != null)
            {
                user.CanCreate = true;
                user.CanView = true;
                user.CanEdit = true;
                user.CanDelete = true;
            }

            return user;
        }
    }

    /// <summary>
    /// AspNetUserService
    /// </summary>
    public class AspNetRoleService : IdentityEntityService<AspNetRole>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AspNetRoleService(IdentityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<AspNetRole>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<AspNetRole> roles =
                await db.AspNetRoles
                .ToListAsync();

            return roles;
        }

        public override async Task<AspNetRole> Find(params object[] keys)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.AspNetRoles.FindAsync(keys);
        }
    }

    /// <summary>
    /// AspNetUserRolesService
    /// </summary>
    public class AspNetUserRoleService : IdentityEntityService<AspNetUserRoles>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AspNetUserRoleService(IdentityModelContainer db)
            : base(db)
        {
            base.db = db;
        }

        public override async Task<List<AspNetUserRoles>> FindAll(int start = 0, int end = 10, Dictionary<string, object> filters = null)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            List<AspNetUserRoles> aspNetUserRoles =
                await db.AspNetUserRoles
                .ToListAsync();

            return aspNetUserRoles;
        }

        public async Task<List<AspNetUserRoles>> FindByUserId(string userId)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            return await db.AspNetUserRoles.Where(au => au.UserId == userId).ToListAsync();
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<GNIdentityDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Task SendAsync(IdentityMessage message)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());

            MailUtil.SendEmail(message.Destination, message.Subject, message.Body);
            
            return Task.FromResult(0);
        }

    }

    public class SmsService : IIdentityMessageService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Task SendAsync(IdentityMessage message)
        {
            LogUtil.LogMethod(logger, MethodBase.GetCurrentMethod());
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }

}
