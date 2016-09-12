// Licensed under the MIT License. See License.txt in the project root for license information.

namespace WebJobs.Extensions.Splunk.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.IO;
    using Newtonsoft.Json.Linq;
    internal class SplunkEventService : ISplunkEventService
    {
        private readonly SplunkConfiguration _config;
        private readonly Uri _host;
        private readonly Guid _token;
        private HttpClient _client;
        private JsonSerializer _serializer;

        public SplunkEventService(SplunkConfiguration config)
        {
            _config = config;
            _host = new Uri(config.HecHost, "services/collector/event");
            _token = config.Token;
            _serializer = new JsonSerializer();
            _serializer.NullValueHandling = NullValueHandling.Ignore;
            _serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public async Task<HttpResponseMessage> SendEventAsync(SplunkEvent splunkEvent)
        {
            splunkEvent.Host = splunkEvent.Host ?? _config.Host;
            splunkEvent.Source = splunkEvent.Source ?? _config.Source;
            splunkEvent.SourceType = splunkEvent.SourceType ?? _config.SourceType;
            splunkEvent.Index = splunkEvent.Index ?? _config.Index;
            var json = new StringBuilder();

            using (var writer = new StringWriter(json))
            {
                _serializer.Serialize(writer, splunkEvent);
                var resp = await GetHttpClient().PostAsync(_host, new StringContent(json.ToString()));
                return resp;
            }
        }

        private HttpClient GetHttpClient()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Splunk", _token.ToString().ToUpper());
            }
            return _client;
        }

        internal static SplunkConfiguration CreateConfiguration(JObject metadata)
        {
            SplunkConfiguration splunkConfig = new SplunkConfiguration();

            splunkConfig = JsonConvert.DeserializeObject<SplunkConfiguration>((string)metadata);

            return splunkConfig;
        }

        internal static SplunkEvent JsonToEventConverter(JObject input)
        {
            SplunkEvent envelope = new SplunkEvent();

            envelope = JsonConvert.DeserializeObject<SplunkEvent>((string)input);

            return envelope;
        }
    }
}
