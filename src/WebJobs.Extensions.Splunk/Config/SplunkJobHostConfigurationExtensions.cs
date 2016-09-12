using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using WebJobs.Extensions.Splunk.Services;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host;

namespace WebJobs.Extensions.Splunk.Config
{
    public static class SplunkJobHostConfigurationExtensions
    {
        public static void UseSplunk(this JobHostConfiguration config, SplunkConfiguration splunkConfig = null)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (splunkConfig == null)
            {
                splunkConfig = new SplunkConfiguration();
            }
            config.RegisterExtensionConfigProvider(new SplunkExtensionConfig(splunkConfig));
        }

        internal class SplunkExtensionConfig : IExtensionConfigProvider
        {

            private readonly SplunkConfiguration _splunkConfig;
            private SplunkEventService _splunk;

            public SplunkExtensionConfig(SplunkConfiguration splunkConfig)
            {
                _splunkConfig = splunkConfig;
            }

            public void Initialize(ExtensionConfigContext context)
            {
                if (context == null)
                {
                    throw new ArgumentException("context");
                }

                if (_splunkConfig.HecHost == null)
                {
                    throw new InvalidOperationException($"The Http Event Collector Host must be set either via a '{SplunkConfiguration.SplunkHecHostSettingName}' app setting");
                }

                if (_splunkConfig.Token == null)
                {
                    throw new InvalidOperationException($"The Http Event Collector Token must be set via a '{SplunkConfiguration.SplunkTokenSettingName}' app setting");
                }

                _splunk = new SplunkEventService(_splunkConfig);

                IConverterManager converterManager = context.Config.GetService<IConverterManager>();
                converterManager.AddConverter<JObject, SplunkEvent>(SplunkEventService.JsonToEventConverter);

                INameResolver nameResolver = context.Config.GetService<INameResolver>();
                BindingFactory factory = new BindingFactory(nameResolver, converterManager);
                IBindingProvider outputProvider = factory.BindToAsyncCollector<SplunkHttpEventCollectorAttribute, SplunkEvent>((attr) =>
                {
                    return new SplunkAsyncCollector(_splunk);
                });

                IExtensionRegistry extensions = context.Config.GetService<IExtensionRegistry>();
                extensions.RegisterBindingRules<SplunkHttpEventCollectorAttribute>(outputProvider);
            }
        }
    }
}
