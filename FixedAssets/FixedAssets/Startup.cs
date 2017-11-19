using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FixedAssets.Startup))]
namespace FixedAssets
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
