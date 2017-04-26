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
    public class TestCoordinateMapper
    {

        [SetUp]
        public void SetUp()
        {

        }


        [Test]
        public void MapperCallsFormatDataWithRelevantArgs()
        {
            var format = Substitute.For<ITransponderDataFormat>();
            var mapper = new CoordinateMapper(format);
            var simulator = new TrackSimulator(mapper, 10);

            var testData = new List<string>();
            testData.Add("test");
        
            var args = new RawTransponderDataEventArgs(testData);

            simulator.OnDataReceieved(null, args);

            format.Received(1).FormatData("test");
        }

        [Test]
        public void MapperNotifiesObserversWithData()
        {
            var airspace = Substitute.For<SWT_ATM.IObserver<Data>>();
            var format = new TransponderDataFormat();
            var mapper = new CoordinateMapper(format);
            var simulator = new TrackSimulator(mapper, 10);

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");

            var args = new RawTransponderDataEventArgs(testData);

            simulator.OnDataReceieved(null, args);

            airspace.Received(1).Update();
        }
    }
}
