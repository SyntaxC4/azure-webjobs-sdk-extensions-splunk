// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using ExtensionsSample.Samples;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions;
using Microsoft.Azure.WebJobs.Host;
using WebJobs.Extensions.Splunk;
using WebJobs.Extensions.Splunk.Config;
//using WebJobsSandbox;

namespace ExtensionsSample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            JobHostConfiguration config = new JobHostConfiguration();

            // See https://github.com/Azure/azure-webjobs-sdk/wiki/Running-Locally for details
            // on how to set up your local environment
            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            config.UseTimers();
            config.UseCore();

            var splunkConfig = new SplunkConfiguration
            {
                HecHost = new Uri("http://localhost:8088"),
                Token = new Guid("3E712E99-63C5-4C5A-841D-592DD070DA51")
            };

            config.UseSplunk(splunkConfig);
            config.TypeLocator = new SamplesTypeLocator(typeof(SplunkSamples));
            config.HostId = "gbtest";
            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
