using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class DelayedWorkOrchestration : TaskOrchestration<int, string, object, object>
    {
        private readonly ILogger<DelayedWorkOrchestration> _logger;
        private readonly RetryOptions _retryOptions;
        private const ushort THREE = 3;

        public DelayedWorkOrchestration(ILogger<DelayedWorkOrchestration> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._retryOptions = new RetryOptions(TimeSpan.FromSeconds(THREE), 1);
        }

        public override async Task<int> RunTask(OrchestrationContext context, string input)
        {
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            List<Task<bool>> delayTaskList = new List<Task<bool>>()
            {
                context.ScheduleWithRetry<bool>(typeof(DelayedAsyncWorkTask), _retryOptions, 10000),
                context.ScheduleWithRetry<bool>(typeof(DelayedAsyncWorkTask), _retryOptions, 10000),
                context.ScheduleWithRetry<bool>(typeof(DelayedAsyncWorkTask), _retryOptions, 10000),
            };

            _logger.LogInformation("##########################################Awaiting DelayedAsyncTasks");

            try
            {
                await Task.WhenAll(delayTaskList);
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~OperationCancelledException Caught");
                return 1;
            }
            

            _logger.LogInformation("##########################################Finished awaiting DelayedAsyncTasks");

            return 0;
        }

        public override void OnEvent(OrchestrationContext context, string eventName, object input)
        {
            _logger.LogInformation($"################################################## OnEvent executed. {eventName} {input}");
        }
    }
}