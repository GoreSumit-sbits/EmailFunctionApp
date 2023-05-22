using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FunctionApp1
{
    public static class AzureEmailTrigger
    {
        [FunctionName("AzureEmailTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var payload = JsonConvert.DeserializeObject<Payload>(requestBody);
            return new OkObjectResult(payload);
            //List<string> responseMessage = await EmailSender.AzureEmail(payload);

            //return new OkObjectResult(responseMessage);
        }
    }
}
