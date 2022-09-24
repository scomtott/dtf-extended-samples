using System;
using System.Collections.Generic;

namespace DTFExtendedSamples.Server.Models
{
    [Serializable]
    public class WriteFileTaskInput
    {
        public WriteFileTaskInput(IList<Film> films, string targetLocation)
        {
            Films = films;
            TargetLocation = targetLocation;
        }

        public IList<Film> Films { get; }

        public string TargetLocation { get; }
    }
}