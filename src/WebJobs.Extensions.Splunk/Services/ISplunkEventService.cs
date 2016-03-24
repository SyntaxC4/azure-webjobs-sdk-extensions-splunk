using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Splunk.Services
{
    internal interface ISplunkEventService
    {
        Task<HttpResponseMessage> SendEventAsync(SplunkEvent splunkEvent);
    }
}
