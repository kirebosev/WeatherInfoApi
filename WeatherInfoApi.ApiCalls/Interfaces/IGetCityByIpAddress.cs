using System.Threading.Tasks;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.ApiCalls.Interfaces
{
    public interface IGetCityByIpAddress
    {
        Task<GetCityByIpAddressResponse> Execute(string city);
    }
}
