using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stage1.Services
{
    public class HttpService(HttpClient client, IConfiguration _config)
    {
        private readonly HttpClient _client = client;
        private readonly string _locationBaseUrl = "http://ip-api.com/json";
        private readonly string _weatherBaseUrl = "https://api.weatherapi.com/v1/current.json?";


        public async Task<(string?, string?)> GetIpDetails(string clientIp)
        {
            var locationData = await GetLocation(clientIp);
            if (locationData is null) return (null,null);
            Console.WriteLine(locationData);

            var city = locationData["city"]!;

            var weatherData = await GetWeatherDetails(locationData["lon"], locationData["lat"]);
            if (weatherData is null) return (null,null);

            var temperature = weatherData!?.current?.temp_c;
            return (city, temperature);
        }
        private async Task<Dictionary<string, string>?> GetLocation(string clientIp)
        {
            string url = _locationBaseUrl + $"/{clientIp}";
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                
                var content = await response.Content.ReadAsStringAsync();
                
                var contentObject =JsonSerializer.Deserialize<Dictionary<string, string>>(content)!;
                if (contentObject["status"]=="fail"){
                    return null;
                }
                return contentObject;
            }
            return null;
        }

        private async Task<dynamic?> GetWeatherDetails(string longitude, string latitude)
        {
            StringBuilder builder = new StringBuilder(_weatherBaseUrl);
            string key = _config.GetSection("WeatherApiKey").Value!;
            string[] queryParams = [$"q={latitude}", $"{longitude}", $"&key={key}"];
            builder.AppendJoin(null, queryParams);

            var response = await _client.GetAsync(builder.ToString());
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStream();
                return JsonSerializer.Deserialize<dynamic>(content);

            }
            return null;
        }
    }


}