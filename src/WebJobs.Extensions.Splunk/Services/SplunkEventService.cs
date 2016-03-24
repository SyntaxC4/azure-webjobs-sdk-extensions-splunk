using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebJobs.Extensions.Splunk.Services
{
    internal class SplunkEventService : ISplunkEventService
    {
        private readonly Uri _host;
        private readonly Guid _token;
        private HttpClient _client;

        public SplunkEventService(Uri host, Guid token)
        {
            _host = new Uri(host, "services/collector/event");
            _token = token;
        }

        public async Task<HttpResponseMessage> SendEventAsync(SplunkEvent splunkEvent)
        {
            var json = JsonConvert.SerializeObject(splunkEvent);
            return await GetHttpClient().PostAsync(_host, new StringContent(json));
        }

        private HttpClient GetHttpClient()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Splunk", _token.ToString());
            }
            return _client;
        }

    }
}
