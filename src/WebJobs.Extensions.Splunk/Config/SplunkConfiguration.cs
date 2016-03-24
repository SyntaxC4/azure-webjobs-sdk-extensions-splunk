// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using WebJobs.Extensions.Splunk;

namespace WebJobs.Extensions.Splunks
{
    /// <summary>
    /// Defines the configuration options for the Splunk binding.
    /// </summary>
    public class SplunkConfiguration : IExtensionConfigProvider
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
            HecHost = new Uri(GetSettingFromConfigOrEnvironment(SplunkHecHostSettingName));
            Token = new Guid(GetSettingFromConfigOrEnvironment(SplunkTokenSettingName));
            Host = GetSettingFromConfigOrEnvironment(SplunkHostSettingName);
            Source = GetSettingFromConfigOrEnvironment(SplunkSourceSettingName);
            SourceType = GetSettingFromConfigOrEnvironment(SplunkSourceTypeSettingName);
            Index = GetSettingFromConfigOrEnvironment(SplunkIndexSettingName);
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
        /// Sourcetype which will be assigned to each event
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// Index where each event will be stored
        /// </summary>
        public string Index { get; set; }

        /// <inheritdoc />
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var converterManager = context.Config.GetService<IConverterManager>();
            var provider = new SplunkAttributeBindingProvider(converterManager, this);
            context.Config.RegisterBindingExtension(provider);
        }

        internal static string GetSettingFromConfigOrEnvironment(string key)
        {
            string value = null;

            if (string.IsNullOrEmpty(value))
            {
                value = ConfigurationManager.AppSettings[key];

                if (string.IsNullOrEmpty(value))
                {
                    value = Environment.GetEnvironmentVariable(key);
                }
            }

            return value;
        }
    }
}
