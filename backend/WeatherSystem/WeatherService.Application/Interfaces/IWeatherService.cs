using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Application.Interfaces;

public interface IWeatherService
{
    Task<object> GetWeatherAsync(string city);
}

