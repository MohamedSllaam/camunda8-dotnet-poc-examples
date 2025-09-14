using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zeebe.Client;

namespace camunda8_dotnet_poc_examples.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProcessController : ControllerBase
{
    private readonly IZeebeClient _zeebeClient;

    public ProcessController()
    {
        _zeebeClient = ZeebeClient.Builder()
            .UseGatewayAddress("127.0.0.1:26500")
            .UsePlainText()
            .Build();
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartProcess()
    {
        var result = await _zeebeClient.NewCreateProcessInstanceCommand()
            .BpmnProcessId("Process_1y4ze5m") // from your BPMN XML
            .LatestVersion()
            .Send();

        return Ok(new { result.ProcessInstanceKey });
    }
}