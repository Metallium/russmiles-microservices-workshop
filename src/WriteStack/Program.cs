﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using Consul;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Serialization;
using Owin;

namespace WriteStack
{
	class Program
	{
		static void Main(string[] args)
		{
			var serviceId = "pepelazzo";
			var serviceType = "WriteStack";
			var bindUri = new Uri("http://127.0.0.1:20001");

			using (Connect(bindUri))
			{
				Console.WriteLine(serviceType);
				RegisterInConsul(bindUri, serviceId, serviceType).GetAwaiter().GetResult();
				Console.ReadKey();
			}
		}

		public static IDisposable Connect(Uri bindUri) => WebApp.Start(bindUri.AbsoluteUri, app =>
		{
			var configuration = new HttpConfiguration();
			configuration.MapHttpAttributeRoutes();
			configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);
			configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			configuration.Routes.MapHttpRoute("root", "{controller}/{action}", new {controller = "Home", action = "AssignPerson"});

			app.UseWebApi(configuration);
		});

		public static async Task RegisterInConsul(Uri bindUri, string serviceId, string serviceType)
		{
			using (var client = new ConsulClient())
			{
				await client.Catalog.Register(new CatalogRegistration
				{
					Address = bindUri.AbsoluteUri,
					Node = serviceId,
					Service = new AgentService
					{
						Address = bindUri.Host,
						ID = serviceId + "-" + serviceType,
						Port = bindUri.Port,
						Service = serviceType
					}
				});
			}
		}
	}
}
