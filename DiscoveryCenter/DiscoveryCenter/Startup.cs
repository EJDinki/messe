using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DiscoveryCenter.Startup))]
namespace DiscoveryCenter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
  
        }
    }
}
