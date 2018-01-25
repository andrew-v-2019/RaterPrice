using RaterPrice.Api;
using Microsoft.Owin;
using Owin;
using RaterPrice.Api.Infrastructure;

[assembly: OwinStartup(typeof (Startup))]

namespace RaterPrice.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
          
        }
    }
}