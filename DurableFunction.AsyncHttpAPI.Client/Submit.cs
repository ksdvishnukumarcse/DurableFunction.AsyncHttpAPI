using DurableFunction.AsyncHttpAPI.Client.Constants;
using DurableFunction.AsyncHttpAPI.Client.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace DurableFunction.AsyncHttpAPI.Client
{
    /// <summary>
    /// Submit
    /// </summary>
    public static class Submit
    {
        /// <summary>
        /// Runs the specified req.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="orchestrationClient">The orchestration client.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>IActionResult</returns>
        [FunctionName(AppConstants.Submit)]
        public static async Task<IActionResult> Run(
                                        [HttpTrigger(AuthorizationLevel.Anonymous, methods: "post", Route = "submit")]
                                        HttpRequest req,
                                        [DurableClient] IDurableOrchestrationClient orchestrationClient,
                                        ILogger logger)
        {
            logger.LogInformation("Submission received via Http");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var submission = JsonConvert.DeserializeObject<Presentation>(requestBody, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            var instanceId = await orchestrationClient.StartNewAsync(AppConstants.ProcessSubmission, submission);
            logger.LogInformation("Submission process started", instanceId);

            string checkStatusLocacion = string.Format("{0}://{1}/api/status/{2}", req.Scheme, req.Host, instanceId); // To inform the client where to check the status
            string message = $"Your submission has been received. To get the status, go to: {checkStatusLocacion}";

            // Create an Http Response with Status Accepted (202) to let the client know that the request has been accepted but not yet processed. 
            ActionResult response = new AcceptedResult(checkStatusLocacion, message); // The GET status location is returned as an http header
            req.HttpContext.Response.Headers.Add("retry-after", AppConstants.RetryTiming); // To inform the client how long to wait in seconds before checking the status

            return response;
        }
    }
}