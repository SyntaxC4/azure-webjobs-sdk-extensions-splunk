using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using WebJobs.Extensions.Splunks;

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
            config.RegisterExtensionConfigProvider(splunkConfig);
        }
    }
}
