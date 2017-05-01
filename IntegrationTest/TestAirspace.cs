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
            var displayFormatter = Substitute.For<IDisplayFormatter>();

            var airspace = new Airspace(monitor, displayFormatter, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");

            monitor.GetTracksInConflict().Returns(new List<Data>());

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

            var displayFormatter = Substitute.For<IDisplayFormatter>();

            var airspace = new Airspace(monitor, displayFormatter, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            log.Received(1).WriteNotification(Arg.Is<Data>(d =>
                d.Altitude == 14000 &&
                d.Tag == "ATR423" && d.Timestamp == "20151006213456789" &&
                d.XCord == 10 && d.YCord == 10), false);

            displayFormatter.Received(1).ShowNotification(Arg.Is<Data>(d =>
                d.Altitude == 14000 &&
                d.Tag == "ATR423" && d.Timestamp == "20151006213456789" &&
                d.XCord == 10 && d.YCord == 10), EventType.ENTERING);
        }

        [Test]
        public void TrackLeavingAirspaceLogWritesNotification()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0, 5000);
            monitor.SetY(0, 5000);
            monitor.SetZ(500, 20000);

            var displayFormatter = Substitute.For<IDisplayFormatter>();

            var airspace = new Airspace(monitor, displayFormatter, log);

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

            displayFormatter.Received(1).ShowNotification(Arg.Is<Data>(d =>
                d.Altitude == 14000 &&
                d.Tag == "ATR423" && d.Timestamp == "20151006213456789" &&
                d.XCord == 5001 && d.YCord == 5001), EventType.LEAVING);
        }

        [Test]
        public void TrackInsideAirspaceLogDoesNotWrite()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0, 5000);
            monitor.SetY(0, 5000);
            monitor.SetZ(500, 20000);

            var displayFormatter = Substitute.For<IDisplayFormatter>();

            var airspace = new Airspace(monitor, displayFormatter, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;501;501;14000;20151006213456789";
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            // Only called once since still inside should not write to log
            log.Received(1).WriteNotification(Arg.Any<Data>(), false);

            displayFormatter.Received(1).ShowNotification(Arg.Any<Data>(), EventType.ENTERING);
        }

        [Test]
        public void TwoTracksConflictingAirspaceLogWritesWarning()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0, 10001);
            monitor.SetY(0, 10001);
            monitor.SetZ(500, 20000);

            var displayFormatter = Substitute.For<IDisplayFormatter>();

            var airspace = new Airspace(monitor, displayFormatter, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456719");

            // ATR423 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;501;501;1000;20151006213456789";

            // ATR423 INSIDE
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));
        
            testData[0] = "AT422;10000;10000;1000;20151006213456799";

            // AT422 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));
 
            testData[0] = "AT422;1000;1000;1000;20151006213456999";

            // AT422 CONFLICTING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            log.Received(1).WriteWarning(Arg.Is<List<Data>>( d => 
            d[1].Tag == "AT422" && d[1].XCord == 1000 && d[1].YCord == 1000 &&
            d[1].Altitude == 1000 && d[1].Timestamp == "20151006213456999" &&
            d[0].Tag == "ATR423" && d[0].XCord == 501 && d[0].YCord == 501 &&
            d[0].Altitude == 1000 && d[0].Timestamp == "20151006213456789"
            ));

            displayFormatter.Received(1).ShowWarning(Arg.Any<List<List<Data>>>());
        }


        [Test]
        public void TwoTracksConflictingEnteringAirspaceLogWritesWarning()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0, 10001);
            monitor.SetY(0, 10001);
            monitor.SetZ(500, 20000);

            var displayFormatter = Substitute.For<IDisplayFormatter>();

            var airspace = new Airspace(monitor, displayFormatter, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456719");

            // ATR423 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;501;501;1000;20151006213456789";

            // ATR423 INSIDE
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "AT422;1000;1000;1000;20151006213456999";

            // AT422 CONFLICTING Entering
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            log.Received(1).WriteWarning(Arg.Is<List<Data>>(d =>
           d[1].Tag == "AT422" && d[1].XCord == 1000 && d[1].YCord == 1000 &&
           d[1].Altitude == 1000 && d[1].Timestamp == "20151006213456999" &&
           d[0].Tag == "ATR423" && d[0].XCord == 501 && d[0].YCord == 501 &&
           d[0].Altitude == 1000 && d[0].Timestamp == "20151006213456789"
            ));

            displayFormatter.Received(1).ShowWarning(Arg.Any<List<List<Data>>>());
        }


        [Test]
        public void TwoTracksConflictingLeavingAirspaceLogWritesWarning()
        {
            var log = Substitute.For<ILog>();

            var monitor = new Monitor();
            monitor.SetX(0, 200);
            monitor.SetY(0, 200);
            monitor.SetZ(0, 500);

            var displayFormatter = Substitute.For<IDisplayFormatter>();

            var airspace = new Airspace(monitor, displayFormatter, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;100;20151006213456719");

            // ATR423 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;20;20;100;20151006213456789";

            // ATR423 INSIDE
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "AT422;200;200;500;20151006213456999";

            // AT422 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "AT422;201;201;399;20151006213456999";

            // AT422 CONFLICTING LEAVING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            log.Received(1).WriteWarning(Arg.Is<List<Data>>(d =>
           d[1].Tag == "AT422" && d[1].XCord == 201 && d[1].YCord == 201 &&
           d[1].Altitude == 399 && d[1].Timestamp == "20151006213456999" &&
           d[0].Tag == "ATR423" && d[0].XCord == 20 && d[0].YCord == 20 &&
           d[0].Altitude == 100 && d[0].Timestamp == "20151006213456789"
            ));

            displayFormatter.Received(1).ShowWarning(Arg.Any<List<List<Data>>>());
        }


    }
}
