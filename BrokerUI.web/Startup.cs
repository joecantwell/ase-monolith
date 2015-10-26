using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BrokerUI.web.Startup))]
namespace BrokerUI.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
