using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherInfoApi.Handler.Interfaces;

namespace WeatherInfoApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class GetWeatherInfoController : ControllerBase
    {
        private readonly IGetWeatherHandler _getWeatherHandler;

        public GetWeatherInfoController(IGetWeatherHandler getWeatherHandler)
        {
            _getWeatherHandler = getWeatherHandler;
        }
        /// <summary>
        /// GET values
        /// </summary>
        /// <returns>string</returns>
        [HttpGet]
        [Route("getweather")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult> Get()
        {
            var result = await _getWeatherHandler.Execute();

            return StatusCode((int)result.StausCode, result);
        }
    }
}
