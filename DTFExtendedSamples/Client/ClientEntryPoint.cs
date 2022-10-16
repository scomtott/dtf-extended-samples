using System;
using System.Threading.Tasks;
using DTFExtendedSamples.Core;
using DTFExtendedSamples.Server.Orchestrations;
using DurableTask.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DTFExtendedSamples.Client
{
    public class ClientEntryPoint: IEntryPoint
    {
        private readonly TaskHubClient _taskHubClient;
        private readonly ClientInput _clientInput;
        private readonly ILogger<ClientEntryPoint> _logger;

        public ClientEntryPoint(TaskHubClient taskHubClient, IOptions<ClientInput> clientInput, ILogger<ClientEntryPoint> logger)
        {
            this._taskHubClient = taskHubClient;
            this._clientInput = clientInput.Value;
            this._logger = logger;
        }

        public async Task Run()
        {
            _logger.LogInformation("Starting orchestration: {orchestration}", _clientInput.Orchestration);
            var instanceId = _clientInput.Instance ?? Guid.NewGuid().ToString();
            var instance = await _taskHubClient.CreateOrchestrationInstanceAsync(_clientInput.Orchestration, "V1", instanceId,  _clientInput.Arguments);
            _logger.LogDebug("Created instance: {instanceId}", instance.InstanceId);
        }
    }
}