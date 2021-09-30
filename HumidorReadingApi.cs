using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ArifsHumidor.ReadingReader;

namespace ArifsHumidor.HumidorReadingApi
{
    public static class HumidorReadingApi
    {
        [Function("HumidorReadingApi")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
        [TableInput("HumidorReadings", "1", Take = 100)] TableData[] humidorReadings,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("HumidorReadingApi");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);

            var query = table.CreateQuery<TableData>();
            query.TakeCount = 100;

            var temperatures = (await table.ExecuteQuerySegmentedAsync(query, null)).ToList();

            if (humidorReadings.Any())
            {
                var result = new ApiResponse
                {
                    CurrentTemperature = humidorReadings.First().Temperature,
                    CurrentHumidity = humidorReadings.First().Humidity,
                    AverageTemperature = humidorReadings.Average(temp => temp.Temperature),
                    AverageHumidity = humidorReadings.Average(hum => hum.Humidity),
                };

                return new OkObjectResult(result);
            }

            return new OkResult();
        }
    }

    public class ApiResponse
    {
        public double CurrentTemperature { get; set; }
        public double CurrentHumidity { get; set; }
        public double AverageTemperature { get; set; }
        public double AverageHumidity { get; set; }


    }
}
