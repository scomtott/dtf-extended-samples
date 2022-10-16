using System.IO;
using System.Text;
using System.Text.Json;
using DTFExtendedSamples.Server.Models;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class WriteFileTask: TaskActivity<WriteFileTaskInput, bool>
    {
        private delegate void WriteToDisk(string location, string? contents);
        private readonly ILogger<WriteFileTask> _logger;

        public WriteFileTask(ILogger<WriteFileTask> logger) => (_logger) = (logger);

        protected override bool Execute(TaskContext context, WriteFileTaskInput input)
        {
            var contents = new StringBuilder( $"ref: {input.ClientRef} - ");
            contents.Append(JsonSerializer.Serialize(input.Films));
            contents.AppendLine();
            
            _logger.LogInformation("About to write to {file} with mode '{mode}' and reference: {ref}", input.TargetLocation, input.WriteMode, input.ClientRef);
            
            WriteToDisk writer = input.WriteMode == 'a' ? File.AppendAllText : File.WriteAllText;

            writer(input.TargetLocation, contents.ToString());
            
            return true;
        }      
    }
}