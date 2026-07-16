using Microsoft.AspNetCore.Mvc;

namespace TmsApi.Controllers.V2;

[ApiController]
[Route("api/v2/courses")]
public class CoursesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[]
        {
            new
            {
                Code = "CS101",
                Title = "Introduction to Programming",
                Credits = 3
            },
            new
            {
                Code = "CS102",
                Title = "Database Systems",
                Credits = 4
            }
        });
    }
}