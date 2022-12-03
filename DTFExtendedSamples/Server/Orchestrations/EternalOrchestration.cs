using System;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class EternalOrchestration : TaskOrchestration<int, EternalOrchestrationInput>
    {
        private readonly ILogger<RetrieveFilmsOrchestration> _logger;

        public EternalOrchestration(ILogger<RetrieveFilmsOrchestration> logger, IConfiguration configuration)
        {
            this._logger = logger;
        }

        public override async Task<int> RunTask(OrchestrationContext context, EternalOrchestrationInput input)
        {
            _logger.LogInformation("Doing some work at iteration: {iteration}", input.IterationCounter);

            _logger.LogDebug("Waiting for 10 seconds");

            await context.CreateTimer(context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(10)), "delayTimer");
            
            context.ContinueAsNew(new EternalOrchestrationInput(input.IterationCounter + 1));

            _logger.LogDebug("Before returning... ");

            return input.IterationCounter;
        }

    }
}