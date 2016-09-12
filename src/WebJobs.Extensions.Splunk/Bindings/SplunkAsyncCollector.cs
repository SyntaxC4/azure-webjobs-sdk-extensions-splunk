using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using WebJobs.Extensions.Splunk.Services;
using System;

namespace WebJobs.Extensions.Splunk
{
    internal class SplunkAsyncCollector : IAsyncCollector<SplunkEvent>
    {
        private readonly ISplunkEventService _eventService;

        public SplunkAsyncCollector(ISplunkEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task AddAsync(SplunkEvent item, CancellationToken cancellationToken = default(CancellationToken))
        {
            if(item == null)
            {
                throw new ArgumentException("item");
            }

            await _eventService.SendEventAsync(item);
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(0);
        }
    }
}
