// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using WebJobs.Extensions.Splunk;
using WebJobs.Extensions.Splunk.Services;
using System.IO;

namespace ExtensionsSample.Samples
{
    public static class SplunkSamples
    {
        
        public static void SendStringEventExplicit(
            [TimerTrigger("0:01", RunOnStartup = true)] TimerInfo timer,
            [SplunkHttpEventCollector] out SplunkEvent splunkEvent)
        {
            splunkEvent = new SplunkEvent()
            {
                Timestamp = DateTime.Now,
                Event = "Hello from a Webjob (Explicit)"
            };
        }

        public static void SendObjectEventExplicit(
            [TimerTrigger("0:01", RunOnStartup = true)] TimerInfo timer,
            [SplunkHttpEventCollector] out SplunkEvent splunkEvent)
        {
            splunkEvent = new SplunkEvent()
            {
                Timestamp = DateTime.Now,
                Event = new { Message = "Hello from a Webjob (Explict)" }
            };
        }

        public static void SendStringEventImplicit(
            [TimerTrigger("0:01", RunOnStartup = true)] TimerInfo timer,
            [SplunkHttpEventCollector] out string splunkEvent)
        {
            splunkEvent = "Hello from a Webjob (Implicit)";
        }

        public static void SendStringObjectImplicit(
            [TimerTrigger("0:01", RunOnStartup = true)] TimerInfo timer,
            [SplunkHttpEventCollector] out object splunkEvent)
        {
            splunkEvent = new { Message = "Hello from a Webjob (Implicit)" };
        }
        

        public static void SendStreamObjectImplict(
            [TimerTrigger("0:01", RunOnStartup = true)] TimerInfo timer,
            [SplunkHttpEventCollector] out Stream splunkEvent)
        {
            var message = "Hello from a Webjob (Implicit Stream)";
            var bytes = Encoding.ASCII.GetBytes(message);            
            var ms = new MemoryStream(bytes);
            splunkEvent = ms;
        }
    }
}
