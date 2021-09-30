using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ArifsHumidor.HumidorSimulator;

namespace ArifsHumidor.ReadingReader
{
    public static class ReadingReader
    {
        [Function("ReadingReader")]
        [TableOutput("HumidorReadings", Connection = "AzureWebJobsStorage")]
        public static TableData Run([QueueTrigger("readings", Connection = "AzureWebJobsStorage")] HumidorStatus humidorStatus,
            FunctionContext context)
        {
            var logger = context.GetLogger("ReadingReader");
            logger.LogInformation($"C# Queue was triggered: {humidorStatus}");

            return new TableData{
                PartitionKey = humidorStatus.DeviceId,
                RowKey = $"{(DateTimeOffset.MaxValue.Ticks-humidorStatus.Timestamp.Ticks):d10}-{Guid.NewGuid():N}",
                Temperature = humidorStatus.Temperature,
                Humidity = humidorStatus.Humidity,
            };
        }

    }
        public class TableData
        {
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public double Temperature { get; set; }
            public double Humidity { get; set; }


        }
}
