using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using WebJobs.Extensions.Splunk.Services;

namespace WebJobs.Extensions.Splunk
{
    internal class SplunkAttributeBindingProvider : IBindingProvider
    {
        private readonly IConverterManager _converterManager;
        private readonly SplunkConfiguration _config;
        private ISplunkEventService _eventService;

        public SplunkAttributeBindingProvider(IConverterManager converterManager, SplunkConfiguration config)
        {
            _converterManager = converterManager;
            _config = config;

            if (_config.HecHost == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                        "The Http Event Collector Host must be set either via a '{0}' app setting",
                        SplunkConfiguration.SplunkHecHostSettingName));
            }

            if (_config.Token == null)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                        "The Http Event Collector Token must be set via a '{0}' app setting",
                        SplunkConfiguration.SplunkTokenSettingName));
            }

            _eventService = new SplunkEventService(_config);
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            ParameterInfo parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<SplunkAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            Func<string, ISplunkEventService> invokeStringBinder = (invokeString) => _eventService;
    
            var binding = BindingFactory.BindCollector(
                parameter,
                _converterManager,
                (eventService, valueBindingContext) => new SplunkAsyncCollector(eventService),
                "Splunk",
                invokeStringBinder);

            return Task.FromResult(binding);
        }
    }
}
