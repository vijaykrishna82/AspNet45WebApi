﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using EventWebApi;
using EventWebApi.Controllers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;


[assembly: OwinStartup("ApiConfiguration", typeof(OwinStartup))]
namespace EventWebApi
{

    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // Owin Middleware; we use token middleware for requests that require authorization.
            var oAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            app.UseOAuthBearerAuthentication(oAuthBearerOptions);

            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            json.SerializerSettings.Formatting = Formatting.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(typeof(EventsController).Assembly);
            var container = builder.Build();

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
        }
    }
}