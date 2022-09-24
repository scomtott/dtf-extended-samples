using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DTFExtendedSamples.Server.Models
{
    [Serializable]
    public class SearchResult<T>
    {
        [JsonPropertyName("count")]
        public int Count { get; }
        
        [JsonPropertyName("Results")] public IList<T> Results { get; }
        
        [JsonConstructor]
        public SearchResult(int count, IList<T> results) => (Count, Results) = (count, results);
    }
}