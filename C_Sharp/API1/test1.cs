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
    public static class test1
    {
        [FunctionName("test1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Retrieve the first secret from environment variable
            string secret1 = EnvironmentHelper.GetEnvironmentVariable("SECRET_NAME_1");

            // Retrieve the second secret from environment variable
            string secret2 = EnvironmentHelper.GetEnvironmentVariable("SECRET_NAME_2");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string todayDate = DateTime.Today.ToString("yyyy-MM-dd");
            string responseMessage = string.IsNullOrEmpty(name)
                ? $"This HTTP triggered function executed successfully on {todayDate}. Pass a name in the query string or in the request body for a personalized response. Secret 1: {secret1}, Secret 2: {secret2}"
                : $"Hello, {name}. This HTTP triggered function executed successfully on {todayDate}. Secret 1: {secret1}, Secret 2: {secret2}";

            return new OkObjectResult(responseMessage);
        }
    }
}

