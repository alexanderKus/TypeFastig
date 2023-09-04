using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Common;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApiController : ControllerBase
{
    public ApiController()
    {
    }

    //protected IActionResult Problem()
    //{
    //    throw new NotImplementedException();
    //}
}
