using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class RaiseEventTask : AsyncTaskActivity<(string, string), bool>
    {
        private readonly ILogger<RaiseEventTask> _logger;
        private readonly TaskHubClient _taskHubClient;

        public RaiseEventTask(
            ILogger<RaiseEventTask> logger,
            TaskHubClient taskHubClient)
        {
            _logger = logger;
            _taskHubClient = taskHubClient;
        }

        protected override async Task<bool> ExecuteAsync(
            TaskContext context,
            (string, string) orchestrationInstanceData)
        {
            _logger.LogInformation($"##########################################Fire event in RaiseEventTask.");
            var orchestrationInstance = new OrchestrationInstance()
            {
                ExecutionId = orchestrationInstanceData.Item1,
                InstanceId = orchestrationInstanceData.Item2,
            };

            await _taskHubClient.RaiseEventAsync(orchestrationInstance, "eventMessageName", "eventData");
            _logger.LogInformation($"##########################################Event fired in RaiseEventTask.");
            return true;
        }

    }
}