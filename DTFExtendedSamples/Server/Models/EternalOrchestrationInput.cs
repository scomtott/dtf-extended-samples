using System;

namespace DTFExtendedSamples.Server.Models
{
    [Serializable]
    public class EternalOrchestrationInput
    {
        // public prop to comply with serialization/deserialization across processes.
        public int IterationCounter { get; set; }

        public EternalOrchestrationInput(int count = 0) => (IterationCounter) = (count);
    }
}