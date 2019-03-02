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
    public class GetUserInfoByIpAddress : IGetUserInfoByIpAddress
    {
        private readonly IRestClient _restClient;
        private readonly IpStakConfig _ipStakConfig;
        private readonly ILogger _logger;

        public GetUserInfoByIpAddress(IRestClient restClient,
            IOptions<IpStakConfig> ipStakConfig,
            ILogger<GetUserInfoByIpAddress> logger)
        {
            _restClient = restClient;
            _ipStakConfig = ipStakConfig.Value;
            _logger = logger;
        }

        public async Task<UserInfoResponse> Execute(string ipAddress)
        {
            try
            {
                _restClient.BaseUrl = new Uri($"{_ipStakConfig.BaseUrl}{ipAddress}?access_key={_ipStakConfig.Apikey}");

                var restRequest = new RestRequest(Method.GET);

                var taskCompletion = new TaskCompletionSource<IRestResponse>();

                _restClient.ExecuteAsync(restRequest, restResponse => taskCompletion.SetResult(restResponse));

                var response = await taskCompletion.Task;

                var currentResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response.Content);
                currentResponse.StausCode = response.StatusCode;

                if(currentResponse.StausCode != System.Net.HttpStatusCode.OK)
                {
                    currentResponse.Errors.Add(response.Content);
                    _logger.LogError($"Error -- {JsonConvert.SerializeObject(response.Content)}");
                }

                _logger.LogInformation($"Success -- {JsonConvert.SerializeObject(currentResponse)}");
                return currentResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception -- {ex.Message}");
                throw ex;
            }
        }
    }
}
