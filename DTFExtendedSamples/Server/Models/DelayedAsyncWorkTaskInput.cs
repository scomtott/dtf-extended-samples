using System;

namespace DTFExtendedSamples.Server.Models
{
    [Serializable]
    public class DelayedAsyncWorkTaskInput
    {
        public DelayedAsyncWorkTaskInput(int delayMilliseconds)
        {
            DelayMilliseconds = delayMilliseconds;
        }

        public int DelayMilliseconds { get; set; }
    }
}