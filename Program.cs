/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();*/

//Exercise 1A
//m4-lab-session2
/*using Microsoft.AspNetCore.Authentication;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<EnrollmentWorker>();

builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});


builder.Services
    .AddAuthentication("Training")
    .AddScheme<AuthenticationSchemeOptions,
        TrainingAuthHandler>("Training", null);

builder.Services.AddAuthorization();

//M4 SESSION 2 EX3
builder.Services
    .AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();
var app = builder.Build();


// Exercise 1B
app.UseMiddleware<RequestLoggingMiddleware>();

//app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/api/assessments/results", () =>
{
    return Results.Ok(new
    {
        courseCode = "CS-101",
        studentId = "S-001",
        letterGrade = "A"
    });
})
.RequireAuthorization();


//To test the worker
app.MapGet("/api/enrollments/worker-smoke",
    (EnrollmentWorker worker) =>
{
    worker.ProcessBatch();
    return Results.Ok("Processed");
});

app.Run();*/

using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Entities;
//module 6
using TmsApi.Repositories;

var builder = WebApplication.CreateBuilder(args);




// Services
builder.Services.AddSingleton<EnrollmentWorker>();

builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
//module 6
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.Services
    .AddAuthentication("Training")
    .AddScheme<AuthenticationSchemeOptions, TrainingAuthHandler>(
        "Training", null);


//builder.Services.AddDbContext<TmsDbContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("TmsDatabase")));
builder.Services.AddDbContext<TmsDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("TmsDatabase"))
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging());


builder.Services.AddAuthorization();

// M4 Session 2 Exercise 3
builder.Services
    .AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();

// M4 Session 3
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();



// Development only
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}



// Middleware
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();



// Controllers
app.MapControllers();



// Existing endpoint
app.MapGet("/api/assessments/results", () =>
{
    return Results.Ok(new
    {
        courseCode = "CS-101",
        studentId = "S-001",
        letterGrade = "A"
    });
})
.RequireAuthorization();



// Worker smoke test
app.MapGet("/api/enrollments/worker-smoke",
    (EnrollmentWorker worker) =>
{
    worker.ProcessBatch();
    return Results.Ok("Processed");
});




// ProblemDetails test
app.MapGet("/api/error", () =>
{
    throw new Exception("Simulated database failure");
});


//Get the Database Context
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<TmsDbContext>();

        // Apply any pending migrations
    context.Database.Migrate();


    



    // Check if the Students table is empty
    if (!context.Students.Any())
    {
        //Console.WriteLine(context.Students.Any());
        var students = new[]
{
    new Student
    {
        RegistrationNumber = "S001",
        Name = "Alice",
        GPA = 3.8m,
        IsActive = true
    },

    new Student
    {
        RegistrationNumber = "S002",
        Name = "Bob",
        GPA = 2.9m,
        IsActive = true
    },

    new Student
    {
        RegistrationNumber = "S003",
        Name = "Charlie",
        GPA = 3.4m,
        IsActive = false
    }
};
        context.Students.AddRange(students);
        context.SaveChanges();


    //courses
    var courses = new[]
{
    new Course
    {
        Code = "CS101",
        Title = "Introduction to Programming",
        Capacity = 30
    },

    new Course
    {
        Code = "CS102",
        Title = "Database Systems",
        Capacity = 25
    },

    new Course
    {
        Code = "CS103",
        Title = "Web Development",
        Capacity = 20
    }
};

    context.Courses.AddRange(courses);
    context.SaveChanges();


    //Enrollments
    var enrollments = new[]
{
    new Enrollment
    {
        StudentId = students[0].Id,
        CourseId = courses[0].Id,
        EnrolledAt = DateTime.UtcNow
    },

    new Enrollment
    {
        StudentId = students[0].Id,
        CourseId = courses[1].Id,
        EnrolledAt = DateTime.UtcNow
    },

    new Enrollment
    {
        StudentId = students[1].Id,
        CourseId = courses[0].Id,
        EnrolledAt = DateTime.UtcNow
    },

    new Enrollment
    {
        StudentId = students[2].Id,
        CourseId = courses[2].Id,
        EnrolledAt = DateTime.UtcNow
    }
};

context.Enrollments.AddRange(enrollments);
context.SaveChanges();



    }

}


app.Run();