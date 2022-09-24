using System.IO;
using System.Text.Json;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class WriteFileTask: TaskActivity<WriteFileTaskInput, bool>
    {
        private readonly ILogger<WriteFileTask> _logger;

        public WriteFileTask(ILogger<WriteFileTask> logger) => (_logger) = (logger);

        protected override bool Execute(TaskContext context, WriteFileTaskInput input)
        {
            var serializedFilms = JsonSerializer.Serialize(input.Films);
            
            _logger.LogInformation("About to write to {file}", input.TargetLocation);
            
            File.WriteAllText(input.TargetLocation, serializedFilms);
            
            return true;
        }
    }
}