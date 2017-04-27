using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using SWT_ATM;
using TransponderReceiver;

namespace IntegrationTest
{
    [TestFixture]
    public class TestTrackSimulator
    {

        private TrackSimulator simulator;
        private ICoordinateMapper mapper;

        [SetUp]
        public void SetUp()
        {
            mapper = Substitute.For<ICoordinateMapper>();
            simulator = new TrackSimulator(mapper, 10);
        }

        [Test]
        public void SimulationSetupOnDataReceive()
        {
            simulator.StartSimulation();

            Thread.Sleep(2000); // Bad should instead await first until MapTrack called

            mapper.ReceivedWithAnyArgs().MapTrack(null);
        }

    }
}
