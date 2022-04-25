﻿using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
	[ApiController]
	public class VehiclesController : ControllerBase {
		private readonly IAutobarnDatabase db;

		public VehiclesController(IAutobarnDatabase db) {
			this.db = db;
		}

		public const int DEFAULT_PAGE_SIZE = 32;
		// GET: api/vehicles
		[HttpGet]
		[Produces("application/hal+json")]
		public IActionResult Get(int index = 0, int count = DEFAULT_PAGE_SIZE) {
			var items = db.ListVehicles().Skip(index).Take(count).Select(v => v.ToResource());
			var total = db.CountVehicles();
			var result = new {
				_links = Hal.Paginate("/api/vehicles", index, count, total),
				total,
				count,
				index,
				items
			};
			return Ok(result);
		}


		// GET api/vehicles/ABC123
		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var vehicle = db.FindVehicle(id);
			if (vehicle == default) return NotFound();
			var result = vehicle.ToResource();
			return Ok(result);
		}


        // PUT api/vehicles/ABC123
        [HttpPut("{id}")]
		public IActionResult Put(string id, [FromBody] VehicleDto dto) {
			var vehicleModel = db.FindModel(dto.ModelCode);
			var vehicle = new Vehicle {
				Registration = dto.Registration,
				Color = dto.Color,
				Year = dto.Year,
				ModelCode = vehicleModel.Code
			};
			db.UpdateVehicle(vehicle);
			return Ok(dto);
		}

		// DELETE api/vehicles/ABC123
		[HttpDelete("{id}")]
		public IActionResult Delete(string id) {
			var vehicle = db.FindVehicle(id);
			if (vehicle == default) return NotFound();
			db.DeleteVehicle(vehicle);
			return NoContent();
		}
	}
}
