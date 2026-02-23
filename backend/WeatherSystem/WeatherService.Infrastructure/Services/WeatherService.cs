using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherService.Application.Interfaces;
using WeatherService.Domain.Entities;

namespace WeatherService.Infrastructure.Services;

public class WeatherService : IWeatherService
{

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;
    private readonly IWeatherHistoryRepository _historyRepository;

    public WeatherService(HttpClient httpClient, IConfiguration configuration, IDistributedCache cache, IWeatherHistoryRepository history)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _cache = cache;
        _historyRepository = history;
    }

    public async Task<object> GetWeatherAsync(string city)
    {
        var cacheKey = $"weather:{city.ToLower()}";
        string json;

        var cachedWeather = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedWeather))
        {
            json = cachedWeather;
        }
        else
        {
            var baseUrl = _configuration["OpenWeather:BaseUrl"];
            var apiKey = _configuration["OpenWeather:ApiKey"];
            var url = $"{baseUrl}weather?q={city}&appid={apiKey}&units=metric";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("City not found");

            json = await response.Content.ReadAsStringAsync();

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _cache.SetStringAsync(cacheKey, json, cacheOptions);
        }

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var weatherHistory = new WeatherHistory
        {
            City = root.GetProperty("name").GetString(),
            Lat = root.GetProperty("coord").GetProperty("lat").GetDouble(),
            Lon = root.GetProperty("coord").GetProperty("lon").GetDouble(),
            Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
            Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
            Description = root.GetProperty("weather")[0].GetProperty("description").GetString(),
            CreatedAt = DateTime.UtcNow
        };

        await _historyRepository.SaveAsync(weatherHistory);

        return new
        {
            city = weatherHistory.City,
            lat = weatherHistory.Lat,
            lon = weatherHistory.Lon,
            temperature = weatherHistory.Temperature,
            humidity = weatherHistory.Humidity,
            description = weatherHistory.Description
        };
    }
}
