using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.ApplicationInsights.TestFramework
{
    [TestClass]
    public class TelemetryClientTests2
    {

        [TestMethod]
        public void TestSetup()
        {
            // SETUP
            var mockChannel = new MockTelemetryChannel();
            var config = new TelemetryConfiguration
            {
                InstrumentationKey = "test-ikey",
                TelemetryChannel = mockChannel,
            };
            var client = new TelemetryClient(config);

            Assert.IsTrue(client.TelemetryConfiguration.InstrumentationKey == "test-ikey");

            // SCENARIO 1

            client.TrackEvent("event1");
            var test1 = (EventTelemetry)mockChannel.Telemetries[0];
            Assert.IsTrue(test1.Name == "event1");
            Assert.IsTrue(test1.Context.InstrumentationKey == "test-ikey");

            // SCENARIO 2

            // client.Context will be used to augment telemetry. this is not set by default. 
            Assert.IsTrue(client.Context.InstrumentationKey == "");
            // any settings here will be applied to all Telemetry that passes through this client.
            client.Context.InstrumentationKey = "different-ikey";
            
            client.TrackEvent("event2");
            var test2 = (EventTelemetry)mockChannel.Telemetries[1];
            Assert.IsTrue(test2.Name == "event2");
            Assert.IsTrue(test2.Context.InstrumentationKey == "different-ikey");
        }




        public class MockTelemetryChannel : ITelemetryChannel
        {
            public bool? DeveloperMode { get; set; } = true;
            public string EndpointAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public List<ITelemetry> Telemetries { get; set; } = new List<ITelemetry>();

            public void Dispose()
            {
                //throw new NotImplementedException();
            }

            public void Flush()
            {
                //throw new NotImplementedException();
            }

            public void Send(ITelemetry item) => this.Telemetries.Add(item);
        }
    }

}
