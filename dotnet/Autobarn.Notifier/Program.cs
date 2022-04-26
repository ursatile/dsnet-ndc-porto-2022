using Autobarn.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Autobarn.Notifier {
    internal class Program {
		private static HubConnection hub;

		static async Task Main(string[] args) {
			
			JsonConvert.DefaultSettings = JsonSettings;

			var config = ReadConfiguration();
			var amqp = config.GetConnectionString("AutobarnRabbitMq"); ;
			var bus = RabbitHutch.CreateBus(amqp);
			hub = new HubConnectionBuilder().WithUrl(config["AutobarnSignalRHubUrl"]).Build();
			await hub.StartAsync();
			Console.WriteLine("Hub started - connected to SignalR!");
			bus.PubSub.Subscribe<NewVehiclePriceMessage>("autobarn.notifier", HandleNewVehiclePriceMessage);
			Console.WriteLine("Listening for NewVehiclePriceMessages. Press enter to quit.");
			Console.ReadLine();	
        }

        private static async Task HandleNewVehiclePriceMessage(NewVehiclePriceMessage nvm) {
			Console.WriteLine($"Got a new vehicle with reg: {nvm.Registration} ({nvm.Manufacturer} {nvm.Model}, {nvm.Color}, {nvm.Year}) - price {nvm.Price} {nvm.CurrencyCode}");
			var json = JsonConvert.SerializeObject(nvm);
			await hub.SendAsync("NotifyAllWebUsersAboutNewThing", "Autobarn.Notifier", json); 
		}

        private static IConfigurationRoot ReadConfiguration() {
			var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
			return new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();
		}

		private static JsonSerializerSettings JsonSettings() =>
			new JsonSerializerSettings {
				ContractResolver = new DefaultContractResolver {
					NamingStrategy = new CamelCaseNamingStrategy()
				}
			};
	}
}
