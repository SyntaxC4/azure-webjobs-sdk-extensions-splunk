// Licensed under the MIT License. See License.txt in the project root for license information.

namespace WebJobs.Extensions.Splunk
{
    using Microsoft.Azure.WebJobs;
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    public class SplunkHttpEventCollectorAttribute : Attribute
    {
        /// <summary>
        /// Host which will be assigned to each event
        /// </summary>
        [AutoResolve]
        public string Host { get; set; }

        /// <summary>
        /// Source which will be assigned to each event
        /// </summary>
        [AutoResolve]
        public string Source { get; set; }

        /// <summary>
        /// SourceType which will be assigned to each event
        /// </summary>
        [AutoResolve]
        public string SourceType { get; set; }

        /// <summary>
        /// Index where each event will be stored
        /// </summary>
        [AutoResolve]
        public string Index { get; set; }
    }
}