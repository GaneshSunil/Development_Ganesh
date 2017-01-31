using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.WsFederation;
using Owin;

[assembly: OwinStartup(typeof(GenomeNext.Portal.OwinStartup))]

namespace GenomeNext.Portal
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureAuth(app);
        }


        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(
            new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType
            });
            app.UseWsFederationAuthentication(
            new WsFederationAuthenticationOptions
            {
                MetadataAddress = "https://login.microsoftonline.com/2c2a63d4-465c-464d-b7f5-6ccfd0dce5b8/federationmetadata/2007-06/federationmetadata.xml",
                //MetadataAddress = "https://shadfs.sanfordhealth.org/adfs/ls/ldpinitiatedsignon.aspx",
                Wtrealm = "https://secure.genomenext.net/Sso/Login"
            });

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}

