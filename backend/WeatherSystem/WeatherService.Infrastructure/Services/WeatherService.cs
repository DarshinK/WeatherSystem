using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using WeatherService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace WeatherService.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<object> GetWeatherAsync(string city)
    {
        var baseUrl = _configuration["OpenWeather:BaseUrl"];
        var apiKey = _configuration["OpenWeather:ApiKey"];

        var url = $"{baseUrl}weather?q={city}&appid={apiKey}&units=metric";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception("City not found");

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
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
}
