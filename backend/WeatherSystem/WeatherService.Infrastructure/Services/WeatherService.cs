using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WeatherService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace WeatherService.Infrastructure.Services;

public class WeatherService : IWeatherService
{

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;

    public WeatherService(HttpClient httpClient, IConfiguration configuration, IDistributedCache cache)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _cache = cache;
    }

    public async Task<object> GetWeatherAsync(string city)
    {
        var cacheKey = $"weather:{city.ToLower()}";
        var cachedWeather = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedWeather))
        {
            using var doc = JsonDocument.Parse(cachedWeather);
            var root = doc.RootElement;
            return new
            {
                city = root.GetProperty("name").GetString(),
                lat = root.GetProperty("coord").GetProperty("lat").GetDouble(),
                lon = root.GetProperty("coord").GetProperty("lon").GetDouble(),
                temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                description = root.GetProperty("weather")[0].GetProperty("description").GetString()
            };
        }

        var baseUrl = _configuration["OpenWeather:BaseUrl"];
        var apiKey = _configuration["OpenWeather:ApiKey"];
        var url = $"{baseUrl}weather?q={city}&appid={apiKey}&units=metric";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            throw new Exception("City not found");
        var json = await response.Content.ReadAsStringAsync();

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(10)
        };
        await _cache.SetStringAsync(cacheKey, json, cacheOptions);

        using var doc2 = JsonDocument.Parse(json);
        var root2 = doc2.RootElement;
        return new
        {
            city = root2.GetProperty("name").GetString(),
            lat = root2.GetProperty("coord").GetProperty("lat").GetDouble(),
            lon = root2.GetProperty("coord").GetProperty("lon").GetDouble(),
            temperature = root2.GetProperty("main").GetProperty("temp").GetDouble(),
            humidity = root2.GetProperty("main").GetProperty("humidity").GetInt32(),
            description = root2.GetProperty("weather")[0].GetProperty("description").GetString()
        };
    }
}
