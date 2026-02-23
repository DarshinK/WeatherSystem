using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherService.Domain.Entities;

public class WeatherHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string City { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}