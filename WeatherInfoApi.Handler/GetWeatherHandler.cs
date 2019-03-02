using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherInfoApi.ApiCalls.Interfaces;
using WeatherInfoApi.Handler.Interfaces;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.Handler
{
    public class GetWeatherHandler : IGetWeatherHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGetOpenWeatherInfo _getOpenWeatherInfo;
        private readonly IGetUserInfoByIpAddress _getUserInfoByIpAddress;
        private readonly IGetCityByIpAddress _getCityByIpAddress;
        private readonly ILogger _logger;

        public GetWeatherHandler(IHttpContextAccessor httpContextAccessor,
            IGetOpenWeatherInfo getOpenWeatherInfo, 
            IGetUserInfoByIpAddress getUserInfoByIpAddress,
            IGetCityByIpAddress getCityByIpAddress,
            ILogger<GetWeatherHandler> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _getOpenWeatherInfo = getOpenWeatherInfo;
            _getUserInfoByIpAddress = getUserInfoByIpAddress;
            _getCityByIpAddress = getCityByIpAddress;
            _logger = logger;
        }

        public async Task<WeatherInfoResponse> Execute()
        {
            var currentResponse = new WeatherInfoResponse()
            {
                Coord = new CoordResponse
                {
                    lat = new double(),
                    lon = new double()
                },
                MainInfo = new MainInfoResponse
                {
                    temp = new double(),
                    temp_max = new double(),
                    temp_min = new double()
                }                
            };
            try
            {
                var ipAddress = "92.53.44.70";//_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(); 

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    _logger.LogInformation( $"IpAddress -- {ipAddress}");

                    var getUserInfo = await _getCityByIpAddress.Execute(ipAddress);

                    if (getUserInfo.StausCode == System.Net.HttpStatusCode.OK)
                    {
                        var openWeatherInfoResponse = await _getOpenWeatherInfo.GetWeatherInfo(getUserInfo.city);

                        if (openWeatherInfoResponse.StausCode == HttpStatusCode.OK)
                        {
                            currentResponse.StausCode = openWeatherInfoResponse.StausCode;
                            currentResponse.City = openWeatherInfoResponse.name;
                            currentResponse.Coord.lat = openWeatherInfoResponse.coord.lat;
                            currentResponse.Coord.lon = openWeatherInfoResponse.coord.lon;
                            currentResponse.MainInfo.temp = openWeatherInfoResponse.main.temp;
                            currentResponse.MainInfo.temp_min = openWeatherInfoResponse.main.temp_min;
                            currentResponse.MainInfo.temp_max = openWeatherInfoResponse.main.temp_max;
                        }
                        else
                        {
                            currentResponse.StausCode = openWeatherInfoResponse.StausCode;
                            currentResponse.Errors = openWeatherInfoResponse.Errors;
                            _logger.LogError($"Error -- {JsonConvert.SerializeObject(currentResponse)}");
                        }
                    }
                    else
                    {
                        currentResponse.StausCode = getUserInfo.StausCode;
                        currentResponse.Errors = getUserInfo.Errors;
                        _logger.LogError($"Error -- {JsonConvert.SerializeObject(currentResponse)}");
                    }                    
                }
                else
                {
                    currentResponse.Errors.Add( "We can't detect the IpAddress");
                    currentResponse.StausCode = HttpStatusCode.InternalServerError;
                    _logger.LogError($"Error -- {JsonConvert.SerializeObject(currentResponse)}");
                }
            }
            catch(Exception ex)
            {
                currentResponse.Errors.Add(ex.Message);
                currentResponse.StausCode = System.Net.HttpStatusCode.InternalServerError;
                _logger.LogError($"Exception -- {JsonConvert.SerializeObject(currentResponse)}");
            }

            return currentResponse;
        }
    }
}
