using Microsoft.AspNetCore.Mvc;

namespace ApiBlog.Controllers
{
	[ApiController]
	[Route("")]
	public class HomeController : ControllerBase
	{
		[HttpGet("")]
		public IActionResult Get()
		{
			return Ok();
		}
	}
}