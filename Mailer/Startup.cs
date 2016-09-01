using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mailer.Startup))]
namespace Mailer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
