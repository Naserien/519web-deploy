using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System;
using System.Linq;

public static class PhotoHandleFunction
{
    [FunctionName("PhotoHandle")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
        [Blob("photos", FileAccess.Write, Connection = "AzureWebJobsStorage")] CloudBlobContainer container,
        [Queue("photoprocess", Connection = "AzureWebJobsStorage")] IAsyncCollector<string> messageCollector,
        ILogger log)
    {
        log.LogInformation("Received a photo upload request.");

        // Check if the container exists and create if it does not
        await container.CreateIfNotExistsAsync();

        // Assuming the file is sent as a form-data with a 'file' key
        var file = req.Form.Files.GetFile("file");
        if (file == null)
        {
            return new BadRequestObjectResult("Please upload a file.");
        }

        // Generate a unique name for the blob
        string blobName = $"{Guid.NewGuid()}-{file.FileName}";
        var blockBlob = container.GetBlockBlobReference(blobName);

        // Upload the file to the blob
        using (var stream = file.OpenReadStream())
        {
            await blockBlob.UploadFromStreamAsync(stream);
        }

        log.LogInformation($"Uploaded blob '{blobName}' to container '{container.Name}'.");

        // Insert a message into the queue to process this blob later
        await messageCollector.AddAsync(blobName);
        log.LogInformation($"Enqueued blob name '{blobName}' for further processing.");

        return new OkObjectResult($"File uploaded successfully: {blobName}");
    }
}
