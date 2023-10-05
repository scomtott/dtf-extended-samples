using System;

namespace DTFExtendedSamples.Server.Models
{
    [Serializable]
    public record DelayedAsyncWorkTaskInput(int delayMilliseconds, string message);
}