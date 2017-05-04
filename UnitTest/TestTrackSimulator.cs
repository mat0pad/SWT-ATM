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
        private ITransponderReceiver receiver;

        [SetUp]
        public void SetUp()
        {
            mapper = Substitute.For<ICoordinateMapper>();

            receiver = Substitute.For<ITransponderReceiver>();

            simulator = new TrackSimulator(mapper, receiver);             
        }

        [Test]
        public void MapTrackIsCalled()
        {
            var testData = new List<string>();
            testData.Add("test");

            var args = new RawTransponderDataEventArgs(testData);
            
            simulator.OnDataReceieved(null, args);
            mapper.Received().MapTrack(testData);
        }

        [Test]
        public void OnDataReceievedRaised()
        {
            var testData = new List<string>();

            testData.Add("test");

            var args = new RawTransponderDataEventArgs(testData);

            receiver.TransponderDataReady += simulator.OnDataReceieved;

            receiver.TransponderDataReady += Raise.EventWith(args);

            mapper.Received().MapTrack(testData);
        }

        [Test]
        public void StartSimulationNotReceived()
        {
            simulator.StartSimulation();

            mapper.DidNotReceiveWithAnyArgs().MapTrack(new List<string> { "test" });

        }


    }
}