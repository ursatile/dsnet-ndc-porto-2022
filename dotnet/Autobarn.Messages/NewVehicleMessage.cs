using System;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Registration { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public DateTimeOffset ListedAt { get; set; }
    }

    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }

        public static NewVehiclePriceMessage FromNewVehicleMessage(NewVehicleMessage nvm, int price, string currencyCode) {
            var result = new NewVehiclePriceMessage {
                Manufacturer = nvm.Manufacturer,
                Model = nvm.Model,
                Color = nvm.Color,
                Year = nvm.Year,
                Price = price,
                CurrencyCode = currencyCode,
                Registration = nvm.Registration,
                ListedAt = nvm.ListedAt
            };
            return result;
        }
    }
}