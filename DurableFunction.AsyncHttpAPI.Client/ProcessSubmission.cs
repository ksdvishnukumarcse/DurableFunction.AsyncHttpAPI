using DurableFunction.AsyncHttpAPI.Client.Constants;
using DurableFunction.AsyncHttpAPI.Client.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DurableFunction.AsyncHttpAPI.Client
{
    public static class ProcessSubmission
    {
        /// <summary>
        /// Durable Functions Orchestration
        /// Receives a Call-for-Speaker submissions and control the approval workflow. 
        /// Updates the Orchestration Instance Custom Status as it progresses. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <returns>bool</returns>
        [FunctionName(AppConstants.ProcessSubmission)]
        public static async Task<bool> RunOrchestrator(
                                    [OrchestrationTrigger] IDurableOrchestrationContext context,
                                    ILogger logger)
        {
            Presentation presentation = context.GetInput<Presentation>();
            presentation.Id = context.InstanceId;
            string stage;
            string status;
            bool isTrackingEvent = true;

            bool activityStatus;

            stage = "Moderation";
            // Set the custom status for the ochestration instance. 
            // This can be any serialisable object. In this case it is just a string. 
            context.SetCustomStatus(stage);
            activityStatus = await context.CallActivityAsync<bool>(AppConstants.Moderate, presentation);

            if (activityStatus)
            {
                stage = "Shortlisting";
                context.SetCustomStatus(stage);
                activityStatus = await context.CallActivityAsync<bool>(AppConstants.Shortlist, presentation);

                if (activityStatus)
                {
                    stage = "Selection";
                    context.SetCustomStatus(stage);
                    activityStatus = await context.CallActivityAsync<bool>(AppConstants.Select, presentation);
                }
            }
            if (activityStatus)
                status = "Approved";
            else
                status = "Rejected";

            context.SetCustomStatus(status);
            logger.LogInformation("Submission has been {status} at stage {stage}. {presenter}, {title}, {track}, {speakerCountry}, {isTrackingEvent}", status, stage, presentation.Speaker.Email, presentation.Title, presentation.Track, presentation.Speaker.Country, isTrackingEvent);

            return activityStatus;
        }
    }
}
