
using Microsoft.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using System.Text.Json;


namespace camunda8_dotnet_poc_examples.Workers;
public class MoveToDeptBWorker : BaseWorkerService
{
    public MoveToDeptBWorker(IZeebeClient zeebeClient, ILogger<MoveToDeptBWorker> logger)
        : base(zeebeClient, logger, "move-to-dept-b", "move-to-dept-b-worker")
    {
    }

    protected override async Task HandleJob(IJobClient jobClient, IJob job)
    {
        Logger.LogInformation("Processing Move to Department B for job: {JobKey}", job.Key);

        try
        {
            // Extract variables from the process
            var variables = JsonSerializer.Deserialize<OnboardingVariables>(job.Variables);

            // Your business logic to move employee to department B
            Logger.LogInformation("Moving employee {EmployeeName} to Department B", variables.EmployeeName);

            // Simulate work
            await Task.Delay(1000);

            // Complete the job with updated variables
            var updatedVariables = new
            {
                employeeMovedToDeptB = true,
                moveTimestamp = DateTime.UtcNow.ToString("O")
            };

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(JsonSerializer.Serialize(updatedVariables))
                .Send();

            Logger.LogInformation("Completed Move to Department B for job: {JobKey}", job.Key);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to process job {JobKey}", job.Key);

            await jobClient.NewFailCommand(job.Key)
                .Retries(job.Retries - 1)
                .ErrorMessage(ex.Message)
                .Send();
        }
    }
}