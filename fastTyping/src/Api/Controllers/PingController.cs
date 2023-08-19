using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    public PingController()
    {
    }

    [HttpGet]
    public IActionResult Pong()
    {
        return Ok("Pong");
    }
}
