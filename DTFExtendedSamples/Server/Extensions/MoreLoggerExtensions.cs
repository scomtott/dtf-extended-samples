using DurableTask.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTFExtendedSamples.Server.Extensions
{
    public static class MoreLoggerExtensions
    {
        public static void LogInformation(this ILogger logger, OrchestrationContext context, string? message, params object?[] args)
        {
            if (!context.IsReplaying)
            {
                logger.Log(LogLevel.Information, message, args);
            }
        }
    }
}
