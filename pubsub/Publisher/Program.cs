using EasyNetQ;
using Messages;

const string AMQP = "amqps://geehsxvk:xZw6UqdzJVel3fXTPaIVKGcxWhAcp7es@sparrow.rmq.cloudamqp.com/geehsxvk";
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var count = 0;
using var bus = RabbitHutch.CreateBus(AMQP);
while (true)
{

    Console.WriteLine("Press a key to publish a message");
    var content = $"Hey everybody! This is message {count++}";
    var greeting = new Greeting
    {
        Machine = Environment.MachineName,
        Content = content,
        Language = "en-GB",
        Published = DateTimeOffset.UtcNow,
    };
    await bus.PubSub.PublishAsync(greeting);
    Console.WriteLine($"Published: {content}");
    Console.ReadKey();
}
