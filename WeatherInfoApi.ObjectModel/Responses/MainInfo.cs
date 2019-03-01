using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherInfoApi.ObjectModel.Responses
{
    public class MainInfo
    {
        public double temp { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public int temp_min { get; set; }
        public double temp_max { get; set; }
    }
}
