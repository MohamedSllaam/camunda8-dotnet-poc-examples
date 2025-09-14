using camunda8_dotnet_poc_examples.Workers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zeebe.Client;
using System.Text.Json;

namespace camunda8_dotnet_poc_examples.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewProcessController : ControllerBase
{
    private readonly IZeebeClient _zeebeClient;
    private readonly ILogger<ProcessController> _logger;

    public NewProcessController(IZeebeClient zeebeClient, ILogger<ProcessController> logger)
    {
        _zeebeClient = zeebeClient;
        _logger = logger;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartOnboardingProcess([FromBody] EmployeeOnboardingRequest request)
    {
        try
        {

            var processInstance = await _zeebeClient
                .NewCreateProcessInstanceCommand()
                .BpmnProcessId("Process_1y4ze5m")
                .LatestVersion()
                .Variables(JsonSerializer.Serialize(new
                {
                    EmployeeName = request.EmployeeName,
                    EmployeeId = request.EmployeeId,
                    Email = request.Email,
                    Department = request.Department
                }))
                .Send();

            return Ok(new
            {
                ProcessInstanceKey = processInstance.ProcessInstanceKey,
                Message = "Onboarding process started successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start onboarding process");
            return StatusCode(500, "Failed to start process");
        }
    }

    [HttpGet("status/{processInstanceKey}")]
    public async Task<IActionResult> GetProcessStatus(long processInstanceKey)
    {
        try
        {
            var instance = await _zeebeClient
                .NewActivateJobsCommand()
                .JobType("check-status")
                .MaxJobsToActivate(1)
                .Send();

            return Ok(new { Status = "Running", ProcessInstanceKey = processInstanceKey });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get status for process instance {processInstanceKey}");
            return StatusCode(500, "Failed to get process status");
        }
    }
}

public class EmployeeOnboardingRequest
{
    public string EmployeeName { get; set; }
    public string EmployeeId { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
}