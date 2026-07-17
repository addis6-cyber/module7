using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TmsApi.Application.Interfaces;
using TmsApi.Infrastructure.Data;
using TmsApi.Infrastructure.Repositories;
using TmsApi.Infrastructure.Services;

namespace TmsApi.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TmsDbContext>(options =>
    options.UseNpgsql(
        configuration.GetConnectionString("TmsDatabase")));

        services.AddScoped<IStudentRepository, StudentRepository>();

        services.AddScoped<ICourseService, CourseService>();

        services.AddScoped<IEnrollmentService, EnrollmentService>();
        
        return services;
    }
}