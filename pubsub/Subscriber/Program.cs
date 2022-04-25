using EasyNetQ;
using Messages;

const string AMQP = "amqps://geehsxvk:xZw6UqdzJVel3fXTPaIVKGcxWhAcp7es@sparrow.rmq.cloudamqp.com/geehsxvk";
// See https://aka.ms/new-console-template for more information
using var bus = RabbitHutch.CreateBus(AMQP);
await bus.PubSub.SubscribeAsync<Greeting>("ndcporto", 
    greeting => Console.WriteLine(greeting.Content)
);
Console.ReadLine();
