using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class ModelGraphType : ObjectGraphType<Model> {
        public ModelGraphType() {
            Name = "model";
            Field(m => m.Name).Description("The name of this model, eg 500, Focus, Quattro");
            Field(m => m.Manufacturer, nullable: false, type: typeof(ManufacturerGraphType))
                .Description("The manufacturer who makes this model of car");
        }
    }
}
