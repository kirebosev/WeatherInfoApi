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

        public GetUserInfoByIpAddress(IRestClient restClient, IOptions<IpStakConfig> ipStakConfig)
        {
            _restClient = restClient;
            _ipStakConfig = ipStakConfig.Value;
        }

        public async Task<UserInfoResponse> Execute(string IpAddress)
        {
            try
            {
                _restClient.BaseUrl = new Uri($"{_ipStakConfig.BaseUrl}{IpAddress}?access_key={_ipStakConfig.Apikey}");

                var restRequest = new RestRequest(Method.GET);

                var taskCompletion = new TaskCompletionSource<IRestResponse>();

                var restResult = _restClient.ExecuteAsync(restRequest, restResponse =>
                taskCompletion.SetResult(restResponse));

                var response = await taskCompletion.Task;

                var currentResponse = JsonConvert.DeserializeObject<UserInfoResponse>(response.Content);
                currentResponse.StausCode = response.StatusCode;

                if(currentResponse.StausCode != System.Net.HttpStatusCode.OK)
                {
                    currentResponse.Errors.Add(response.Content);
                }

                return currentResponse;

            } catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
