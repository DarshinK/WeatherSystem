using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Application.DTOs;

public class WeatherDto
{
    public string City { get; set; } = null!;
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; } = null!;
}
