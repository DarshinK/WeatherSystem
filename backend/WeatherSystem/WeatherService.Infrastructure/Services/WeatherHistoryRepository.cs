using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using WeatherService.Domain.Entities;
using WeatherService.Application.Interfaces;

namespace WeatherService.Infrastructure.Repositories;

public class WeatherHistoryRepository : IWeatherHistoryRepository
{
    private readonly IMongoCollection<WeatherHistory> _collection;

    public WeatherHistoryRepository(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["MongoSettings:ConnectionString"]);
        var database = client.GetDatabase(configuration["MongoSettings:DatabaseName"]);
        _collection = database.GetCollection<WeatherHistory>(
            configuration["MongoSettings:WeatherCollection"]);
    }

    public async Task SaveAsync(WeatherHistory history)
    {
        await _collection.InsertOneAsync(history);
    }

    public async Task<List<WeatherHistory>> GetRecentAsync(string city)
    {
        return await _collection
            .Find(x => x.City == city)
            .SortByDescending(x => x.CreatedAt)
            .Limit(10)
            .ToListAsync();
    }
}