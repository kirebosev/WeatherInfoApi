using System.Collections.Generic;

namespace WeatherInfoApi.ObjectModel.Responses
{
    public class OpenWeatherInfoResponse : ErrorMessageResponse
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string basic { get; set; }
        public MainInfo main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
}
