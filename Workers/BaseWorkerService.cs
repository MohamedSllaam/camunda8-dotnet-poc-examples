using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace camunda8_dotnet_poc_examples.Workers;

public abstract class BaseWorkerService : BackgroundService
{
    protected readonly IZeebeClient ZeebeClient;
    protected readonly ILogger<BaseWorkerService> Logger;
    protected readonly string JobType;
    protected readonly string WorkerName;

    protected BaseWorkerService(IZeebeClient zeebeClient, ILogger<BaseWorkerService> logger, string jobType, string workerName)
    {
        ZeebeClient = zeebeClient;
        Logger = logger;
        JobType = jobType;
        WorkerName = workerName;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var jobWorker = ZeebeClient.NewWorker()
            .JobType(JobType)
            .Handler(HandleJob)
            .MaxJobsActive(5)
            .Name(WorkerName)
            .PollInterval(TimeSpan.FromMilliseconds(100))
            .Timeout(TimeSpan.FromSeconds(30))
            .Open())
        {
            Logger.LogInformation("{WorkerName} worker started for job type: {JobType}", WorkerName, JobType);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    protected abstract Task HandleJob(IJobClient jobClient, IJob job);
}

public class OnboardingVariables
{
    public string EmployeeName { get; set; }
    public string EmployeeId { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
}