// Licensed under the MIT License. See License.txt in the project root for license information.

namespace WebJobs.Extensions.Splunk
{
    using System;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// Defines the configuration options for the Splunk binding.
    /// </summary>
    public class SplunkConfiguration
    {
        internal const string SplunkHecHostSettingName = "AzureWebJobsSplunkHttpEventCollectorHost";
        internal const string SplunkTokenSettingName = "AzureWebJobsSplunkToken";
        internal const string SplunkHostSettingName = "AzureWebJobsSplunkHost";
        internal const string SplunkSourceSettingName = "AzureWebJobsSplunkSource";
        internal const string SplunkSourceTypeSettingName = "AzureWebJobsSplunkSourceType";
        internal const string SplunkIndexSettingName = "AzureWebJobsSplunkIndex";

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SplunkConfiguration()
        {
            var nameResolver = new DefaultNameResolver();

            HecHost = new Uri((nameResolver.Resolve(SplunkHecHostSettingName) ?? "https://localhost:8088"));
            Token = new Guid(nameResolver.Resolve(SplunkTokenSettingName) ?? Guid.NewGuid().ToString());
            Host = nameResolver.Resolve(SplunkHostSettingName);
            Source = nameResolver.Resolve(SplunkSourceSettingName);
            SourceType = nameResolver.Resolve(SplunkSourceTypeSettingName);
            Index = nameResolver.Resolve(SplunkIndexSettingName);
        }

        /// <summary>
        /// Uri of the Splunk Http Event Collector to send events to
        /// </summary>
        public Uri HecHost { get; set; }

        /// <summary>
        /// Token to use to authenticate with Splunk
        /// </summary>
        public Guid Token { get; set; }

        /// <summary>
        /// Host which will be assigned to each event
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Source which will be assigned to each event
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// SourceType which will be assigned to each event
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// Index where each event will be stored
        /// </summary>
        public string Index { get; set; }
    }
}
