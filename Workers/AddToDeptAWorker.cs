using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using System.Text.Json;
namespace camunda8_dotnet_poc_examples.Workers;

public class AddToDeptAWorker : BaseWorkerService
{
    public AddToDeptAWorker(IZeebeClient zeebeClient, ILogger<AddToDeptAWorker> logger)
        : base(zeebeClient, logger, "add-to-dept-a", "add-to-dept-a-worker")
    {
    }

    protected override async Task HandleJob(IJobClient jobClient, IJob job)
    {
        Logger.LogInformation("Processing Add to Department A for job: {JobKey}", job.Key);

        try
        {
            // Extract variables from the process
            var variables = JsonSerializer.Deserialize<OnboardingVariables>(job.Variables);

            // Your business logic to add employee to department A
            Logger.LogInformation("Adding employee {EmployeeName} to Department A", variables.EmployeeName);

            // Simulate work
            await Task.Delay(1000);

            // Complete the job with updated variables
            var updatedVariables = new
            {
                employeeAddedToDeptA = true,
                timestamp = DateTime.UtcNow.ToString("O")
            };

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(JsonSerializer.Serialize(updatedVariables))
                .Send();

            Logger.LogInformation("Completed Add to Department A for job: {JobKey}", job.Key);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to process job {JobKey}", job.Key);

            // Fail the job with retry options
            await jobClient.NewFailCommand(job.Key)
                .Retries(job.Retries - 1)
                .ErrorMessage(ex.Message)
                .Send();
        }
    }
}