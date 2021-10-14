using WeatherConsoleApp_Alcuaz_Brian.Models;

namespace WeatherConsoleApp_Alcuaz_Brian.Data
{
    public interface IWeatherData
    {
        CurrentWeather GetWeatherData(WeatherFilter filt);
    }
}