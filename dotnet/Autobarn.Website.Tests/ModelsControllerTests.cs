using Autobarn.Data;
using Autobarn.Messages;
using Autobarn.Website.Controllers.api;
using EasyNetQ;
using EasyNetQ.Internals;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Autobarn.Website.Tests {
    public class ModelsControllerTests {
        [Fact]
        public void POST_Publishes_Details_To_Bus() {
            var bus = new FakeBus();
            var db = new AutobarnCsvFileDatabase(new NullLogger<AutobarnCsvFileDatabase>());
            var c = new ModelsController(db, bus);
            c.Post("abarth-124", new Models.VehicleDto {
                Registration = "test1234",
                Year = 2021,
                Color = "Blue"
            });
            var messages = ((FakePubSub)bus.PubSub).Messages;
            messages.Count.ShouldBe(1);
            var nvm = messages[0] as NewVehicleMessage;
            nvm.ShouldNotBeNull();
            nvm.Registration.ShouldBe("TEST1234");
        }
    }

    public class FakeBus : IBus {
        private IPubSub pubSub = new FakePubSub();

        public IPubSub PubSub => pubSub;

        public IRpc Rpc => throw new NotImplementedException();

        public ISendReceive SendReceive => throw new NotImplementedException();

        public IScheduler Scheduler => throw new NotImplementedException();

        public IAdvancedBus Advanced => throw new NotImplementedException();

        public void Dispose() {
            throw new NotImplementedException();
        }
    }

    public class FakePubSub : IPubSub {
        public ArrayList Messages { get; set; } = new ArrayList();
        public Task PublishAsync<T>(T message, Action<IPublishConfiguration> configure, CancellationToken cancellationToken = default) {
            Messages.Add(message);
            return Task.CompletedTask;
        }

        public AwaitableDisposable<ISubscriptionResult> SubscribeAsync<T>(string subscriptionId, Func<T, CancellationToken, Task> onMessage, Action<ISubscriptionConfiguration> configure, CancellationToken cancellationToken = default) {
            throw new NotImplementedException();
        }
    }
}