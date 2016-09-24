using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tournaments.Startup))]
namespace Tournaments
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
