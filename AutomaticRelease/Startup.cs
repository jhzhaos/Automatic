using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutomaticRelease.Startup))]
namespace AutomaticRelease
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
