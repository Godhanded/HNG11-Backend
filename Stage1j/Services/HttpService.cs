using System.Text;
using System.Text.Json;
using Stage1.Models;


namespace Stage1.Services
{
    public class HttpService
    {
        public HttpService(HttpClient client, IConfiguration config)
        {
            _client=client;
            _config=config;
        }
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly string _locationBaseUrl = "http://ip-api.com/json";
        private readonly string _weatherBaseUrl = "https://api.weatherapi.com/v1/current.json?";


        public async Task<(string?, double?)> GetIpDetails(string clientIp)
        {
            var locationData = await GetLocation(clientIp);
            if (locationData is null) return (null,null);
           

            var city = locationData["city"]!;
            var lon=locationData["lon"];
            var lat=locationData["lat"];

            var weatherData = await GetWeatherDetails(lon.ToString(), lat.ToString());
            if (weatherData is null) return (null,null);

            var temperature= weatherData?.current.temp_c;
            
            return (city.ToString(), temperature);
        }
        private async Task<Dictionary<string, dynamic>?> GetLocation(string clientIp)
        {
            string url = _locationBaseUrl + $"/{clientIp}";
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                
                var content = await response.Content.ReadAsStringAsync();
                
                var contentObject =JsonSerializer.Deserialize<Dictionary<string, dynamic>>(content)!;
                if (contentObject["status"].ToString()=="fail"){
                    return null;
                }
                return contentObject;
            }
            return null;
        }

        private async Task<WeatherResponse?> GetWeatherDetails(string longitude, string latitude)
        {
            StringBuilder builder = new StringBuilder(_weatherBaseUrl);
            string key = _config.GetSection("WeatherApiKey").Value!;
            string[] queryParams = {$"q={latitude}", $",{longitude}", $"&key={key}"};
            builder.AppendJoin(null, queryParams);

            var response = await _client.GetAsync(builder.ToString());
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<WeatherResponse>();
                return content!;

            }
            return null;
        }
    }


}