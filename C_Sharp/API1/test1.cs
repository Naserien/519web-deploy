// using System;
// using System.IO;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;

// namespace API1
// {
//     public static class test1
//     {
//         [FunctionName("test1")]
//         public static async Task<IActionResult> Run(
//             [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
//             ILogger log)
//         {
//             log.LogInformation("C# HTTP trigger function processed a request.");

//             string name = req.Query["name"];

//             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//             dynamic data = JsonConvert.DeserializeObject(requestBody);
//             name = name ?? data?.name;

//             string todayDate = DateTime.Today.ToString("yyyy-MM-dd");
//             string responseMessage = string.IsNullOrEmpty(name)
//                 ? $"This HTTP triggered function executed successfully on {todayDate}. Pass a name in the query string or in the request body for a personalized response."
//                 : $"Hello, {name}. This HTTP triggered function executed successfully on {todayDate}.";

//             return new OkObjectResult(responseMessage);
//         }
//     }
// }
using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class QueueFunction
{
    private readonly ILogger<QueueFunction> _logger;

    public QueueFunction(ILogger<QueueFunction> logger)
    {
        _logger = logger;
    }

    [FunctionName("test1")]
    [QueueOutput("output-queue")]
    public string[] Run([QueueTrigger("input-queue")] string myQueueItem, FunctionContext context)
    {
        _logger.LogInformation("C# queue trigger function processed a request.");

        // Process the incoming message
        // Example: Assume myQueueItem contains a JSON string representing an album
        // You can deserialize it to an Album object and process it as needed
        // For demonstration purposes, let's just log the incoming message
        _logger.LogInformation($"Incoming message: {myQueueItem}");

        // Use a string array to return more than one message.
        string[] messages = {
            $"Processed message: {myQueueItem}"
        };

        // Log the processed messages
        foreach (var message in messages)
        {
            _logger.LogInformation(message);
        }

        // Queue Output messages
        return messages;
    }
}


