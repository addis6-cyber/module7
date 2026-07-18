using Microsoft.AspNetCore.Mvc;

namespace TmsApi.Controllers.V1;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        //Adding Deprecation
        Response.Headers.Append("Deprecation", "true");

        Response.Headers.Append(
            "Sunset",
            "Wed, 31 Dec 2026 23:59:59 GMT");

        return Ok(new[]
        {
            new
            {
                Code = "CS101",
                Title = "Introduction to Programming"
            },
            new
            {
                Code = "CS102",
                Title = "Database Systems"
            }
        });
    }
}
