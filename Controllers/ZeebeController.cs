using System.Threading.Tasks;
using camunda8_dotnet_poc_examples.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace camunda8_dotnet_poc_examples.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ZeebeController : Controller
    {
        private readonly ILogger<ZeebeController> _logger;
        private readonly IZeebeService _zeebeService;

        public ZeebeController(ILogger<ZeebeController> logger, IZeebeService zeebeService)
        {
            _logger = logger;
            _zeebeService = zeebeService;
        }

        //[Route("/status")]
        [HttpGet("status")]
        public async Task<IActionResult> Get()
        {
            var result = (await _zeebeService.Status()).ToString();
            return Ok(result);
        }

        [HttpGet("deploy")]
        public async Task<IActionResult> DeployWorkflow()
        {
            var response = await _zeebeService.Deploy("test-process.bpmn");
            return Ok(response);
        }

      //  [Route("/start-workers")]
        [HttpGet("start-workers")]
        public async Task<IActionResult> StartWorkflow()
        {
            await _zeebeService.StartWorkers();
            return Ok("done");
        }

        //[Route("/create-instance")]
        [HttpGet("create-instance")]

        public async Task<IActionResult> StartWorkflowInstance()
        {
            var instance = await _zeebeService.CreateWorkflowInstance("Process_0l9rt3b");
            return Ok(instance);
        }
    }
}