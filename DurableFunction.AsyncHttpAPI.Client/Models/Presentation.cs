using System;

namespace DurableFunction.AsyncHttpAPI.Client.Models
{
    /// <summary>
    /// Presentation
    /// </summary>
    public class Presentation
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the speaker.
        /// </summary>
        /// <value>
        /// The speaker.
        /// </value>
        public Speaker Speaker { get; set; }

        /// <summary>
        /// Gets or sets the track.
        /// </summary>
        /// <value>
        /// The track.
        /// </value>
        public string Track { get; set; }
    }
}
