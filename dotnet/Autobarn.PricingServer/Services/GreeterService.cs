using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Autobarn.PricingEngine;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer
{
    public class PricerService : Pricer.PricerBase
    {
        private readonly ILogger<PricerService> _logger;
        public PricerService(ILogger<PricerService> logger)
        {
            _logger = logger;
        }

        public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context)
        {
            var reply = new PriceReply {
                Price = 12345,
                CurrencyCode = "EUR"
            };
            return Task.FromResult(reply);
        }
    }
}
