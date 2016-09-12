// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using ExtensionsSample.Samples;
using Microsoft.Azure.WebJobs;
using WebJobs.Extensions.Splunk;
using WebJobs.Extensions.Splunk.Config;

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
                Token = new Guid("A1BFE7A1-2FE2-40C3-873F-9BD35F2D71EA")
            };

            config.UseSplunk(splunkConfig);
            config.TypeLocator = new SamplesTypeLocator(typeof(SplunkSamples));
            config.HostId = "gbtest";

            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
