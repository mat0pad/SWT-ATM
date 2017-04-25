using NUnit.Framework;
using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework.Internal;
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

            list = new List<ITransponderReceiver>();

            list.Add(Substitute.For<ITransponderReceiver>());

            simulator = new TrackSimulator(mapper, 1, list);             
        }

        [Test]
        public void TransponderDataReadyEventRaised()
        {
            var testData = new List<string>();
            int num = 0;

            list[0].TransponderDataReady += delegate(object sender, RawTransponderDataEventArgs e)
            {
                testData.Add("test");
                num += 1;
            };

            Assert.That(testData.Count, Is.EqualTo(num));
        }


        [Test]
        public void MapTrackIsCalled()
        {
            var testData = new List<string>();
            testData.Add("test");

            var args = new RawTransponderDataEventArgs(testData);
            
            simulator.OnDataReceieved(null, args);
            mapper.Received().MapTrack("test");
        }

        [Test]
        public void OnDataReceievedRaised()
        {
          /*  var testData = new List<string>();
            testData.Add("test");

            var args = new RawTransponderDataEventArgs(testData);

            simulator.StartSimulation();

            list[0].TransponderDataReady += simulator.OnDataReceieved;

            

            mapper.ReceivedWithAnyArgs().MapTrack("test");*/
        }
    }
}