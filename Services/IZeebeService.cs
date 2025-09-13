using Zeebe.Client.Api.Responses;

namespace camunda8_dotnet_poc_examples.Services;

    public interface IZeebeService
    {
        public Task<ITopology> Status();
        public Task<IDeployResourceResponse> Deploy(string modelFile);
        public Task StartWorkers();
        public Task<string> CreateWorkflowInstance(string bpmProcessId);

    }

