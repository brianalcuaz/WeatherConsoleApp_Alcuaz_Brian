using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using WeatherConsoleApp_Alcuaz_Brian.Models;

namespace WeatherConsoleApp_Alcuaz_Brian.Data
{
    public class WeatherData : IWeatherData
    {
        public CurrentWeather GetWeatherData(WeatherFilter filt)
        {
            CurrentWeather result = null;
            var url = GenerateUrl(filt);

            if (!string.IsNullOrEmpty(url))
                result = GetCurrentWeather(url);

            return result;
        }

        private static string GenerateUrl(WeatherFilter filt)
        {
            var result = string.Empty;

            if (string.IsNullOrEmpty(filt.Url) || string.IsNullOrEmpty(filt.AccessKey) || string.IsNullOrEmpty(filt.ZipCode))
            {
                return result;
            }
            else
            {
                result = $"{filt.Url}current?access_key={filt.AccessKey}&query={filt.ZipCode}";
            }

            return result;
        }

        private static dynamic RequestWeather(string url)
        {
            object result = null;

            Uri uri = new Uri(url);
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = WebRequestMethods.Http.Get;

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                var data = reader.ReadToEnd();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(data);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = responseObj;
                }
            }

            return result;
        }

        private static CurrentWeather GetCurrentWeather(string url)
        {
            CurrentWeather result = null;

            var weatherObject = RequestWeather(url);

            if (weatherObject != null && weatherObject.request != null)
            {
                string type = weatherObject.request.type;

                if (type.ToLower() != "zipcode")
                {
                    return result;
                }
                else
                {
                    result = new CurrentWeather()
                    {
                        Type = weatherObject.request.type,
                        Precip = weatherObject.current.precip,
                        WindSpeed = weatherObject.current.wind_speed,
                        UvIndex = weatherObject.current.uv_index,
                    };
                }
            }

            return result;
        }
    }
}