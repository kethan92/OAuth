using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using WebApplication6.Entities;

namespace WebApplication6
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();


            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            //builder.EntitySet<Product>("Product");
            //builder.EntitySet<Category>("Category");
            builder.EntitySet<Login>("Logins");
            /*
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            ); */

            config.MapODataServiceRoute(
            routeName: "odata",
            routePrefix: "api",
            model: builder.GetEdmModel());


            //config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            /* now the default setting for WebAPI OData is:
            client can’t apply $count, $orderby, $select, $top, $expand, $filter in the query, query
            like localhost\odata\Customers?$orderby=Name will failed as BadRequest,
            because all properties are not sort-able by default, this is a breaking change in 6.0.0
            So, we now need to enable OData Model Bound Attributes
            */
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            // WebAPI when dealing with JSON & JavaScript!  
            // Setup json serialization to serialize classes to camel (std. Json format)  
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            // Adding JSON type web api formatting.  
            config.Formatters.Clear();
            config.Formatters.Add(formatter);
        }
        /*
        public static Microsoft.OData.Edm.IEdmModel GetModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            //builder.EntitySet<Product>("Product");
            //builder.EntitySet<Category>("Category");
            builder.EntitySet<Login>("Login");

            return builder.GetEdmModel();
        }
        */
    }
}
