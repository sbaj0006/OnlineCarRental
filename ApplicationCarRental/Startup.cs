using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ApplicationCarRental.Startup))]
namespace ApplicationCarRental
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
