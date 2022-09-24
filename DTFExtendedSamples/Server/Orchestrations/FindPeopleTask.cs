using DTFExtendedSamples.Server.Models;
using DTFExtendedSamples.Server.ThirdPartyServices;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class FindPeopleTask : TaskActivity<string, SearchResult<Person>>
    {
        private readonly StarWarsAPI _api;
        private readonly ILogger<FindPeopleTask> _logger;

        public FindPeopleTask(StarWarsAPI api, ILogger<FindPeopleTask> logger) => (_api, _logger) = (api, logger);

        protected override SearchResult<Person> Execute(TaskContext context, string person)
        {
            _logger.LogInformation("Finding characters with name: {person}", person);

            var results = _api.PeopleSearch(person);

            return results ?? throw new APIException();
        }
    }
}