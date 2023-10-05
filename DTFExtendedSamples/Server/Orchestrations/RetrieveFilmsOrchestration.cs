using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTFExtendedSamples.Server.Extensions;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class RetrieveFilmsOrchestration : TaskOrchestration<int, string>
    {
        private readonly ILogger<RetrieveFilmsOrchestration> _logger;
        private readonly RetryOptions _retryOptions;
        private readonly TaskHubClient _taskHubClient;
        private const ushort THREE = 3;

        public RetrieveFilmsOrchestration(
            ILogger<RetrieveFilmsOrchestration> logger,
            IConfiguration configuration,
            TaskHubClient taskHubClient)
        {
            this._logger = logger;
            this._retryOptions = new RetryOptions(TimeSpan.FromSeconds(THREE), THREE);
            this._taskHubClient = taskHubClient;
        }

        public override async Task<int> RunTask(OrchestrationContext context, string input)
        {
            var args = new ParsedArgs(input);

            var peopleSearchResults = await context.ScheduleTask<SearchResult<Person>>(typeof(FindPeopleTask), args.Person);

            var numOfPeople = peopleSearchResults.Count;

            _logger.LogInformation(context, $"##########################################ScheduleDelayedWorkOrchestrationTask scheduled from RetrieveFilmsOrchestration");
            var orchestrationInstance = await context.ScheduleTask<OrchestrationInstance>(typeof(ScheduleDelayedWorkOrchestrationTask), "hello");

            _logger.LogInformation(context, $"##########################################RaiseEventTask Scheduled from RetrieveFilmsOrchestration");
            var result2 = await context.ScheduleTask<bool>(typeof(RaiseEventTask), (orchestrationInstance.ExecutionId, orchestrationInstance.InstanceId));

            _logger.LogInformation(context, $"##########################################WaitForOrchestrationAsync RetrieveFilmsOrchestration");
            var orchResult = await _taskHubClient.WaitForOrchestrationAsync(orchestrationInstance, TimeSpan.FromSeconds(60));


            _logger.LogInformation(context, $"##########################################WaitForOrchestrationAsync finished OrchestrationStatus: {orchResult.OrchestrationStatus}.");

            /*if (args.SecondsDelay > 0)
            {
                _logger.LogDebug("Waiting for {seconds}", args.SecondsDelay);
                await context.CreateTimer(context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(args.SecondsDelay)), "delayTimer");
            }

            if (numOfFilms > 0)
            {
                _logger.LogInformation(context, "Number of films {numOfFilms}", numOfFilms);
                var films = await Task.WhenAll(filmsTasks);

                // aggregate
                await context.ScheduleTask<bool>(typeof(WriteFileTask), new WriteFileTaskInput(films, args.FileName, args.WriteMode, args.ClientRef));
            }

            return numOfFilms;*/
            return 1;
        }

        private class ParsedArgs
        {
            internal string Person { get; }
            internal string FileName { get; }
            internal int SecondsDelay { get; }
            internal char WriteMode { get; }
            internal string? ClientRef { get; }

            internal ParsedArgs(string input)
            {
                var args = input.Split(";");
                if (args.Length < 2)
                {
                    throw new ArgumentException("This orchestration requires at least two values separated by ;");
                }

                this.Person = args[0];
                this.FileName = args[1];

                if (args.Length > 2)
                {
                    int.TryParse(args[2], out var delay);
                    this.SecondsDelay = delay;
                }

                this.WriteMode = args.Length > 3 ? args[3].ToLower().ToCharArray().First() : 'w';

                this.ClientRef = args.Length > 4 ? args[4] : "_";
            }
        } 
    }
}