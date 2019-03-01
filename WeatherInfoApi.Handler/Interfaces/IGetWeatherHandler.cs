using System.Threading.Tasks;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.Handler.Interfaces
{
    public interface IGetWeatherHandler
    {
        Task<WeatherInfoResponse> Execute();
    }
}
