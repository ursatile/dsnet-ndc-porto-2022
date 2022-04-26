using Grpc.Net.Client;
using System;
using Autobarn.PricingEngine;
using System.Threading.Tasks;

namespace Autobarn.PricingClient {
    class Program {
        static async Task Main(string[] args) {
            using var channel = GrpcChannel.ForAddress("https://workshop.ursatile.com:5003/");
            var client = new Pricer.PricerClient(channel);
            Console.WriteLine("Press Enter to send a gRPC request");
            while (true) {
                var reply = await client.GetPriceAsync(new PriceRequest());
                Console.WriteLine($"{reply.Price} {reply.CurrencyCode}");
                Console.ReadLine();
            }
        }
    }
}

