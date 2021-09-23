using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ArifsHumidor.HumidorSimulator
{
    public class HumidorStatus
    {
        public string DeviceId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double Temperature { get; set; }   
        public double Humidity { get; set; }  
    }

    public static class HumidorSimulator
    {
        [Function("HumidorSimulator")]
        [QueueOutput("readings", Connection = "AzureWebJobsStorage")]
        public static HumidorStatus Run([TimerTrigger("*/5 * * * * *")] MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger("HumidorSimulator");
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        
        var rng = new Random();
        var reading = new HumidorStatus {
            DeviceId = "1",
            Timestamp = DateTimeOffset.Now,
            Temperature = rng.NextDouble() * 100,
            Humidity = rng.NextDouble() * 100,
        };

        return reading;

        }

    }

    public class MyInfo
    {

        public bool IsPastDue { get; set; }
    }

}
