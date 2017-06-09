using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GPSServer.Startup))]
namespace GPSServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
        }
    }
}
