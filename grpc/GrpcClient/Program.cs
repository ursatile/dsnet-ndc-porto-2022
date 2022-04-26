// See https://aka.ms/new-console-template for more information
using System.Threading.Tasks;
using GrpcGreeter;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress("https://localhost:7036/");
var client = new Greeter.GreeterClient(channel);
var count = 0;
Console.WriteLine("Press Enter to send a gRPC request");
while (true)
{
    var reply = await client.SayHelloAsync(new HelloRequest
    {
        Name = $"NDC Porto {count++}"
    });
    Console.WriteLine(reply.Message);
    Console.ReadLine();
}

