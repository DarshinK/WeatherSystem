using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Domain.Entities;

public class WeatherRecord
{
    public int Id { get; set; }

    public string City { get; set; } = null!;

    public double Temperature { get; set; }

    public int Humidity { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

