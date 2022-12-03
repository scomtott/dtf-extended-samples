using System;
using System.Threading.Tasks;
using DTFExtendedSamples.Core;
using DTFExtendedSamples.Server.Models;
using DTFExtendedSamples.Server.Orchestrations;
using DurableTask.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server
{
    public class ServerEntryPoint: IEntryPoint, IDisposable
    {
        private readonly ILogger<ServerEntryPoint> _logger;
        private readonly TaskHubWorker _taskHubWorker;
        private readonly  IHost _host;
        private readonly TaskHubClient _taskHubClient;

        public ServerEntryPoint(ILogger<ServerEntryPoint> logger, TaskHubWorker taskHubWorker, TaskHubClient taskHubClient, IHost host)
        {
            this._logger = logger;
            this._taskHubWorker = taskHubWorker;
            this._host = host;
            this._taskHubClient = taskHubClient;
        }

        public async Task Run()
        {
            await _taskHubWorker.StartAsync();
            _logger.LogInformation("Server is running");

            // The orchestration entry is recreated, hence the input is reset upon creating the instance.
            await _taskHubClient.CreateOrchestrationInstanceAsync(typeof(EternalOrchestration).Name, "V1", "singleton::instance::of::EternalOrchestration", new EternalOrchestrationInput() );

            await _host.RunAsync();
        }

        public void Dispose()
        {
            _logger.LogDebug("Stopping task worker");
            _taskHubWorker?.StopAsync().Wait();
            _taskHubWorker?.Dispose();
        }
    }
}