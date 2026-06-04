using Microsoft.AspNetCore.Mvc;

namespace AutomaticBuild.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Kishan.Yogarajan", "Sujan.Yogarajan", "Lingesh.Jawahar", "Soundhar", "Bharathi", "Vishant", "Santhosh", "Yogarajan", "Yogaraja", "Yogaraj"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        //[HttpGet("{id}")]
        //public ActionResult<WeatherForecast> GetById(int id)
        //{
        //    if (id < 1 || id > 5)
        //    {
        //        return NotFound();
        //    }

        //    var forecast = new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(id)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    };

        //    return Ok(forecast);
        //}

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // Unused variable (SonarQube: Remove this unused 'x' local variable)
            int x = 0;

            // Magic number (SonarQube: Replace this magic number with a named constant)
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<WeatherForecast> GetById(int id)
        {
            // Empty catch block (SonarQube: Either log or rethrow this exception)
            try
            {
                if (id < 1 || id > 5)
                {
                    return NotFound();
                }

                var forecast = new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(id)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                };

                return Ok(forecast);
            }
            catch
            {
                // Do nothing
            }

            // Unreachable code (SonarQube: Remove this statement)
            return null;
        }

    }
}
