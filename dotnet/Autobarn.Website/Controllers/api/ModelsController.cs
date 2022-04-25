using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Autobarn.Website.Controllers.api {
	[Route("api/[controller]")]
	[ApiController]
	public class ModelsController : ControllerBase {
		private readonly IAutobarnDatabase db;
        private readonly IBus bus;

        public ModelsController(IAutobarnDatabase db, IBus bus) {
			this.db = db;
            this.bus = bus;
        }

		[HttpGet]
		public IActionResult Get() {
			var result = db.ListModels().Select(model => model.ToResource());
			return Ok(result);
		}

		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var vehicleModel = db.FindModel(id);
			if (vehicleModel == default) return NotFound();
			var result = vehicleModel.ToDynamic();
			result._actions = new {
				create = new {
					href = $"/api/models/{id}",
					type = "application/json",
					method = "POST",
					name = $"Create a new {vehicleModel.Manufacturer.Name} {vehicleModel.Name}"
				}
			};
			return Ok(result);
		}

		// POST api/models/{code}
		[HttpPost("{id}")]
		public IActionResult Post(string id, [FromBody] VehicleDto dto) {
			var existing = db.FindVehicle(dto.Registration);

			if (existing != default) return Conflict($"Sorry, there is already a vehicle with code {dto.Registration} listed for sale.");

			var vehicleModel = db.FindModel(id);
			if (vehicleModel == default) {
				return BadRequest($"Sorry - there is no model with with code {dto.ModelCode}");
			}

			var vehicle = new Vehicle {
				Registration = dto.Registration,
				Color = dto.Color,
				Year = dto.Year,
				VehicleModel = vehicleModel
			};
			db.CreateVehicle(vehicle);
			PublishNewVehicleNotification(vehicle);
			var result = vehicle.ToResource();
			return Created($"/api/vehicles/{vehicle.Registration}", result);
		}

        private void PublishNewVehicleNotification(Vehicle vehicle) {
			var message = new NewVehicleMessage {
				Manufacturer = vehicle.VehicleModel.Manufacturer.Name,
				Model = vehicle.VehicleModel.Name,
				Registration = vehicle.Registration,
				Color = vehicle.Color,
				ListedAt = DateTimeOffset.UtcNow,
				Year = vehicle.Year
			};
			bus.PubSub.Publish(message);                
        }
    }
}