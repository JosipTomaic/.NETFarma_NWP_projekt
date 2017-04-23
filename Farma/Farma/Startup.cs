using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Farma.Startup))]
namespace Farma
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
