using DTFExtendedSamples.Core;
using DTFExtendedSamples.Server.Models;
using DTFExtendedSamples.Server.ThirdPartyServices;
using DurableTask.Core;
using Microsoft.Extensions.Logging;

namespace DTFExtendedSamples.Server.Orchestrations
{
    public class FindFilmTask: TaskActivity<string, Film>
    {
        
        private readonly StarWarsAPI _api;
        private readonly  ILogger<FindFilmTask> _logger;
        private readonly IRandomFailure _randomFailure = new CountBasedFailure();

        public FindFilmTask(StarWarsAPI api, ILogger<FindFilmTask> logger) => (_api, _logger) = (api, logger);
        
        protected override Film Execute(TaskContext context, string filmUrl)
        {
            _logger.LogInformation("Retrieving Film at URL {filmUrl}", filmUrl);
            _randomFailure.EventuallyFails();
            return _api.GetFilm(filmUrl) ?? throw new APIException();
        }
    }
}