using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
//JLS: These do not appear to be needed.
//using Microsoft.Owin.Security.DataProtection;
//using Microsoft.Owin.Security.Google;
using Owin;
using System;
using GenomeNext.Portal.Models;
using GenomeNext.Data.IdentityModel;
using GenomeNext.App;



//JLS: Using statements required for new code.
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.WsFederation;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


//TF
using System.Linq;
using System.Security.Claims;

using GenomeNext.Portal.Models;
using GenomeNext.Data.IdentityModel;
using GenomeNext.Data.EntityModel;

namespace GenomeNext.Portal
{
    public partial class Startup
    {

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(GNIdentityDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // JLS: Ensure that, by default, the system will authenticate requests with our configured CookieAuthentication.
            app.SetDefaultSignInAsAuthenticationType(DefaultAuthenticationTypes.ApplicationCookie);


            //JLS: ***********************************************

            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    AuthenticationType = "Sanford",
                    Wtrealm = ConfigurationManager.AppSettings["Sanford:Wtrealm"],
                    MetadataAddress = ConfigurationManager.AppSettings["Sanford:Metadata"],
                    Notifications = new WsFederationAuthenticationNotifications
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            //JLS: ToDo: Replace this target of this redirect with the appropriate URL for the GenomeNext application.
                            context.Response.Redirect("Home/Error?message=" + context.Exception.Message);
                            return Task.FromResult(-1);
                        },
                        //SecurityTokenReceived
                        SecurityTokenValidated = context =>
                        {
                            var userEmail = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.Email).Value;

                            var identityDB = new IdentityModelContainer();
                            var db = new GNEntityModelContainer();
                            GNContact contact;

                            AspNetUser user = identityDB.AspNetUsers.Where(a => a.Email.ToLower().Equals(userEmail.ToLower())).FirstOrDefault();
                            if (user != null)
                            {
                                //exists
                                contact = db.GNContacts.Where(a => a.User.Id.Equals(user.Id) && a.IsInviteAccepted == true).FirstOrDefault();
                            }
                            else
                            {
                                //create new
                                String ssoEmail = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.Email).Value;
                                GNOrganization organization = db.GNOrganizations.Where(a => a.Name.Contains("Sanford")).FirstOrDefault();
                                AspNetUser newUser = new AspNetUser
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Email = ssoEmail,
                                    IsAdmin = false,
                                    Password = "SanfordSSO1!",
                                    UserName = ssoEmail,
                                    EmailConfirmed = true
                                };
                                identityDB.AspNetUsers.Add(newUser);

                                contact = new GNContact
                                {
                                    Id = Guid.NewGuid(),
                                    AspNetUserId = newUser.Id,
                                    CreateDateTime = DateTime.Now,
                                    Email = ssoEmail,
                                    FirstName = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.GivenName).Value
                                };

                                db.GNContacts.Add(contact);
                                identityDB.SaveChanges();
                                db.SaveChanges();
                            }


                            var applicationUserIdentity = new ClaimsIdentity();
                            applicationUserIdentity.AddClaim(new Claim(ClaimTypes.Name, contact.FullName, ""));
                            applicationUserIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email, ""));
                            context.OwinContext.Authentication.User.AddIdentity(applicationUserIdentity);

                            context.Response.Redirect("Home");
                            return Task.FromResult(0);
                        }
                    }
                }
            );
            //JLS: ***********************************************


            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

        }
    }
}