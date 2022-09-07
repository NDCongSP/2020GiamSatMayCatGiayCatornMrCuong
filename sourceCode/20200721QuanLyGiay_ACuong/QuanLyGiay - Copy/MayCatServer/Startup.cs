using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System.Diagnostics;
using System.Net;

namespace MayCatServer
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {   
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
            GlobalHost.Configuration.DefaultMessageBufferSize = 32;
            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = 20 * 1024;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            Debug.WriteLine("Start");   
        }
    }
}
