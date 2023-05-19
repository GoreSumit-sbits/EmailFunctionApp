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
using FunctionApp1.EmailServices.Models;

namespace FunctionApp1
{
    public static class EmailTrigger
    {
        [FunctionName("EmailTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

                using (var reader = new StreamReader(req.Body))
            try
            {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    var emailRequest = JsonConvert.DeserializeObject<EmailRequest>(requestBody);


                    await EmailSender.SendEmailAsync(emailRequest.Emails);

                return new OkObjectResult("Sent successfully");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "An error occurred while sending email.");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

    }
        public class EmailRequest
        {
            [JsonProperty("emails")]
            public List<string> Emails { get; set; }
        }
}
