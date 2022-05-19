using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.AspNetCore.Mvc;

namespace WWA_API.Controllers
{
    [ApiController]
    [Route("api")]
    public class WeatherForecastController : ControllerBase
    {

        private List<WeatherData> mockData = new()
        {
            new WeatherData() { Id = 1, RainProbability = 0, SunHours = 9, Date = DateTime.Now, Degree = 20.0, Location = "Bern" },
            new WeatherData() { Id = 1, RainProbability = 10, SunHours = 8, Date = DateTime.Now, Degree = 18.0, Location = "Zürich" },
            new WeatherData() { Id = 1, RainProbability = 11, SunHours = 5, Date = DateTime.Now, Degree = 16.0, Location = "Olten" },
            new WeatherData() { Id = 1, RainProbability = 5, SunHours = 3, Date = DateTime.Now, Degree = 15.0, Location = "Genf" },

            new WeatherData() { Id = 1, RainProbability = 0, SunHours = 9, Date = DateTime.Now.AddDays(1), Degree = 20.0, Location = "Bern" },
            new WeatherData() { Id = 1, RainProbability = 10, SunHours = 8, Date = DateTime.Now.AddDays(1), Degree = 18.0, Location = "Zürich" },
            new WeatherData() { Id = 1, RainProbability = 11, SunHours = 5, Date = DateTime.Now.AddDays(1), Degree = 16.0, Location = "Olten" },
            new WeatherData() { Id = 1, RainProbability = 5, SunHours = 3, Date = DateTime.Now.AddDays(1), Degree = 15.0, Location = "Genf" },

            new WeatherData() { Id = 1, RainProbability = 0, SunHours = 9, Date = DateTime.Now.AddDays(2), Degree = 20.0, Location = "Bern" },
            new WeatherData() { Id = 1, RainProbability = 10, SunHours = 8, Date = DateTime.Now.AddDays(2), Degree = 18.0, Location = "Zürich" },
            new WeatherData() { Id = 1, RainProbability = 11, SunHours = 5, Date = DateTime.Now.AddDays(2), Degree = 16.0, Location = "Olten" },
            new WeatherData() { Id = 1, RainProbability = 5, SunHours = 3, Date = DateTime.Now.AddDays(2), Degree = 15.0, Location = "Genf" },

            new WeatherData() { Id = 1, RainProbability = 0, SunHours = 9, Date = DateTime.Now.AddDays(3), Degree = 20.0, Location = "Bern" },
            new WeatherData() { Id = 1, RainProbability = 10, SunHours = 8, Date = DateTime.Now.AddDays(3), Degree = 18.0, Location = "Zürich" },
            new WeatherData() { Id = 1, RainProbability = 11, SunHours = 5, Date = DateTime.Now.AddDays(3), Degree = 16.0, Location = "Olten" },
            new WeatherData() { Id = 1, RainProbability = 5, SunHours = 3, Date = DateTime.Now.AddDays(3), Degree = 15.0, Location = "Genf" },

            new WeatherData() { Id = 1, RainProbability = 0, SunHours = 9, Date = DateTime.Now.AddDays(4), Degree = 20.0, Location = "Bern" },
            new WeatherData() { Id = 1, RainProbability = 10, SunHours = 8, Date = DateTime.Now.AddDays(4), Degree = 18.0, Location = "Zürich" },
            new WeatherData() { Id = 1, RainProbability = 11, SunHours = 5, Date = DateTime.Now.AddDays(4), Degree = 16.0, Location = "Olten" },
            new WeatherData() { Id = 1, RainProbability = 5, SunHours = 3, Date = DateTime.Now.AddDays(4), Degree = 15.0, Location = "Genf" },

            new WeatherData() { Id = 1, RainProbability = 0, SunHours = 9, Date = DateTime.Now.AddDays(5), Degree = 20.0, Location = "Bern" },
            new WeatherData() { Id = 1, RainProbability = 10, SunHours = 8, Date = DateTime.Now.AddDays(5), Degree = 18.0, Location = "Zürich" },
            new WeatherData() { Id = 1, RainProbability = 11, SunHours = 5, Date = DateTime.Now.AddDays(5), Degree = 16.0, Location = "Olten" },
            new WeatherData() { Id = 1, RainProbability = 5, SunHours = 3, Date = DateTime.Now.AddDays(5), Degree = 15.0, Location = "Genf" },
        };
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("forecast")]
        public IEnumerable<WeatherData> Get()
        {
            _logger.LogInformation("Request Get all forecasts");
            return mockData;

        }

        [HttpPost("forecast")]
        public ActionResult<WeatherData> GetByLocation(RequestDto req)
        {
            _logger.LogInformation($"Request get forecast for location {req.Location}");
            var data = mockData.FirstOrDefault(w => w.Location.ToLower() == req.Location.ToLower() && w.Date > req.StartDate && w.Date <= req.EndDate);
            if (data == null)
            {
                _logger.LogError($"Location {req.Location} not found");
                return NotFound();
            }
            return Ok(data);
        }


        [HttpGet(Name = "GetLocations")]
        [Route("locations")]
        public async Task<IEnumerable<string>> GetLocations()
        {
            _logger.LogInformation("Request Get all locations");
            return new[] { "Bern", "Zürich", "Olten", "Genf" };
        }


        [HttpGet(Name = "test")]
        [Route("test")]
        public object Test()
        {
            try
            {
               // var awsCredentials = new BasicAWSCredentials(id, secret);

                //var client = new AmazonDynamoDBClient(awsCredentials, Amazon.RegionEndpoint.EUCentral1);
                var client = new AmazonDynamoDBClient();
                var tableName = "TestDB";
                var table = Table.LoadTable(client, tableName);

                var conditions = new Dictionary<string, Condition>();
                // you can add scan conditions, or leave empty
                var allDocs = client.ScanAsync(tableName, conditions).GetAwaiter().GetResult();
                return allDocs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test failed");
                dynamic err = new { Message = ex.Message, StackTrace = ex.StackTrace };
                return Ok(err);
            }
        }
    }
}