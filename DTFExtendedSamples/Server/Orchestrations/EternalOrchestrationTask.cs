using System;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class EternalOrchestrationTask : AsyncTaskActivity<EternalOrchestrationInput, EternalOrchestrationResult>
    {
        private readonly ILogger<EternalOrchestrationTask> _logger;

        public EternalOrchestrationTask(ILogger<EternalOrchestrationTask> logger) => (_logger) = (logger);

        protected override async Task<EternalOrchestrationResult> ExecuteAsync(TaskContext context, EternalOrchestrationInput input)
        {
             _logger.LogInformation("Performing a long task here!");

             await Task.Delay(TimeSpan.FromSeconds(3));

            var plusOne = input.IterationCounter + 1;

             return new("ABC" + plusOne, plusOne);
        }
    }
}