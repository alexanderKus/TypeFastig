using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Common;

public class ErrorsController : ControllerBase
{
    [Route("/errors")]
    public IActionResult Errors()
    {
        return Problem();
    }
}
