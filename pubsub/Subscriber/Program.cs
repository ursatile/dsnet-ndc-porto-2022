using EasyNetQ;
using Messages;

const string AMQP = "amqps://geehsxvk:xZw6UqdzJVel3fXTPaIVKGcxWhAcp7es@sparrow.rmq.cloudamqp.com/geehsxvk";
// See https://aka.ms/new-console-template for more information
using var bus = RabbitHutch.CreateBus(AMQP);
await bus.PubSub.SubscribeAsync<Greeting>($"ndcporto_{Environment.MachineName}", Handle);

void Handle(Greeting greeting)
{
    if (greeting.Content.EndsWith("5")) throw new Exception("We don't like 5!");
    Console.WriteLine($"{greeting.Content} (from {greeting.Machine} at {greeting.Published}");
}

Console.ReadLine();
