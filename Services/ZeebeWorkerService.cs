using Zeebe.Client;
using Zeebe.Client.Api.Worker;

namespace camunda8_dotnet_poc_examples.Services;
public class ZeebeWorkerService : BackgroundService
{
    private readonly IZeebeClient _zeebe;

    public ZeebeWorkerService(IZeebeClient zeebe)
    {
        _zeebe = zeebe;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting BPMN workers...");

        // Worker 1: Add employee
        _zeebe.NewWorker()
            .JobType("add-to-dept-a")
            .Handler(async (client, job) =>
            {
                var vars = job.Variables;
                Console.WriteLine($"Adding employee to Dept A: {vars}");

                await client.NewCompleteJobCommand(job.Key).Send();
            })
            .MaxJobsActive(10)
            .Name("AddEmployeeWorker")
            .Open();

        // Worker 2: Move employee
        _zeebe.NewWorker()
            .JobType("move-to-dept-b")
            .Handler(async (client, job) =>
            {
                Console.WriteLine("Moving employee to Dept B...");
                await client.NewCompleteJobCommand(job.Key).Send();
            })
            .MaxJobsActive(10)
            .Name("MoveEmployeeWorker")
            .Open();

        // Worker 3: Send email
        _zeebe.NewWorker()
            .JobType("send-email")
            .Handler(async (client, job) =>
            {
             //  var email = job.VariablesAsDictionary().GetValueOrDefault("email")?.ToString();
            //    Console.WriteLine($"Sending email to {email ?? "unknown"}");

                // In real app → send SMTP / SendGrid
                await client.NewCompleteJobCommand(job.Key).Send();
            })
            .MaxJobsActive(5)
            .Name("EmailWorker")
            .Open();

        await Task.Delay(-1, stoppingToken); // keep workers alive
    }
}