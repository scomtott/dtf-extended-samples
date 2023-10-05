using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class ScheduleDelayedWorkOrchestrationTask : AsyncTaskActivity<string, OrchestrationInstance>
    {
        private readonly ILogger<ScheduleDelayedWorkOrchestrationTask> _logger;
        private readonly TaskHubClient _taskHubClient;

        public ScheduleDelayedWorkOrchestrationTask(
            ILogger<ScheduleDelayedWorkOrchestrationTask> logger,
            TaskHubClient taskHubClient)
        {
            _logger = logger;
            _taskHubClient = taskHubClient;
        }

        protected override async Task<OrchestrationInstance> ExecuteAsync(
            TaskContext context,
            string input)
        {
            return await _taskHubClient.CreateOrchestrationInstanceAsync(name: nameof(DelayedWorkOrchestration), version: "V1", input);
        }

    }
}