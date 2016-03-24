using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using WebJobs.Extensions.Splunk.Services;

namespace WebJobs.Extensions.Splunk
{
    internal class SplunkAsyncCollector : IAsyncCollector<SplunkEvent>
    {
        private readonly ISplunkEventService _eventService;

        public SplunkAsyncCollector(ISplunkEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task AddAsync(SplunkEvent item, CancellationToken cancellationToken = new CancellationToken())
        {
            await _eventService.SendEventAsync(item);
        }

        public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(0);
        }
    }
}
