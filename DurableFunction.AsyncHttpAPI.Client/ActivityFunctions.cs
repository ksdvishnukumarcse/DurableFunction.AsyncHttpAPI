using DurableFunction.AsyncHttpAPI.Client.Constants;
using DurableFunction.AsyncHttpAPI.Client.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace DurableFunction.AsyncHttpAPI.Client
{
    /// <summary>
    /// ActivityFunctions
    /// </summary>
    public class ActivityFunctions
    {
        /// <summary>
        /// Moderates the specified presentation.
        /// </summary>
        /// <param name="presentation">The presentation.</param>
        /// <param name="log">The log.</param>
        /// <returns>bool</returns>
        [FunctionName(AppConstants.Moderate)]
        public static bool Moderate([ActivityTrigger] Presentation presentation, ILogger log)
        {
            log.LogInformation($"Processing in {AppConstants.Moderate}.");
            return true;
        }

        /// <summary>
        /// Shortlists the specified presentation.
        /// </summary>
        /// <param name="presentation">The presentation.</param>
        /// <param name="log">The log.</param>
        /// <returns>bool</returns>
        [FunctionName(AppConstants.Shortlist)]
        public static bool Shortlist([ActivityTrigger] Presentation presentation, ILogger log)
        {
            log.LogInformation($"Processing in {AppConstants.Shortlist}.");
            return true;
        }

        /// <summary>
        /// Selects the specified presentation.
        /// </summary>
        /// <param name="presentation">The presentation.</param>
        /// <param name="log">The log.</param>
        /// <returns>bool</returns>
        [FunctionName(AppConstants.Select)]
        public static bool Select([ActivityTrigger] Presentation presentation, ILogger log)
        {
            log.LogInformation($"Processing in {AppConstants.Select}.");
            return true;
        }
    }
}
