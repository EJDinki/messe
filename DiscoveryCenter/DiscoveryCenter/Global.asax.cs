using DiscoveryCenter.Migrations;
using DiscoveryCenter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DiscoveryCenter
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelBinders.Binders.Add(typeof(SurveyViewModel), new SurveyModelBinder());
            ModelBinders.Binders.Add(typeof(Survey), new SurveyModelBinder());
#if DEBUG
#else
            Database.SetInitializer<SurveyContext>(null);
#endif
        }
    }
}
