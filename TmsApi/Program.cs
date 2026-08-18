
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

using System.Threading.Channels;
using TmsApi.Application.Transcripts;
using TmsApi.Infrastructure.Transcripts;
using TmsApi.Infrastructure.Workers;
using TmsApi.Hubs;
using TmsApi.Services;
using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder(args);


// Services
var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>()
    ?? ["http://localhost:4200"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("TmsClient", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
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

builder.Services.AddSingleton(
    Channel.CreateUnbounded<TranscriptJobRequest>());

builder.Services.AddSingleton<ITranscriptStatusStore, InMemoryTranscriptStatusStore>();

builder.Services.AddHostedService<TranscriptWorker>();

builder.Services.AddSignalR();

builder.Services.AddSingleton<ITranscriptNotificationPublisher,
    SignalRTranscriptNotificationPublisher>();


builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "XSRF-TOKEN";
    options.HeaderName = "X-XSRF-TOKEN";
});



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

//app.UseCors("AllowAngular");
app.UseCors("TmsClient");

app.UseAuthentication();

app.UseAuthorization();

// Controllers
app.MapControllers();

app.MapHub<EnrollmentHub>("/hubs/enrollments");

app.MapHub<NotificationsHub>("/hubs/notifications");

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


app.MapPost("/api/grades", async (object payload) =>
{
    await Task.Delay(3000);

    return Results.Ok(new
    {
        id = Guid.NewGuid().ToString("N")[..8],
        success = true
    });
});

app.MapGet("/api/v1/auth/xsrf",
    (HttpContext ctx, IAntiforgery antiforgery) =>
{
    var tokens = antiforgery.GetAndStoreTokens(ctx);

    return Results.Ok(new
    {
        token = tokens.RequestToken
    });
});

app.Run();




