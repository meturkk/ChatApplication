﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChatApplication.Startup))]

namespace ChatApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // SignalR'ı başlat
            app.MapSignalR();
        }
    }
}
