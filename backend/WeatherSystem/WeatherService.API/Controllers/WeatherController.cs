using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WeatherService.Application.Interfaces;

namespace WeatherService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase

{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

   
    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeather(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest("City is required.");

        try
        {
            var result = await _weatherService.GetWeatherAsync(city);
            Log.Information("Weather endpoint was called with city {City}", city);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }
}
