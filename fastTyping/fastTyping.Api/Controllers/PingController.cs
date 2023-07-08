using System;
using Microsoft.AspNetCore.Mvc;

namespace fastTyping.Api.Controllers
{
	[Route("/api/[controller]")]
	public class PingController : ControllerBase
	{
		public PingController()
		{
		}

		[HttpGet]
		public ActionResult Ping()
		{
			return Ok("pong");
		}
	}
}

