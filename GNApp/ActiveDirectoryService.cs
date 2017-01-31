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
//using System.DirectoryServices;
//using Microsoft.IdentityModel.Protocols; //.WSTrust;
//using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
//using System.IdentityModel.Protocols.WSTrust;

using Thinktecture.IdentityModel.WSTrust;
using Thinktecture.IdentityModel.Extensions;
using System.Security.Cryptography.X509Certificates;

//////////////////////////

using Newtonsoft.Json;
using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.WSTrust;
using System.IdentityModel.Tokens;
using JWT;


//////////////////////////

namespace GenomeNext.App
{
    public class ActiveDirectoryService : GNEntityService<GNContact>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private GNCloudNoSQLService audit = new GNCloudNoSQLService();
        private readonly string ENTITY = "ACTIVE_DIRECTORY";

        public AspNetRoleService aspNetRoleService { get; set; }
        public AspNetUserRoleService aspNetUserRolesService { get; set; }

        public ActiveDirectoryService(GNEntityModelContainer db, IdentityModelContainer identityDB)
        {
            base.db = db;

            this.aspNetRoleService = new AspNetRoleService(identityDB);
            this.aspNetUserRolesService = new AspNetUserRoleService(identityDB);
        }

        /**
         * tfrege 2016.08.04
         */
        public bool SingleSignOn()
        {

            /*
            string stsEndpoint = "https://WIN-2013.win2008.marz.com/adfs/services/trust/13/usernamemixed";
            string relyingPartyUri = "https://www.yourrelyingpartyuri.com";

            WSTrustChannelFactory factory = new WSTrustChannelFactory(
            new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential),
            new EndpointAddress(stsEndpoint));

            factory.TrustVersion = TrustVersion.WSTrust13;

            // Username and Password here...
            factory.Credentials.UserName.UserName = "remote_user01";
            factory.Credentials.UserName.Password = "the_password";

            RequestSecurityToken rst = new RequestSecurityToken
            {
                RequestType = Microsoft.IdentityModel.Protocols.WSTrust.WSTrust13Constants.RequestTypes.Issue,
                AppliesTo = new EndpointAddress(relyingPartyUri),
                KeyType = Microsoft.IdentityModel.Protocols.WSTrust.WSTrust13Constants.KeyTypes.Bearer,
            };

            IWSTrustChannelContract channel = factory.CreateChannel();

            SecurityToken token = channel.Issue(rst);

            //if authentication is failed, exception will be thrown. Error is inside the innerexception.
            //Console.WriteLine("Token Id: " + token.Id);
            */

            return true;
        }


        public static void tokenTest()
        {
            string relyingPartyId = "https://shadfs.sanfordhealth.org/adfs/ls/ldpinitiatedsignon.aspx";
            WSTrustChannelFactory factory = null;
            try
            {
                // use a UserName Trust Binding for username authentication
                factory = new WSTrustChannelFactory(
                    new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                     new EndpointAddress("https://secure.genomenext.net/app/services/trust/13/usernamemixed"));
                        /////I'll change this endpoint this later////////

                factory.TrustVersion = TrustVersion.WSTrust13;

                factory.Credentials.UserName.UserName = "test";
                factory.Credentials.UserName.Password = "test";

                var rst = new RequestSecurityToken
                {
                    RequestType = RequestTypes.Issue,
                    AppliesTo = new EndpointReference(relyingPartyId),
                    KeyType = KeyTypes.Bearer
                };
                IWSTrustChannelContract channel = factory.CreateChannel();
                GenericXmlSecurityToken genericToken = channel.Issue(rst) as GenericXmlSecurityToken; //MessageSecurityException -> PW falsch

                var _handler = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection();
                var tokenString = genericToken.ToTokenXmlString();

                var samlToken2 = _handler.ReadToken(new XmlTextReader(new StringReader(tokenString)));

                ValidateSamlToken(samlToken2);

                X509Certificate2 certificate = null;

                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                certificate = store.Certificates.Find(X509FindType.FindByThumbprint, "thumb", false)[0];

              //  var jwt = ConvertSamlToJwt(samlToken2, "https://party.mycomp.com", certificate);

            }
            finally
            {
                if (factory != null)
                {
                    try
                    {
                        factory.Close();
                    }
                    catch (CommunicationObjectFaultedException)
                    {
                        factory.Abort();
                    }
                }
            }

                  private static void FederatedAuthentication_FederationConfigurationCreated(object sender, FederationConfigurationCreatedEventArgs e)
    {
        //from appsettings...
        const string allowedAudience = "http://audience1/user/get";
        const string rpRealm = "http://audience1/";
        const string domain = "";
        const bool requireSsl = false;
        const string issuer = "http://sts/token/create;
        const string certThumbprint = "mythumbprint";
        const string authCookieName = "StsAuth";

        var federationConfiguration = new FederationConfiguration();
                                 federationConfiguration.IdentityConfiguration.AudienceRestriction.AllowedAudienceUris.Add(new Uri(allowedAudience));

        var issuingAuthority = new IssuingAuthority(internalSts);
        issuingAuthority.Thumbprints.Add(certThumbprint);
        issuingAuthority.Issuers.Add(internalSts);
        var issuingAuthorities = new List<IssuingAuthority> {issuingAuthority};

        var validatingIssuerNameRegistry = new ValidatingIssuerNameRegistry {IssuingAuthorities = issuingAuthorities};
        federationConfiguration.IdentityConfiguration.IssuerNameRegistry = validatingIssuerNameRegistry;
        federationConfiguration.IdentityConfiguration.CertificateValidationMode = X509CertificateValidationMode.None;

        var chunkedCookieHandler = new ChunkedCookieHandler {RequireSsl = false, Name = authCookieName, Domain = domain, PersistentSessionLifetime = new TimeSpan(0, 0, 30, 0)};
        federationConfiguration.CookieHandler = chunkedCookieHandler;
        federationConfiguration.WsFederationConfiguration.Issuer = issuer;
        federationConfiguration.WsFederationConfiguration.Realm = rpRealm;
        federationConfiguration.WsFederationConfiguration.RequireHttps = requireSsl;

        e.FederationConfiguration = federationConfiguration;
                  }
        }

        /*
        public static TokenResponse ConvertSamlToJwt(SecurityToken securityToken, string scope, X509Certificate2 SigningCertificate)
        {
            var subject = ValidateSamlToken(securityToken);


            var descriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                AppliesToAddress = scope,
                SigningCredentials = new X509SigningCredentials(SigningCertificate),
                TokenIssuerName = "https://panav.mycomp.com",
                Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(10080))
            };

            
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwt = jwtHandler.CreateToken(descriptor);


            return new TokenResponse
            {
                AccessToken = jwtHandler.WriteToken(jwt),
                ExpiresIn = 10080
            };

        }*/


        public static ClaimsIdentity ValidateSamlToken(SecurityToken securityToken)
        {
            var configuration = new SecurityTokenHandlerConfiguration();
            configuration.AudienceRestriction.AudienceMode = AudienceUriMode.Never;
            configuration.CertificateValidationMode = X509CertificateValidationMode.None;
            configuration.RevocationMode = X509RevocationMode.NoCheck;
            configuration.CertificateValidator = X509CertificateValidator.None;

            var registry = new ConfigurationBasedIssuerNameRegistry();
            registry.AddTrustedIssuer("thumb", "ADFS Signing - mycomp.com");
            configuration.IssuerNameRegistry = registry;

            var handler = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(configuration);
            var identity = handler.ValidateToken(securityToken).First();
            return identity;
        }


    }

    public class TokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }


        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }


        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }


        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}
