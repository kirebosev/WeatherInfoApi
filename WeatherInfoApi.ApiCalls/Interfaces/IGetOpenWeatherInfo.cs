using System.Threading.Tasks;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.ApiCalls.Interfaces
{
    public interface IGetOpenWeatherInfo
    {
        Task<OpenWeatherInfoResponse> GetWeatherInfo(string city);
    }
}
