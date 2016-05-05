using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Consul;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Serialization;
using Owin;

namespace WebStack
{
	class Program
	{
		public static IDisposable Connect(Uri bindUri) => WebApp.Start(bindUri.AbsoluteUri, app =>
		{
			var configuration = new HttpConfiguration();
			configuration.MapHttpAttributeRoutes();
			configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);
			configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			app.UseWebApi(configuration);
		});

		static void Main(string[] args)
		{
			var serviceId = "kc";
			var serviceType = "WebStack";
			var bindUri = new Uri("http://127.0.0.1:20000");

			Run(bindUri, serviceType, serviceId).Wait();
		}

		private static async Task Run(Uri bindUri, string serviceType, string serviceId)
		{
			using (Connect(bindUri))
			{
				Console.WriteLine(serviceType);
				RegisterInConsul(bindUri, serviceId, serviceType).GetAwaiter().GetResult();
				string input = null;
				while (input != "exit")
				{
					using (var consulClient = new ConsulClient())
					{
						await AssignPersonToUserStory(consulClient);
						await RetrieveUserStories(consulClient);
					}
					input = Console.ReadLine();
				}
			}
		}

		private static async Task AssignPersonToUserStory(ConsulClient consulClient)
		{
			var writeStack = await consulClient.Catalog.Service("WriteStack");

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.PostAsJsonAsync(writeStack.Response.First().Address, new AssignPersonCommand
				{
					PersonId = new PersonId(Guid.NewGuid()),
					UserStoryId = new UserStoryId(Guid.NewGuid())
				});

				Console.WriteLine(response.StatusCode);
			}
		}

		private static async Task RetrieveUserStories(ConsulClient consulClient)
		{
			var writeStack = await consulClient.Catalog.Service("ReadStack");

			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(writeStack.Response.First().Address);

				Console.WriteLine(response.Content);
			}
		}

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
