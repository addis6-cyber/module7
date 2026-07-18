//For the first test
/*public class EnrollmentWorker
{
    private readonly IEnrollmentService _service;

    public EnrollmentWorker(IEnrollmentService service)
    {
        _service = service;
    }

    public void ProcessBatch()
    {
        Console.WriteLine("Processing...");
    }
}*/
//Exercise 1
using Microsoft.Extensions.DependencyInjection;
using TmsApi.Application.Interfaces;
public class EnrollmentWorker
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EnrollmentWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void ProcessBatch()
    {
        using var scope = _scopeFactory.CreateScope();

        var service = scope.ServiceProvider
            .GetRequiredService<IEnrollmentService>();

        Console.WriteLine("Processing...");
    }
}