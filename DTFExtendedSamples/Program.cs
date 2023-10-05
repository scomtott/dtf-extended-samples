using System;
using CommandLine;
using DTFExtendedSamples.Client;
using DTFExtendedSamples.Core;
using DTFExtendedSamples.Server;
using DurableTask.AzureStorage;
using DurableTask.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples
{
    interface IApplicationMode
    {
        void ConfigureServices(IServiceCollection services);
    }

    [Verb("client", HelpText = "Start the application in client mode")]
    class ClientMode : IApplicationMode
    {

        private readonly  ClientConfiguration _clientConfiguration = new ClientConfiguration();

        [Option('o', "orchestration", Required = true, HelpText = "The name of the orchestration")]
        public string? Orchestration { get; set; }

        [Option('a', "argument", Required = false, HelpText = "An optional argument for the orchestration")]
        public string? Argument { get; set; }

         [Option('i', "instance", Required = false, HelpText = "An optional name for the orchestration instance")]
        public string? Instance { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            _clientConfiguration.ConfigureServices(services, Orchestration ?? throw new ArgumentException("The Orchestration can't be null"), Argument, Instance);
        }
    }

    [Verb("server", HelpText = "Start the application in server mode")]
    class ServerMode : IApplicationMode
    {
        private readonly ServerConfiguration _serverConfiguration = new ServerConfiguration();
        public void ConfigureServices(IServiceCollection services)
        {
            _serverConfiguration.ConfigureServices(services);
        }
    }


    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<ClientMode, ServerMode>(args)
                .MapResult(
                    (IApplicationMode mode) => ConfigureAndRun(args, mode),
                    err => 1
                );
        }

        private static int ConfigureAndRun(string[] args, IApplicationMode app)
        {
            ConfigureHost(args, app);
            return 0;
        }
        
        private static void ConfigureHost(string[] args, IApplicationMode app)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();

                    services.AddSingleton<IConfiguration>(configuration);

                    var serviceSettings = new AzureStorageOrchestrationServiceSettings()
                    {
                        StorageConnectionString = configuration.GetSection("DTF")
                            .GetValue<string>("StorageAccountConnectionString"),
                        TaskHubName = "dtfHub",
                        ControlQueueVisibilityTimeout = TimeSpan.FromMinutes(1),
                        LoggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>(),
                    };

                    var orchestrationServiceAndClient = new AzureStorageOrchestrationService(serviceSettings);
                    orchestrationServiceAndClient.CreateIfNotExistsAsync().Wait();
                    
                    services.AddSingleton<IOrchestrationService>(orchestrationServiceAndClient);
                    services.AddSingleton<IOrchestrationServiceClient>(orchestrationServiceAndClient);


                    app.ConfigureServices(services);
                })
                .ConfigureLogging((context, b) =>
                {
                    b.AddConsole();
                })
                .Build();

            var entryPoint = host.Services.GetRequiredService<IEntryPoint>();
            entryPoint.Run().Wait();
        }
    }
}