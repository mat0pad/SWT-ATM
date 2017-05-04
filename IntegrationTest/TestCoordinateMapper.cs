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
        // Test: Tracksimulator -> Coordinatemapper -> IObserver<> == airspace
        //                              |
        //                     TransponderDataFormat

        [SetUp]
        public void SetUp()
        {

        }


        [Test]
        public void MapperCallsFormatDataWithRelevantArgs()
        {
            var format = Substitute.For<ITransponderDataFormat>();
            var mapper = new CoordinateMapper(format);
            var simulator = new TrackSimulator(mapper, Substitute.For<ITransponderReceiver>());

            var testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
 
            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            format.Received(1).FormatData("ATR423;39045;12932;14000;20151006213456789");
        }

        [Test]
        public void MapperNotifiesAttachedObserversWithData()
        {
            var airspace = Substitute.For<SWT_ATM.IObserver<List<Data>>>();
            var format = new TransponderDataFormat();
            var mapper = new CoordinateMapper(format);
            var simulator = new TrackSimulator(mapper, Substitute.For<ITransponderReceiver>());

            mapper.Attach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            airspace.Received(1).Update(Arg.Is<List<Data>>(d =>
                d[0].Altitude == 14000 &&
                d[0].Tag == "ATR423" && d[0].Timestamp == "20151006213456789" &&
                d[0].XCord == 39045 && d[0].YCord == 12932)
            );
        }


        [Test]
        public void MapperDoesNotNotifyDettachedObservers()
        {
            var airspace = Substitute.For<SWT_ATM.IObserver<List<Data>>>();
            var format = new TransponderDataFormat();
            var mapper = new CoordinateMapper(format);
            var simulator = new TrackSimulator(mapper, Substitute.For<ITransponderReceiver>());

            mapper.Attach(airspace);
            mapper.Deattach(airspace);

            var testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");

            simulator.OnDataReceieved(null, new RawTransponderDataEventArgs(testData));

            airspace.DidNotReceive().Update(Arg.Any<List<Data>>());
        }
    }
}
