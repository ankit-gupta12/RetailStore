using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SST.RS.StoreAdminGUI.Startup))]
namespace SST.RS.StoreAdminGUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
