using Microsoft.AspNetCore.Mvc;

namespace ElectronicVoting.Admin.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    /// <summary>
    /// Testuje API.
    /// </summary>
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Test API działa poprawnie");
    }
}