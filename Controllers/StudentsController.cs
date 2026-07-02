using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Models;
using TmsApi.Entities;

// Module 6
using TmsApi.Repositories;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly TmsDbContext _context;
    private readonly IStudentRepository _repository;

    public StudentsController(
        TmsDbContext context,
        IStudentRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetStudents()
    {
        var students = _repository.GetAll();

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

    //Add a No-Tracking Endpoint
    [HttpGet("no-tracking")]
    public IActionResult GetStudentsNoTracking()
    {
        var students = _context.Students
            .AsNoTracking()
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.GPA
            })
            .ToList();

        return Ok(students);
    }

    //Compare Tracking
    [HttpGet("tracking-demo")]
    public IActionResult TrackingDemo()
    {
        var student = _context.Students.First();

        student.Name = "Updated Name";

        _context.SaveChanges();

        return Ok(student);
    }

    //See what happens with AsNoTracking()
    [HttpGet("no-tracking-demo")]
    public IActionResult NoTrackingDemo()
    {
        var student = _context.Students
            .AsNoTracking()
            .First();

        student.Name = "No Tracking Student";

        _context.SaveChanges();

        return Ok(student);
    }

    [HttpPost]
    public IActionResult CreateStudent(CreateStudentDto dto)
    {
        var student = new Student
        {
            RegistrationNumber = dto.RegistrationNumber,
            Name = dto.Name,
            GPA = dto.GPA,
            IsActive = dto.IsActive
        };

        _context.Students.Add(student);
        // Try to save
        _context.SaveChanges();

        return CreatedAtAction(
            nameof(GetStudent),
            new { registrationNumber = student.RegistrationNumber },
            student);
    }
}