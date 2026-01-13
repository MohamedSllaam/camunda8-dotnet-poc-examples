
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zeebe.Client;
namespace camunda8_dotnet_poc_examples.Services;
public class ProcessDeploymentService : IHostedService
{
    private readonly IZeebeClient _zeebeClient;
    private readonly ILogger<ProcessDeploymentService> _logger;

    public ProcessDeploymentService(IZeebeClient zeebeClient, ILogger<ProcessDeploymentService> logger)
    {
        _zeebeClient = zeebeClient;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            
            // Deploy your BPMN process
            var deployment = await _zeebeClient.NewDeployCommand()
                .AddResourceFile("Resources/test-process.bpmn")
                .Send();

            _logger.LogInformation("Deployed {DeployedProcesses} processes", deployment.Processes.Count);
            if (!deployment.Processes.Any())
            {
                _logger.LogError("❌ Deployment succeeded but NO processes were deployed");
                return;
            }

            foreach (var p in deployment.Processes)
            {
                _logger.LogInformation(
                    "✅ Deployed BPMN: {ProcessId}, Version={Version}, Key={Key}",
                    p.BpmnProcessId,
                    p.Version,
                    p.ProcessDefinitionKey
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deploy BPMN process");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}