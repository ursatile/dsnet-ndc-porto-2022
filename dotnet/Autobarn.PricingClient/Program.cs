using Grpc.Net.Client;
using System;
using Autobarn.PricingEngine;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using EasyNetQ;
using Autobarn.Messages;

namespace Autobarn.PricingClient {
    class Program {
        static Pricer.PricerClient grpcClient;
        static async Task Main(string[] args) {
            var config = ReadConfiguration();
            var amqp = config.GetConnectionString("AutobarnRabbitMq"); ;
            var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>("autobarn.auditlog", HandleNewVehicleMessage);
            using var channel = GrpcChannel.ForAddress(config["AutobarnPricingServerUrl"]);
            grpcClient = new Pricer.PricerClient(channel);
            Console.WriteLine("Listening for NewVehicleMessages - press Enter to quit");
            Console.ReadLine();
        }

        private static async void HandleNewVehicleMessage(NewVehicleMessage message) {
            Console.WriteLine($"Checking pricer for {message.Year} {message.Manufacturer} {message.Model} ({message.Color})");
            var request = new PriceRequest {
                Color = message.Color,
                Manufacturer = message.Manufacturer,
                Model = message.Model,
                Year = message.Year
            };
            var priceReply = await grpcClient.GetPriceAsync(request);
            Console.WriteLine($"Price: {priceReply.Price} {priceReply.CurrencyCode}");
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

