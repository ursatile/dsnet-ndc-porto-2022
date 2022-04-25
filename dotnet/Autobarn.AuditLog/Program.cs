using Autobarn.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Autobarn.AuditLog {
    internal class Program {
        static void Main(string[] args) {
			var config = ReadConfiguration();
			var amqp = config.GetConnectionString("AutobarnRabbitMq"); ;
			var bus = RabbitHutch.CreateBus(amqp);
			bus.PubSub.Subscribe<NewVehicleMessage>("autobarn.auditlog", HandleNewVehicleMessage);
			Console.WriteLine("Listening for NewVehicleMessages. Press enter to quit.");
			Console.ReadLine();	
        }

        private static void HandleNewVehicleMessage(NewVehicleMessage nvm) {
			Console.WriteLine($"Got a new vehicle with reg: {nvm.Registration} ({nvm.Manufacturer} {nvm.Model}, {nvm.Color}, {nvm.Year})");
		}

        private static IConfigurationRoot ReadConfiguration() {
			var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
			return new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();
		}
	}
}
