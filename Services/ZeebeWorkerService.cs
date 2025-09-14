using Zeebe.Client;
using Zeebe.Client.Api.Worker;

namespace camunda8_dotnet_poc_examples.Services;
public class ZeebeWorkerService : BackgroundService
{
    private readonly IZeebeClient _client;

    public ZeebeWorkerService()
    {
        _client = ZeebeClient.Builder()
            .UseGatewayAddress("127.0.0.1:26500") // Zeebe Gateway
            .UsePlainText()
            .Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("🚀 Zeebe workers starting...");

        // Worker for add-to-dept-a
        _client.NewWorker()
            .JobType("add-to-dept-a")
            .Handler(async (client, job) =>
            {
                Console.WriteLine("👔 Employee added to Dept A");
                await client.NewCompleteJobCommand(job.Key)
                    .Variables("{\"status\":\"added-to-dept-a\"}")
                    .Send();
            })
            .Name("worker-add-to-dept-a")
            .MaxJobsActive(5)
            .Open();

        // Worker for move-to-dept-b
        _client.NewWorker()
            .JobType("move-to-dept-b")
            .Handler(async (client, job) =>
            {
                Console.WriteLine("📦 Employee moved to Dept B");
                await client.NewCompleteJobCommand(job.Key)
                    .Variables("{\"status\":\"moved-to-dept-b\"}")
                    .Send();
            })
            .Name("worker-move-to-dept-b")
            .MaxJobsActive(5)
            .Open();

        // Worker for send-email
        _client.NewWorker()
            .JobType("send-email")
            .Handler(async (client, job) =>
            {
                Console.WriteLine("📧 Welcome email sent");
                await client.NewCompleteJobCommand(job.Key)
                    .Variables("{\"status\":\"email-sent\"}")
                    .Send();
            })
            .Name("worker-send-email")
            .MaxJobsActive(5)
            .Open();

        // Keep background service alive until stopped
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("🛑 Stopping Zeebe workers...");
        _client.Dispose();
        await base.StopAsync(stoppingToken);
    }
}
