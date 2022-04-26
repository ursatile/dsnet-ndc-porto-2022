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
            var handler = MakeHandler(bus);
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>("autobarn.auditlog", handler);
            using var channel = GrpcChannel.ForAddress(config["AutobarnPricingServerUrl"]);
            grpcClient = new Pricer.PricerClient(channel);
            Console.WriteLine("Listening for NewVehicleMessages - press Ctrl-C to quit");
            while (true) {
                Console.ReadLine();
                var test = new NewVehicleMessage {
                    Manufacturer = "DMC",
                    Model = "Delorean",
                    Year = 1985,
                    Color = "Silver"
                };
                handler(test);  
            }
        }

        private static Action<NewVehicleMessage> MakeHandler(IBus bus) {
            return async (NewVehicleMessage message) => {
                Console.WriteLine($"Checking pricer for {message.Year} {message.Manufacturer} {message.Model} ({message.Color})");
                var request = new PriceRequest {
                    Color = message.Color,
                    Manufacturer = message.Manufacturer,
                    Model = message.Model,
                    Year = message.Year
                };
                var priceReply = await grpcClient.GetPriceAsync(request);
                Console.WriteLine($"Price: {priceReply.Price} {priceReply.CurrencyCode}");
                var nvpm = NewVehiclePriceMessage.FromNewVehicleMessage(message, priceReply.Price, priceReply.CurrencyCode);
                await bus.PubSub.PublishAsync(nvpm);
            };
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

