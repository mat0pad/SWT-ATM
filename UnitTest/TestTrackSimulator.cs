using NUnit.Framework;
using System;
using System.Collections.Generic;
using NSubstitute;
using SWT_ATM;
using TransponderReceiver;

namespace UnitTest
{
    [TestFixture]
    public class TestTrackSimulator
    {

        private TrackSimulator simulator;
        private ICoordinateMapper mapper;
        private List<ITransponderReceiver> list;

        [SetUp]
        public void SetUp()
        {
            mapper = Substitute.For<ICoordinateMapper>();

            list.Add(Substitute.For<ITransponderReceiver>());

            simulator = new TrackSimulator(mapper, 1, list);             
        }

        [Test]
        public void OneTimeTearDown()
        {
            var testData = new List<string>();
            testData.Add("test");

            var args = new RawTransponderDataEventArgs(testData);

            list[0].TransponderDataReady += Raise.EventWith<RawTransponderDataEventArgs>(args);

            mapper.Received().MapTrack("test");
        }
    }
}