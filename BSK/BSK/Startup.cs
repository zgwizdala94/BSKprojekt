using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BSK.Startup))]
namespace BSK
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
