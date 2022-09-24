using DTFExtendedSamples.Server.Models;
using RestSharp;

namespace DTFExtendedSamples.Server.ThirdPartyServices
{
    public class StarWarsAPI
    {
        private readonly RestClient _client;

        private const ushort TEN_SECONDS = 10000;

        public StarWarsAPI()
        {
            this._client = new RestClient(new RestClientOptions("https://swapi.dev/")
                {ThrowOnAnyError = true, MaxTimeout = TEN_SECONDS});
        }

        public SearchResult<Person>? PeopleSearch(string person)
        {
            return _client.Get<SearchResult<Person>>(new RestRequest($"api/people/?search={person}"));
        }
        
        public Film? GetFilm(string filmUrl)
        {
            return _client.Get<Film>(new RestRequest(filmUrl));
        }
        
    }
}