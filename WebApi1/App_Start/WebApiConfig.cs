using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using WebApi1.Models;

namespace WebApi1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //config.Formatters.Add(new BinaryMediaTypeFormatter());
            // Web API routes

            var provider = new SimpleModelBinderProvider(typeof(byte[]), new ModelBinderWithBase64Arrays());
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
