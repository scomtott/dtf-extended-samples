using System;
using System.Collections.Generic;

namespace DTFExtendedSamples.Server.Models
{
    [Serializable]
    public class WriteFileTaskInput
    {
        public WriteFileTaskInput(IList<Film> films, string targetLocation, char writeMode, string? clientRef)
        {
            Films = films;
            TargetLocation = targetLocation;
            WriteMode = writeMode;
            ClientRef = clientRef;
        }

        public IList<Film> Films { get; }

        public string TargetLocation { get; }

        public char WriteMode { get; }
        public string? ClientRef { get; }
    }
}