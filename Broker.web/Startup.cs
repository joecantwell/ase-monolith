using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Broker.web.Startup))]
namespace Broker.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
