using System;
using System.Threading.Tasks;
using DTFExtendedSamples.Core;
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
        
        public ServerEntryPoint(ILogger<ServerEntryPoint> logger, TaskHubWorker taskHubWorker, IHost host)
        {
            this._logger = logger;
            this._taskHubWorker = taskHubWorker;
            this._host = host;
        }

        public async Task Run()
        {
            await _taskHubWorker.StartAsync();
            _logger.LogInformation("Server is running");

            
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