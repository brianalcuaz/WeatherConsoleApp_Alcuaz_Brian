using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using WeatherConsoleApp_Alcuaz_Brian.Data;
using WeatherConsoleApp_Alcuaz_Brian.Models;

namespace WeatherConsoleApp_Alcuaz_Brian
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IWeatherData, WeatherData>();
                    services.AddScoped<IWeatherService, WeatherService>();
                })
                .Build();

            var service = ActivatorUtilities.CreateInstance<WeatherService>(host.Services);

            Console.WriteLine(WeatherConstants.InputUserName);
            var userName = Console.ReadLine();
            var isValidUser = service.ValidateUser(userName);

            if (isValidUser)
            {
                Console.WriteLine(WeatherConstants.InputZipCode);
                var zipCode = Console.ReadLine();

                if (string.IsNullOrEmpty(zipCode))
                {
                    Console.WriteLine(WeatherConstants.InvalidZipCode + System.Environment.NewLine);
                    Main(args);
                }
                else
                {
                    var result = service.WeatherResults(zipCode);

                    if (string.IsNullOrEmpty(result.Message))
                    {
                        Console.WriteLine(result.GoOutside);
                        Console.WriteLine(result.WearSunscreen);
                        Console.WriteLine(result.FlyKite);
                    }
                    else
                    {
                        Console.WriteLine(result.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine(WeatherConstants.InvalidUser + System.Environment.NewLine);
                Main(args);
            }
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
