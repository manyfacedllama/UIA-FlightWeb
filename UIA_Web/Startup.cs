using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UIA_Web.Startup))]
namespace UIA_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
