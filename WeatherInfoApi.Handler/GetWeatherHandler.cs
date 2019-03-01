using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherInfoApi.ApiCalls.Interfaces;
using WeatherInfoApi.Handler.Interfaces;
using WeatherInfoApi.ObjectModel;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.Handler
{
    public class GetWeatherHandler : IGetWeatherHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGetOpenWeatherInfo _getOpenWeatherInfo;
        private readonly IGetUserInfoByIpAddress _getUserInfoByIpAddress;

        public GetWeatherHandler(IHttpContextAccessor httpContextAccessor, IGetOpenWeatherInfo getOpenWeatherInfo, IGetUserInfoByIpAddress getUserInfoByIpAddress)
        {
            _httpContextAccessor = httpContextAccessor;
            _getOpenWeatherInfo = getOpenWeatherInfo;
            _getUserInfoByIpAddress = getUserInfoByIpAddress;
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
                    temp_min = new int()
                }                
            };
            try
            {
                var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(); // "89.205.122.48";

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    var getUserInfo = await _getUserInfoByIpAddress.Execute(ipAddress);

                    var openWeatherInfoResponse = await _getOpenWeatherInfo.GetWeatherInfo(getUserInfo.city);

                    if(openWeatherInfoResponse.StausCode == System.Net.HttpStatusCode.OK)
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
                    }                    
                    
                }
                else
                {
                    currentResponse.Errors.Add( "We can't detect the IpAddress");
                    currentResponse.StausCode = System.Net.HttpStatusCode.InternalServerError;
                    return currentResponse;
                }
            }
            catch(Exception ex)
            {
                currentResponse.Errors.Add(ex.Message);
                currentResponse.StausCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return currentResponse;
        }
    }
}
