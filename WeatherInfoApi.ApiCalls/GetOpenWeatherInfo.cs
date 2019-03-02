using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using WeatherInfoApi.ApiCalls.Interfaces;
using WeatherInfoApi.ObjectModel.Config;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.ApiCalls
{
    public class GetOpenWeatherInfo : IGetOpenWeatherInfo
    {
        private readonly IRestClient _restClient;
        private readonly OpenWeatherConfig _openWeatherConfig;
        private readonly ILogger _logger;

        public GetOpenWeatherInfo(IRestClient restClient, 
            IOptions<OpenWeatherConfig> openWeatherConfig,
            ILogger<GetOpenWeatherInfo> logger)
        {
            _openWeatherConfig = openWeatherConfig.Value;
            _restClient = restClient;
            _logger = logger;
        }
        public async Task<OpenWeatherInfoResponse> GetWeatherInfo(string city)
        {
            try
            {
                _restClient.BaseUrl = new Uri($"{_openWeatherConfig.BaseUrl}?q={city}&units={_openWeatherConfig.Unit}&APPID={_openWeatherConfig.ApiKey}");

                var restRequest = new RestRequest(Method.GET);

                var taskCompletion = new TaskCompletionSource<IRestResponse>();

                _restClient.ExecuteAsync(restRequest, restResponse => taskCompletion.SetResult(restResponse));

                var response = await taskCompletion.Task;

                var currentResponse = JsonConvert.DeserializeObject<OpenWeatherInfoResponse>(response.Content);
                currentResponse.StausCode = response.StatusCode;
                if(currentResponse.StausCode != System.Net.HttpStatusCode.OK)
                {
                    currentResponse.Errors.Add(response.Content);
                    _logger.LogError($"Error -- {JsonConvert.SerializeObject(response.Content)}");
                }
                _logger.LogInformation($"End -- {JsonConvert.SerializeObject(currentResponse)}");
                return currentResponse;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error -- {ex.Message}");
                throw ex;
            }
        }
    }
}
