using Microsoft.AspNetCore.Mvc;

namespace Communication.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    [HttpGet(Name = "GetWeatherForecast")]
    public IResult Get()
    {
        return Results.Ok();
    }
}
