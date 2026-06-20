using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly TmsDbContext _context;

    public StudentsController(TmsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetStudents()
    /*{
       var students = _context.Students
    .Include(s => s.Enrollments)
        .ThenInclude(e => e.Course)
    .ToList();

        return Ok(students);
    }*/
    {
    var students = _context.Students
        .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
        .Select(s => new
        {
            s.Id,
            s.Name,
            s.RegistrationNumber,
            s.GPA,
            Courses = s.Enrollments.Select(e => new
            {
                e.Course.Code,
                e.Course.Title
            })
        })
        .ToList();

    return Ok(students);
}


//Filter Data
    [HttpGet("active")]
public IActionResult GetActiveStudents()
{
    var students = _context.Students
        .Where(s => s.IsActive)
        .Select(s => new
        {
            s.Id,
            s.Name,
            s.RegistrationNumber,
            s.GPA
        })
        .ToList();

    return Ok(students);
}

//sorting
[HttpGet("by-gpa")]
public IActionResult GetStudentsByGpa()
{
    var students = _context.Students
        .OrderByDescending(s => s.GPA)
        .Select(s => new
        {
            s.Name,
            s.GPA
        })
        .ToList();

    return Ok(students);
}

//Search for a Student by Registration Number
[HttpGet("search/{registrationNumber}")]
public IActionResult GetStudent(string registrationNumber)
{
    var student = _context.Students
        .Where(s => s.RegistrationNumber == registrationNumber)
        .Select(s => new
        {
            s.Id,
            s.Name,
            s.RegistrationNumber,
            s.GPA,
            s.IsActive
        })
        .FirstOrDefault();

    if (student == null)
    {
        return NotFound();
    }

    return Ok(student);
}
//Prevent Circular References
[HttpGet("raw")]
public IActionResult GetRawStudents()
{
    var students = _context.Students
        .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
        .ToList();

    return Ok(students);
}

//Create a second endpoint that returns a DTO/projection
[HttpGet("safe")]
public IActionResult GetSafeStudents()
{
    var students = _context.Students
        .Select(s => new
        {
            s.Id,
            s.Name,
            s.RegistrationNumber,
            s.GPA,
            Courses = s.Enrollments.Select(e => new
            {
                e.Course.Code,
                e.Course.Title
            })
        })
        .ToList();

    return Ok(students);
}
}