
using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TmsApi.Infrastructure.Data;
using Asp.Versioning;
//module7session0
using TmsApi.Infrastructure.DependencyInjection;
using MediatR;
using FluentValidation;
using TmsApi.Application.Behaviors;
using TmsApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);


// Services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});


builder.Services.AddSingleton<EnrollmentWorker>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.Services
    .AddAuthentication("Training")
    .AddScheme<AuthenticationSchemeOptions, TrainingAuthHandler>(
        "Training", null);


builder.Services.AddAuthorization();

// M4 Session 2 Exercise 3
builder.Services
    .AddOptions<PaymentOptions>()
    .BindConfiguration("Payments")
    .ValidateDataAnnotations()
    .ValidateOnStart();

// M4 Session 3
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(TmsApi.Application.Interfaces.ICourseService).Assembly);

    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(
    typeof(TmsApi.Application.Courses.Commands.CreateCourseCommandValidator).Assembly);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHybridCache();
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

app.UseCors("AllowAngular");

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
    (IServiceProvider provider) =>
{
    var worker = provider.GetRequiredService<EnrollmentWorker>();
    worker.ProcessBatch();

    return Results.Ok("Processed");
});




// ProblemDetails test
app.MapGet("/api/error", () =>
{
    throw new Exception("Simulated database failure");
});

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<TmsDbContext>();

    await DataSeeder.SeedAsync(context);
}


app.MapGet("/api/enrollments", () =>
{
    return Results.Ok(new[]
    {
        new
        {
            id = "1",
            studentId = 1001,
            studentName = "Liya Kebede",
            courseId = 1,
            courseName = "Introduction to Programming",
            status = "Pending",
            enrolledAt = DateTime.UtcNow.ToString("o")
        },
        new
        {
            id = "2",
            studentId = 1002,
            studentName = "Dawit Bekele",
            courseId = 2,
            courseName = "Database Systems",
            status = "Approved",
            enrolledAt = DateTime.UtcNow.ToString("o")
        }
    });
});

app.MapPost("/api/enrollments/{id}/approve", (string id) =>
{
    Console.WriteLine($"Approved enrollment {id}");
    return Results.NoContent();
});
app.Run();





