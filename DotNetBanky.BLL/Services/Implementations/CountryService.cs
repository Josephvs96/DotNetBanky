

using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DotNetBanky.BLL.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private class CountryData
        {
            [JsonPropertyName("flags")]
            public Dictionary<string, string> Flags { get; set; }
        }

        HttpClient _httpClient;
        public CountryService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(configuration.GetSection("CountryApi").Value);
        }
        public async Task<string> GetCountryFlagUrl(string countryName)
        {
            var countryData = await _httpClient.GetFromJsonAsync<CountryData[]>(countryName);

            return countryData[0].Flags["png"];
        }
    }
}
