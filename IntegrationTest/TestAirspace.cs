using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using SWT_ATM;
using TransponderReceiver;

namespace IntegrationTest
{
    [TestFixture]
    public class TestAirspace
    {
        private TransponderDataFormat format;
        private CoordinateMapper mapper;
        private TrackSimulator simulator;

        [SetUp]
        public void Setup()
        {
            format = new TransponderDataFormat();
            mapper = new CoordinateMapper(format);
            simulator = new TrackSimulator(mapper, 3);
        }


        [Test]
        public void AirspaceCallsMonitorEventTrackerWithData()
        {
            var log = Substitute.For<ILog>();
            var monitor = Substitute.For<IMonitor>();
            var display = Substitute.For<IDisplay>();

            var airspace = new Airspace(monitor, display, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            monitor.Received(1).EventTracker(Arg.Is<Data>(d =>
                d.Altitude == 14000 &&
                d.Tag == "ATR423" && d.Timestamp == "20151006213456789" &&
                d.XCord == 39045 && d.YCord == 12932)
            );
        }

        [Test]
        public void TrackEnteringAirspaceLogWritesNotification()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0,5000);
            monitor.SetY(0, 5000);
            monitor.SetZ(500, 20000);

            var display = Substitute.For<IDisplay>();

            var airspace = new Airspace(monitor, display, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            log.Received(1).WriteNotification(Arg.Is<Data>(d =>
                d.Altitude == 14000 &&
                d.Tag == "ATR423" && d.Timestamp == "20151006213456789" &&
                d.XCord == 10 && d.YCord == 10), false);
        }

        [Test]
        public void TrackLeavingAirspaceLogWritesNotification()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0, 5000);
            monitor.SetY(0, 5000);
            monitor.SetZ(500, 20000);

            var display = Substitute.For<IDisplay>();

            var airspace = new Airspace(monitor, display, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;5001;5001;14000;20151006213456789";
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            log.Received(1).WriteNotification(Arg.Is<Data>(d =>
                d.Altitude == 14000 &&
                d.Tag == "ATR423" && d.Timestamp == "20151006213456789" &&
                d.XCord == 5001 && d.YCord == 5001), true);
        }


        [Test]
        public void TrackInsideAirspaceLogDoesNotWrite()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0, 5000);
            monitor.SetY(0, 5000);
            monitor.SetZ(500, 20000);

            var display = Substitute.For<IDisplay>();

            var airspace = new Airspace(monitor, display, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;501;501;14000;20151006213456789";
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            // Only called once since still inside should not write to log
            log.Received(1).WriteNotification(Arg.Any<Data>(), false);
        }

        [Test]
        public void TwoTracksConflictingAirspaceLogWritesWarning()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0, 10000);
            monitor.SetY(0, 10000);
            monitor.SetZ(500, 20000);

            var display = Substitute.For<IDisplay>();

            var airspace = new Airspace(monitor, display, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;501;501;1000;20151006213456789";
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));
        
            testData[0] = ("AT422;10000;10000;1000;20151006213456789");
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));
 
            testData[0] = "AT422;1000;1000;1000;20151006213456789";
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            log.Received(1).WriteWarning(Arg.Any<List<Data>>());
        }
    }
}
