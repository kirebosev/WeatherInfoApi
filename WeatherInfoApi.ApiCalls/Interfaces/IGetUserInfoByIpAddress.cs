using System.Threading.Tasks;
using WeatherInfoApi.ObjectModel.Responses;

namespace WeatherInfoApi.ApiCalls.Interfaces
{
    public interface IGetUserInfoByIpAddress
    {
        Task<UserInfoResponse> Execute(string IpAddress);
    }
}
