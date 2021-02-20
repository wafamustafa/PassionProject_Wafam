using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpenseManager_WafaM.Startup))]
namespace ExpenseManager_WafaM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
