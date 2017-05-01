using SWT_ATM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.Core.Arguments;
using TransponderReceiver;

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
            simulator = new TrackSimulator(mapper, 3);

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

            notificationCenter.Received(1).EnqueNotification(Arg.Is<List<string>>(d => d[0] == "ATR423" && d[1] == "ENTERING"));
            notificationCenter.Received(1).SetNotificationSignalHandle();

        }
    }
}
