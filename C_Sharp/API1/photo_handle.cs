// using System.IO;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Azure.WebJobs.Extensions.Http;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Logging;
// using Microsoft.WindowsAzure.Storage.Blob;
// using System.Threading.Tasks;
// using System;
// using System.Linq;

// public static class PhotoHandleFunction
// {
//     [FunctionName("PhotoHandle")]
//     public static async Task<IActionResult> Run(
//         [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
//         [Blob("photos", FileAccess.Write, Connection = "AzureWebJobsStorage")] CloudBlobContainer container,
//         [Queue("photoprocess", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> messageCollector,
//         ILogger log)
//     {
//         log.LogInformation("Received a photo upload request.");

//         // Check if the container exists and create if it does not
//         await container.CreateIfNotExistsAsync();

//         // Assuming the file is sent as a form-data with a 'file' key
//         var file = req.Form.Files.GetFile("file");
//         if (file == null)
//         {
//             return new BadRequestObjectResult("Please upload a file.");
//         }

//         // Generate a unique name for the blob
//         string blobName = $"{Guid.NewGuid()}-{file.FileName}";
//         var blockBlob = container.GetBlockBlobReference(blobName);

//         // Upload the file to the blob
//         using (var stream = file.OpenReadStream())
//         {
//             await blockBlob.UploadFromStreamAsync(stream);
//         }

//         log.LogInformation($"Uploaded blob '{blobName}' to container '{container.Name}'.");

//         // Insert a message into the queue to process this blob later
//         await messageCollector.AddAsync(blobName);
//         log.LogInformation($"Enqueued blob name '{blobName}' for further processing.");

//         return new OkObjectResult($"File uploaded successfully: {blobName}");
//     }
// }

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
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API1
{
    public static class photoprocess
    {
        [FunctionName("photoprocess")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("api1", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> queueCollector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Retrieve the second secret from environment variable
            string secret2 = EnvironmentHelper.GetEnvironmentVariable("Secret2");

            // Log the value of secret2
            log.LogInformation($"Value of Secret 2: {secret2}");

            // Generate a message to be inserted into the queue
            string message = $"Value of Secret 2 from queue: {secret2}";

            // Add the message to the storage queue
            await queueCollector.AddAsync(message);

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string todayDate = DateTime.Today.ToString("yyyy-MM-dd");
            string responseMessage = string.IsNullOrEmpty(name)
                ? $"This HTTP triggered function executed successfully on {todayDate}. Pass a name in the query string or in the request body for a personalized response. Message added to queue."
                : $"Hello, {name}. This HTTP triggered function executed successfully on {todayDate}. Message added to queue.";

            return new OkObjectResult(responseMessage);
        }
    }
}


