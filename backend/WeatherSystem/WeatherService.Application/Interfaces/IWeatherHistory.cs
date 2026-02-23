using WeatherService.Domain.Entities;

public interface IWeatherHistoryRepository
{
    Task SaveAsync(WeatherHistory history);
    Task<List<WeatherHistory>> GetRecentAsync(string city);
}