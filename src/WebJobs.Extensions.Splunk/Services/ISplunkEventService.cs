

namespace WebJobs.Extensions.Splunk.Services
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Linq;

    internal interface ISplunkEventService
    {
        Task<HttpResponseMessage> SendEventAsync(SplunkEvent splunkEvent);
    }
}
