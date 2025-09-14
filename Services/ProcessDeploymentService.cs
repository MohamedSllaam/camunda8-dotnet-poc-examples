
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
                .AddResourceFile("Processes/employee-onboarding.bpmn")
                .Send();

            _logger.LogInformation("Deployed {DeployedProcesses} processes", deployment.Processes.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deploy BPMN process");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}