using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GenomeNext.Portal.Startup))]
namespace GenomeNext.Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
