using Api.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class PingController : ApiController
{
    public PingController()
    {
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Pong()
    {
        return Ok("Pong");
    }

    [HttpGet("auth")]
    public IActionResult PongAuth()
    {
        return Ok("Pong auth");
    }
}
