// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using WebJobs.Extensions.Splunk;
using WebJobs.Extensions.Splunk.Services;
using System.IO;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Splunk
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
            var host = GetSettingFromConfigOrEnvironment(SplunkHecHostSettingName);
            HecHost = host != null ? new Uri(host) : null;
            var token = GetSettingFromConfigOrEnvironment(SplunkTokenSettingName);
            Token = token != null ? new Guid(token) : Token; 
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
            var cm = context.Config.GetService<IConverterManager>();
            cm.AddConverter<object, SplunkEvent>(ConvertObject2SplunkEvent);
            cm.AddConverter<string, SplunkEvent>(ConvertString2SplunkEvent);
            cm.AddConverter<Stream, SplunkEvent>(ConvertStream2SplunkEvent);
            var provider = new SplunkAttributeBindingProvider(cm, this);
            context.Config.RegisterBindingExtension(provider);
        }

        internal static string GetSettingFromConfigOrEnvironment(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(value))
            {
                value = Environment.GetEnvironmentVariable(key);
            }

            return value;
        }

        private static SplunkEvent ConvertObject2SplunkEvent(object input)
        {
            var splunkEvent = new SplunkEvent
            {
                Event = input
            };
            return splunkEvent;
        }

        private static SplunkEvent ConvertString2SplunkEvent(string input)
        {
            var splunkEvent = new SplunkEvent
            {
                Event = input
            };
            return splunkEvent;
        }

        private static SplunkEvent ConvertStream2SplunkEvent(Stream input)
        {
            using (var reader = new StreamReader(input))
            {
                var splunkEvent = new SplunkEvent
                {
                    Event = reader.ReadToEnd()
                };
                return splunkEvent;
            }
        }
    }
}
