using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_DMD.Startup))]
namespace Project_DMD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
