using System;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class EternalOrchestration : TaskOrchestration<int, string>
    {
        private readonly ILogger<RetrieveFilmsOrchestration> _logger;

        public EternalOrchestration(ILogger<RetrieveFilmsOrchestration> logger, IConfiguration configuration)
        {
            this._logger = logger;
        }

        public override async Task<int> RunTask(OrchestrationContext context, string args)
        {
            EternalOrchestrationInput input = new EternalOrchestrationInput(Convert.ToInt32(args));

            _logger.LogInformation("Doing some work at iteration: {}", input.IterationCounter);

            var result = await context.ScheduleTask<EternalOrchestrationResult>(typeof(EternalOrchestrationTask), input);

            _logger.LogDebug("Waiting for 10 seconds after returning result: {}", result.aString);

            await context.CreateTimer(context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(10)), "delayTimer");
            
            context.ContinueAsNew(Convert.ToString(input.IterationCounter + 1));

            _logger.LogDebug("Before returning... ");

            return input.IterationCounter;
        }

    }
}