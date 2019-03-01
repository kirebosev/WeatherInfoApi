namespace WeatherInfoApi.ObjectModel.Responses
{
    public class WeatherInfoResponse : ErrorMessageResponse
    {
        public string City { get; set; }        
        public MainInfoResponse MainInfo { get; set; }
        public CoordResponse Coord { get; set; }
    }
}
