using SWT_ATM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace IntegrationTest
{
    class TestDisplayFormatter
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
        public void TrackEnteringAirspaceDisplayFormatterNotification()
        {
            var log = Substitute.For<ILog>();

            var notificationCenter = Substitute.For<INotificationCenter>();
            var positionCalc = Substitute.For<IPositionCalc>();
            var display = Substitute.For<IDisplay>();

            var monitor = new Monitor();
            monitor.SetX(0, 5000);
            monitor.SetY(0, 5000);
            monitor.SetZ(500, 20000);

            var displayFormatter = new DisplayFormatter(display,positionCalc,notificationCenter);

            var airspace = new Airspace(monitor, displayFormatter, log);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;10;10;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            

        }
    }
}
