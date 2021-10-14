using Microsoft.Extensions.Configuration;
using System.Linq;
using WeatherConsoleApp_Alcuaz_Brian.Data;
using WeatherConsoleApp_Alcuaz_Brian.Models;

namespace WeatherConsoleApp_Alcuaz_Brian
{
    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration _config;
        private readonly IWeatherData _weatherData;
        public WeatherService(IConfiguration config,
            IWeatherData weatherData)
        {
            _config = config;
            _weatherData = weatherData;
        }

        public bool ValidateUser(string user)
        {
            var result = false;

            if (!string.IsNullOrEmpty(user))
            {
                // Check if name of users are present from the app settings
                var validUsers = _config.GetSection("ValidUsers").Get<string[]>();

                if (validUsers.Contains(user))
                    result = true;
            }

            return result;
        }

        public WeatherResponse WeatherResults(string zipCode)
        {
            var accessKey = _config.GetValue<string>("AccessKey");
            var url = _config.GetValue<string>("Url");
            var currentWeather = _weatherData.GetWeatherData(new WeatherFilter { AccessKey = accessKey, Url = url, ZipCode = zipCode });
            
            WeatherResponse result;
            if (currentWeather != null)
            {
                result = new WeatherResponse()
                {
                    GoOutside = currentWeather.Precip > 0 ? $"Precipitation value of {currentWeather.Precip} recorded. Please stay indoors"
                                    : $"Precipitation value of {currentWeather.Precip} recorded.You may go outside",
                    WearSunscreen = currentWeather.UvIndex > 3 ? $"UV Index value of {currentWeather.UvIndex}. Please wear suncreen"
                                    : $"UV Index value of {currentWeather.UvIndex}. You can go without sunscreen",
                    FlyKite = currentWeather.Precip > 0 && currentWeather.WindSpeed > 15 ? $"Precipitation value of {currentWeather.Precip} recorded. Your kite might get wet"
                                    : currentWeather.Precip == 0 && currentWeather.WindSpeed <= 15 ? $"Wind speed value of {currentWeather.WindSpeed} recorded. You will have a hard time flying your kite"
                                    : "Enjoy flying your kite"
                };
            }
            else
            {
                result = new WeatherResponse()
                {
                    Message = "We encountered an error processing your request, this might be caused by Zip Code that could not be found by the API. Please try again"
                };
            }

            return result;
        }
    }
}
