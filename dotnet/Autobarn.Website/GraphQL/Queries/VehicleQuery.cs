using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autobarn.Website.GraphQL.Queries {
    public class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;

        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;

            Field<ListGraphType<VehicleGraphType>>("Vehicles",
                "Query that retrieves all vehicles",
                resolve: GetAllVehicles);

            var vehicleArg = MakeNonNullStringArgument("registration", "The registration plate of the vehicle you want");
            Field<VehicleGraphType>("Vehicle", "Query to retrieve a single vehicle",
                new QueryArguments(vehicleArg),
                resolve: GetVehicle);

            Field<ListGraphType<VehicleGraphType>>("VehiclesByColor", "Query to retrieve all Vehicles matching the specified color",
                new QueryArguments(MakeNonNullStringArgument("color", "The name of a color, eg 'blue', 'grey'")),
                resolve: GetVehiclesByColor);

        }

        private IEnumerable<Vehicle> GetVehiclesByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            var vehicles = db.ListVehicles()
                .Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
            return vehicles;

        }

        private Vehicle GetVehicle(IResolveFieldContext<object> context) {
            var registration = context.GetArgument<string>("registration");
            return db.FindVehicle(registration);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }


        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg) {
            return db.ListVehicles();
        }
    }
}
