using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Autobarn.Website.Controllers.api {
    [Route("api")]
	[ApiController]
	public class HomeController : ControllerBase {
		[HttpGet]
		public IActionResult Get() {
			var result = new {
				message = "Welcome to the Autobarn API!",
				_links = new {
					vehicles = new {
						href = "/api/vehicles"
					},
					models = new {
						href = "/api/models",
					},
					manufacturers = new {
						href = "/api/manufacturers"
					}
				}
			};
			return Ok(result);
		}
	}
}
