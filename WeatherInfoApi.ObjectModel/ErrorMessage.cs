using System.Collections.Generic;
using System.Net;

namespace WeatherInfoApi.ObjectModel
{
    public class ErrorMessageResponse
    {
        public List<string> Errors { get; set; }
        public HttpStatusCode StausCode { get; set; }
    }
}
