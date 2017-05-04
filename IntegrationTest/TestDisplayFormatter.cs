using SWT_ATM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.Core.Arguments;
using TransponderReceiver;
using Monitor = SWT_ATM.Monitor;

namespace IntegrationTest
{
    class TestDisplayFormatter
    {
        private TransponderDataFormat format;
        private CoordinateMapper mapper;
        private TrackSimulator simulator;
        private Log log;
        private Monitor monitor;
        private DisplayFormatter displayFormatter;

        [SetUp]
        public void Setup()
        {
            format = new TransponderDataFormat();
            mapper = new CoordinateMapper(format);
            simulator = new TrackSimulator(mapper, Substitute.For<ITransponderReceiver>());

            monitor = new Monitor();
            monitor.SetX(0, 5000);
            monitor.SetY(0, 5000);
            monitor.SetZ(500, 20000);

            log = new Log();
        }


        [Test]
        public void DisplayFormatterDoesEnteringNotification()
        {
            var notificationCenter = Substitute.For<INotificationCenter>();
            var positionCalc = Substitute.For<IPositionCalc>();
            var display = Substitute.For<IDisplay>();
            
            displayFormatter = new DisplayFormatter(display,positionCalc,notificationCenter);

            mapper.Attach(new Airspace(monitor, displayFormatter, log));

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            notificationCenter.Received(1).EnqueNotification(Arg.Is<List<string>>(d => d[0] == "ATR423" && d[1] == "ENTERING"));
            notificationCenter.Received(1).SetNotificationSignalHandle();

        }

        [Test]
        public void DisplayFormatterDoesLeavingNotification()
        {
            var notificationCenter = Substitute.For<INotificationCenter>();
            var positionCalc = Substitute.For<IPositionCalc>();
            var display = Substitute.For<IDisplay>();

            displayFormatter = new DisplayFormatter(display, positionCalc, notificationCenter);

            mapper.Attach(new Airspace(monitor, displayFormatter, log));

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;5001;5001;14000;20151006213456789";
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));


            notificationCenter.Received(1).EnqueNotification(Arg.Is<List<string>>(d => d[0] == "ATR423" && d[1] == "LEAVING"));
            notificationCenter.Received(2).SetNotificationSignalHandle();

        }

        [Test]
        public void DisplayFormatterDoesConflictingWarning()
        {
            var notificationCenter = Substitute.For<INotificationCenter>();
            var positionCalc = Substitute.For<IPositionCalc>();
            var display = Substitute.For<IDisplay>();

            displayFormatter = new DisplayFormatter(display, positionCalc, notificationCenter);

            mapper.Attach(new Airspace(monitor, displayFormatter, log));

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

            notificationCenter.Received(1).EnqueWarning(Arg.Any<List<List<string>>>());
            notificationCenter.Received(1).SetWarningsSignalHandle();

        }

        [Test]
        public void DisplayFormatterDoesConflictingEnteringWarning()
        {
            var notificationCenter = Substitute.For<INotificationCenter>();
            var positionCalc = Substitute.For<IPositionCalc>();
            var display = Substitute.For<IDisplay>();

            displayFormatter = new DisplayFormatter(display, positionCalc, notificationCenter);

            mapper.Attach(new Airspace(monitor, displayFormatter, log));

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

            notificationCenter.Received(1).EnqueWarning(Arg.Any<List<List<string>>>());
            notificationCenter.Received(1).SetWarningsSignalHandle();

        }

        [Test]
        public void DisplayFormatterDoesConflictingLeavingWarning()
        {
            var notificationCenter = Substitute.For<INotificationCenter>();
            var positionCalc = Substitute.For<IPositionCalc>();
            var display = Substitute.For<IDisplay>();

            displayFormatter = new DisplayFormatter(display, positionCalc, notificationCenter);

            monitor.SetX(0, 200);
            monitor.SetY(0, 200);
            monitor.SetZ(0, 500);

            mapper.Attach(new Airspace(monitor, displayFormatter, log));

            var testData = new List<string>();
            testData.Add("ATR423;10;10;499;20151006213456719");

            // ATR423 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;10;10;499;20151006213456789";

            // ATR423 INSIDE
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "AT422;10;10;499;20151006213456999";

            // AT422 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "AT422;10;10;501;20151006213456999";

            // AT422 CONFLICTING LEAVING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));
            Thread.Sleep(1000);
          //  notificationCenter.Received().EnqueWarning(Arg.Any<List<List<string>>>());
            notificationCenter.Received().SetWarningsSignalHandle();
        }


        [Test]
        public void DisplayFormatterDoesShowTracks()
        {
            var notificationCenter = Substitute.For<INotificationCenter>();
            var positionCalc = Substitute.For<IPositionCalc>();
            var display = Substitute.For<IDisplay>();

            displayFormatter = new DisplayFormatter(display, positionCalc, notificationCenter);

            mapper.Attach(new Airspace(monitor, displayFormatter, log));

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456719");

            // ATR423 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;501;501;1000;20151006213456789";

            // ATR423 INSIDE
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            display.Received(2).ShowTracks(Arg.Any<List<IEnumerable<string>>>());
            positionCalc.ReceivedWithAnyArgs().FormatTrackData(null, null);
        }


        [Test]
        public void DisplayFormatterDoesShowTracksWithPositionCalc()
        {
            var notificationCenter = Substitute.For<INotificationCenter>();
            var positionCalc = new PositionCalc();
            var display = Substitute.For<IDisplay>();

            displayFormatter = new DisplayFormatter(display, positionCalc, notificationCenter);

            mapper.Attach(new Airspace(monitor, displayFormatter, log));

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456719");

            // ATR423 ENTERING
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            testData[0] = "ATR423;501;501;1000;20151006213456789";

            // ATR423 INSIDE
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            display.Received(2).ShowTracks(Arg.Any<List<IEnumerable<string>>>());
        }
    }
}
