using Microsoft.AspNetCore.Mvc;

namespace Eventra.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(new {
            name = "Eventra API",
            message = "Welcome to the Eventra REST API!",
            swagger = "/swagger"
        });
    }
}
