using WeatherConsoleApp_Alcuaz_Brian.Models;

namespace WeatherConsoleApp_Alcuaz_Brian
{
    public interface IWeatherService
    {
        bool ValidateUser(string user);
        WeatherResponse WeatherResults(string zipCode);
    }
}