using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Models;
using DTFExtendedSamples.Server.Extensions;
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
            /*List<Task<bool>> delayTaskList = new List<Task<bool>>()
            {
                context.ScheduleTask<bool>(typeof(DelayedAsyncWorkTask), 10000),
                context.ScheduleTask<bool>(typeof(DelayedAsyncWorkTask), 10000),
                context.ScheduleTask<bool>(typeof(DelayedAsyncWorkTask), 10000),
            };*/

            _logger.LogInformation(context, "##########################################DelayedWorkOrchestration started");

            try
            {
                _logger.LogInformation(context, "##########################################Scheduled DelayedAsyncWorkTask 1 from DelayedWorkOrchestration");
                await context.ScheduleTask<bool>(typeof(DelayedAsyncWorkTask), new DelayedAsyncWorkTaskInput(10000, "1"));

                _logger.LogInformation(context, "##########################################Scheduled DelayedAsyncWorkTask 2 from DelayedWorkOrchestration");
                await context.ScheduleTask<bool>(typeof(DelayedAsyncWorkTask), new DelayedAsyncWorkTaskInput(10000, "2"));

                _logger.LogInformation(context, "##########################################Scheduled DelayedAsyncWorkTask 3 from DelayedWorkOrchestration");
                await context.ScheduleTask<bool>(typeof(DelayedAsyncWorkTask), new DelayedAsyncWorkTaskInput(10000, "3"));
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~OperationCancelledException Caught");
                return 1;
            }
            

            _logger.LogInformation(context, "##########################################Finished awaiting DelayedAsyncTasks from DelayedWorkOrchestration");

            return 0;
        }

        public override async void OnEvent(OrchestrationContext context, string eventName, object input)
        {
            _logger.LogInformation(context, $"################################################## OnEvent started. {eventName} {input} from DelayedWorkOrchestration");
            await context.ScheduleTask<bool>(typeof(DelayedAsyncWorkTask), new DelayedAsyncWorkTaskInput(15000, nameof(OnEvent)));
            _logger.LogInformation(context, $"################################################## OnEvent finished from DelayedWorkOrchestration");
        }
    }
}