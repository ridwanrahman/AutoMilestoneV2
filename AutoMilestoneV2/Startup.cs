using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoMilestoneV2.Startup))]
namespace AutoMilestoneV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
