using DTFExtendedSamples.Core;
using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DTFExtendedSamples.Client
{
    public class ClientConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string orchestration, string? argument, string? instance)
        {
            services.Configure<ClientInput>(input =>
            {
                input.Orchestration = orchestration;
                input.Arguments = argument;
                input.Instance = instance;
            });
            services.AddSingleton<IEntryPoint, ClientEntryPoint>();
            services.AddSingleton<TaskHubClient>();
        }
    }
}