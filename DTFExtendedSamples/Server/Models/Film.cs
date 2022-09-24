using System;
using System.Reflection;
using System.Text.Json.Serialization;

namespace DTFExtendedSamples.Server.Models
{
    [Serializable]
    public class Film
    {
        [JsonPropertyName("title")]
        public string Title { get;  }

        [JsonPropertyName("episode_id")]
        public int EpisodeId { get;  }

        [JsonConstructor]
        public Film(string title, int episodeId) => (Title, EpisodeId) = (title, episodeId);

    }
}