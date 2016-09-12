using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Script.Extensibility;
using Newtonsoft.Json.Linq;
using WebJobs.Extensions.Splunk.Config;
using WebJobs.Extensions.Splunk.Services;

namespace WebJobs.Extensions.Splunk.Bindings
{
    public class SplunkScriptBindingProvider : ScriptBindingProvider
    {

        public SplunkScriptBindingProvider(JobHostConfiguration config, JObject hostMetadata, TraceWriter traceWriter)
            : base(config, hostMetadata, traceWriter) { }

        public override bool TryCreate(ScriptBindingContext context, out ScriptBinding binding)
        {
            if (context == null)
            {
                throw new ArgumentException("context");
            }

            binding = null;

            if (string.Compare(context.Type, "splunk", StringComparison.OrdinalIgnoreCase) == 0)
            {

                binding = new SplunkBinding(context);
            }

            return binding != null;
        }

        public override void Initialize()
        {
            var splunkConfig = SplunkEventService.CreateConfiguration(Metadata);

            Config.UseSplunk(splunkConfig);
        }

        public override bool TryResolveAssembly(string assemblyName, out Assembly assembly)
        {
            assembly = null;

            if (string.Compare(assemblyName, "Splunk", StringComparison.OrdinalIgnoreCase) == 0)
            {
                assembly = typeof(SplunkEvent).Assembly;
            }

            return assembly != null;
        }

        private class SplunkBinding : ScriptBinding
        {

            public SplunkBinding(ScriptBindingContext context) : base(context)
            {

            }

            public override Type DefaultType
            {
                get
                {
                    return typeof(IAsyncCollector<JObject>);
                }
            }

            public override Collection<Attribute> GetAttributes()
            {
                return new Collection<Attribute>
                {
                    new SplunkHttpEventCollectorAttribute
                    {
                        Host = Context.GetMetadataValue<string>("host"),
                        Index = Context.GetMetadataValue<string>("index"),
                        Source = Context.GetMetadataValue<string>("source"),
                        SourceType = Context.GetMetadataValue<string>("sourceType")
                    }
                };
            }
        }
    }
}
