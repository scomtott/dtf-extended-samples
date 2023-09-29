using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class DelayedAsyncWorkTask : AsyncTaskActivity<int, bool>
    {
        private delegate void WriteToDisk(string location, string? contents);
        private readonly ILogger<DelayedAsyncWorkTask> _logger;

        public DelayedAsyncWorkTask(ILogger<DelayedAsyncWorkTask> logger) => (_logger) = (logger);

        protected override async Task<bool> ExecuteAsync(TaskContext context, int input)
        {
            _logger.LogInformation($"##########################################Start waiting for {input} ms.");
            await Task.Delay(input);
            _logger.LogInformation($"##########################################Finished waiting for {input} ms.");
            
            return true;
        }

    }
}