using System.Collections.Generic;
using System.Linq;
using DTFExtendedSamples.Core;
using DTFExtendedSamples.Server.Orchestrations;
using DTFExtendedSamples.Server.ThirdPartyServices;
using DurableTask.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server
{
    
    public class ServerConfiguration
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TaskHubClient>();

            #region Orchestrations

            services.AddSingleton<TaskOrchestration, RetrieveFilmsOrchestration>();
            services.AddSingleton<TaskOrchestration, EternalOrchestration>();
            services.AddSingleton<TaskOrchestration, DelayedWorkOrchestration>();
            services.AddSingleton<TaskActivity, FindPeopleTask>();
            services.AddSingleton<TaskActivity, FindFilmTask>();
            services.AddSingleton<TaskActivity, WriteFileTask>();
            services.AddSingleton<TaskActivity, EternalOrchestrationTask>();
            services.AddSingleton<TaskActivity, DelayedAsyncWorkTask>();
            services.AddSingleton<TaskActivity, RaiseEventTask>();
            services.AddSingleton<TaskActivity, ScheduleDelayedWorkOrchestrationTask>();

            #endregion

            services.AddSingleton<StarWarsAPI>();
            services.AddSingleton<IEntryPoint, ServerEntryPoint>();
            
            services.AddSingleton(provider =>
            {
                var service = provider.GetService<IOrchestrationService>();
                var loggerFactory = provider.GetService<ILoggerFactory>();
                var taskHubWorker = new TaskHubWorker(service, loggerFactory);

                var orchestrations = provider.GetRequiredService<IEnumerable<TaskOrchestration>>()
                    .Select(orchestration => new NameValueObjectCreator<TaskOrchestration>(orchestration.GetType().Name, "V1", orchestration))
                    .ToArray();

                taskHubWorker.AddTaskOrchestrations(orchestrations);
                taskHubWorker.AddTaskActivities(provider.GetRequiredService<IEnumerable<TaskActivity>>().ToArray());
                
                return taskHubWorker;
            });
            
        }
    }
}