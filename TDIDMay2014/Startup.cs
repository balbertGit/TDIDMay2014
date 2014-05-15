using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TDIDMay2014.Startup))]
namespace TDIDMay2014
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
