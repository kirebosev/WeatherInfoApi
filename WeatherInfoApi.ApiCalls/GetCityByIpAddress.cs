using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using WeatherInfoApi.ApiCalls.Interfaces;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.ApiCalls
{
    public class GetCityByIpAddress : IGetCityByIpAddress
    {
        private readonly IRestClient _restClient;
        private readonly ILogger _logger;

        public GetCityByIpAddress(IRestClient restClient, ILogger<GetCityByIpAddress> logger)
        {
            _restClient = restClient;
            _logger = logger;
        }
        public async Task<GetCityByIpAddressResponse> Execute(string city)
        {
            try
            {
                _restClient.BaseUrl = new Uri($"http://ip-api.com/json/{city}");

                var restRequest = new RestRequest(Method.GET);

                var taskCompletion = new TaskCompletionSource<IRestResponse>();

                _restClient.ExecuteAsync(restRequest, restResponse => taskCompletion.SetResult(restResponse));

                var response = await taskCompletion.Task;

                var currentResponse = JsonConvert.DeserializeObject<GetCityByIpAddressResponse>(response.Content);
                currentResponse.StausCode = response.StatusCode;

                if (currentResponse.StausCode != System.Net.HttpStatusCode.OK)
                {
                    currentResponse.Errors.Add(response.Content);
                    _logger.LogError($"Error -- {JsonConvert.SerializeObject(response.Content)}");
                }

                _logger.LogInformation($"Success -- {JsonConvert.SerializeObject(currentResponse)}");
                return currentResponse;
            } 
            catch(Exception ex)
            {
                _logger.LogError($"Exception -- {ex.Message}");
                throw ex;
            }
        }
    }
}
