using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class ManufacturerGraphType : ObjectGraphType<Manufacturer> {
        public ManufacturerGraphType() {
            Name = "manufacturer";
            Field(c => c.Name).Description("The name of this manufacturer, eg. BMW, Ford, Audi");
        }
    }
}
