using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly TmsDbContext _context;

    public ReportsController(TmsDbContext context)
    {
        _context = context;
    }


    [HttpGet("student-enrollments")]
public IActionResult StudentEnrollments()
{
    var result = _context.Students
        .Select(student => new
        {
            student.Name,
            CourseCount = student.Enrollments.Count()
        })
        .ToList();

    return Ok(result);
}

[HttpGet("high-gpa")]
public IActionResult HighGpaStudents()
{
    var result = _context.Students
        .Where(s => s.GPA > 3.0m)
        .Select(s => new
        {
            s.Name,
            s.GPA
        })
        .ToList();

    return Ok(result);
}

[HttpGet("course-enrollments")]
public IActionResult CourseEnrollments()
{
    var result = _context.Courses
        .Select(course => new
        {
            course.Code,
            course.Title,
            StudentCount = course.Enrollments.Count()
        })
        .ToList();

    return Ok(result);
}

[HttpGet("active-students")]
public IActionResult ActiveStudents()
{
    var result = _context.Students
        .Where(s => s.IsActive)
        .Select(s => new
        {
            s.RegistrationNumber,
            s.Name,
            s.GPA
        })
        .ToList();

    return Ok(result);
}

[HttpGet("student-courses")]
public IActionResult StudentCourses()
{
    var result = _context.Students
        .Select(student => new
        {
            student.Name,
            Courses = student.Enrollments
                .Select(e => e.Course.Code)
                .ToList()
        })
        .ToList();

    return Ok(result);
}


//m5-lab-session2
//pagination endpoint
[HttpGet("students")]
public IActionResult Students(int page = 1)
{
    const int pageSize = 10;

    var students = _context.Students
        .OrderBy(s => s.Name)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(s => new
        {
            s.Id,
            s.Name,
            s.GPA
        })
        .ToList();

    return Ok(students);
}

//Exercise 3 - Step 2
[HttpGet("top-courses")]
public IActionResult TopCourses()
{
    var result = _context.Courses
        .Select(c => new
        {
            c.Title,
            EnrollmentCount = c.Enrollments.Count()
        })
        .OrderByDescending(c => c.EnrollmentCount)
        .Take(5)
        .ToList();

    return Ok(result);
}
}