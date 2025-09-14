
using Microsoft.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using System.Text.Json;


namespace camunda8_dotnet_poc_examples.Workers;
public class SendEmailWorker : BaseWorkerService
{
   // private readonly IEmailService _emailService;

    public SendEmailWorker(IZeebeClient zeebeClient, ILogger<SendEmailWorker> logger, )
        : base(zeebeClient, logger, "send-email", "send-email-worker")
    {
      //  _emailService = emailService;
    }

    protected override async Task HandleJob(IJobClient jobClient, IJob job)
    {
        Logger.LogInformation("Processing Send Welcome Email for job: {JobKey}", job.Key);

        try
        {
            // Extract variables from the process
            var variables = JsonSerializer.Deserialize<OnboardingVariables>(job.Variables);

            // Your business logic to send welcome email
            Logger.LogInformation("Sending welcome email to {Email}", variables.Email);

            // Use injected service to send email
          //  await _emailService.SendWelcomeEmailAsync(variables.Email, variables.EmployeeName);

            // Complete the job with updated variables
            var updatedVariables = new
            {
                emailSent = true,
                emailTimestamp = DateTime.UtcNow.ToString("O")
            };

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(JsonSerializer.Serialize(updatedVariables))
                .Send();

            Logger.LogInformation("Successfully sent welcome email for job: {JobKey}", job.Key);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to send email for job: {JobKey}", job.Key);

            await jobClient.NewFailCommand(job.Key)
                .Retries(job.Retries - 1)
                .ErrorMessage(ex.Message)
                .Send();
        }
    }
}