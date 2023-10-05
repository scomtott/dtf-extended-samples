using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class DelayedAsyncWorkTask : AsyncTaskActivity<DelayedAsyncWorkTaskInput, bool>
    {
        private readonly ILogger<DelayedAsyncWorkTask> _logger;

        public DelayedAsyncWorkTask(ILogger<DelayedAsyncWorkTask> logger)
        {
            _logger = logger;
        }

        protected override async Task<bool> ExecuteAsync(TaskContext context, DelayedAsyncWorkTaskInput input)
        {
            _logger.LogInformation($"##########################################Start waiting for task {input.message}, for: {input.delayMilliseconds} ms.");
            await Task.Delay(input.delayMilliseconds);
            _logger.LogInformation($"##########################################Finished waiting for task {input.message}.");
            
            return true;
        }

    }
}