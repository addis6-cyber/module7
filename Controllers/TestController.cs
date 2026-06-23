using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Entities;


namespace TmsApi.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly TmsDbContext _context;

    public TestController(TmsDbContext context)
    {
        _context = context;
    }


    [HttpGet("deferred")]
public IActionResult Deferred()
{
    Console.WriteLine("Step 1: Building query...");

    var query = _context.Students
        .Where(s => s.IsActive);

    Console.WriteLine("Step 2: Query built.");

    Console.WriteLine("Step 3: About to execute ToList()...");

    var students = query.ToList();

    Console.WriteLine("Step 4: Query executed.");
    return Ok(students);
    
}
   [HttpGet("translation-fail")]
public IActionResult TranslationFail()
{
    Console.WriteLine("Building query...");

    var query = _context.Students
        .Where(s => IsExcellentStudent(s));

   // Console.WriteLine("About to execute ToList()...");

    var students = query.ToList();

    return Ok(students);
}
private bool IsExcellentStudent(Student student)
{
    return student.GPA >= 3.5m;
}

//Exercise5 Test the Restrict Behavior
[HttpDelete("delete-course/{id}")]
public IActionResult DeleteCourse(int id)
{
    var course = _context.Courses.Find(id);

    if (course == null)
        return NotFound();

    _context.Courses.Remove(course);
    _context.SaveChanges();

    return Ok("Course deleted.");
}
}

