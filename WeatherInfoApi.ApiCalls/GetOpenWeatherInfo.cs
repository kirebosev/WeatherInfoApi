using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using WeatherInfoApi.ApiCalls.Interfaces;
using WeatherInfoApi.ObjectModel;
using WeatherInfoApi.ObjectModel.Config;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.ApiCalls
{
    public class GetOpenWeatherInfo : IGetOpenWeatherInfo
    {
        private readonly IRestClient _restClient;
        private readonly OpenWeatherConfig _openWeatherConfig;

        public GetOpenWeatherInfo(IRestClient restClient, IOptions<OpenWeatherConfig> openWeatherConfig)
        {
            _openWeatherConfig = openWeatherConfig.Value;
            _restClient = restClient;
        }
        public async Task<OpenWeatherInfoResponse> GetWeatherInfo(string city)
        {
            try
            {
                _restClient.BaseUrl = new Uri($"{_openWeatherConfig.BaseUrl}?q={city}&units={_openWeatherConfig.Unit}&APPID={_openWeatherConfig.ApiKey}");

                var restRequest = new RestRequest(Method.GET);

                var taskCompletion = new TaskCompletionSource<IRestResponse>();

                var restResult = _restClient.ExecuteAsync(restRequest, restResponse =>
                taskCompletion.SetResult(restResponse));

                var response = await taskCompletion.Task;

                var currentResponse = JsonConvert.DeserializeObject<OpenWeatherInfoResponse>(response.Content);
                currentResponse.StausCode = response.StatusCode;
                if(currentResponse.StausCode != System.Net.HttpStatusCode.OK)
                {
                    currentResponse.Errors.Add(response.Content);
                }

                return currentResponse;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
