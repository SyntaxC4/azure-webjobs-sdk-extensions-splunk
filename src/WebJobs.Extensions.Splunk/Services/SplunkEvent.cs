using System;
using Newtonsoft.Json;

namespace WebJobs.Extensions.Splunk.Services
{
    public class SplunkEvent
    {
        public SplunkEvent()
        {
            Timestamp = DateTime.Now;
        }

        [JsonIgnore]
        public DateTime Timestamp { get; set; }

        [JsonProperty("time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double Time
        {
            get
            {
                double epochTime = (Timestamp.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
                return epochTime; // truncate to 3 digits after floating point
            }
        }

        //can be a string an object which is serializable by JSON.NET.
        [JsonProperty("event", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Event { get; set; }

        [JsonProperty("host", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Host { get; set; }

        [JsonProperty("source", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("sourcetype", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SourceType { get; set; }

        [JsonProperty("index", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Index { get; set; }
    }
}