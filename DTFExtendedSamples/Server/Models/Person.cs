using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DTFExtendedSamples.Server.Models
{
    [Serializable]
    public class Person
    {
        [JsonPropertyName("name")]
        public string Name { get;  }

        [JsonPropertyName("films")]
        public IList<string> Films { get; }

        [JsonConstructor]
        public Person(string name, IList<string> films) => (Name, Films) = (name, films);
        
    }
}