using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeatherInfoApi.Handler.Interfaces;
using WeatherInfoApi.ObjectModel;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class GetWeatherInfoController : ControllerBase
    {
        private readonly IGetWeatherHandler _getWeatherHandler;
        private readonly ILogger _logger;

        public GetWeatherInfoController(IGetWeatherHandler getWeatherHandler, ILogger<GetWeatherInfoController> logger)
        {
            _getWeatherHandler = getWeatherHandler;
            _logger = logger;
        }
        /// <summary>
        /// GET values
        /// </summary>
        /// <returns>string</returns>
        [HttpGet]
        [Route("getweather")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WeatherInfoResponse), 200)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(ErrorMessageResponse), 500)]
        public async Task<ActionResult> Get()
        {
            _logger.LogInformation("Start");

            var result = await _getWeatherHandler.Execute();

            _logger.LogInformation($"End --- {JsonConvert.SerializeObject(result)}");

            return StatusCode((int)result.StausCode, result);
        }
    }
}
