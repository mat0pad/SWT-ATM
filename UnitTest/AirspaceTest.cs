using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SWT_ATM;

namespace UnitTest
{
    [TestFixture]
    class AirspaceTest
    {
        private Airspace _airspace;
        private ILog _log;
        private IDisplay _display;
        private IMonitor _monitor;


        [SetUp]
        public void Init()
        {
            _log = Substitute.For<ILog>();
            _display = Substitute.For<IDisplay>();
            _monitor = Substitute.For<IMonitor>();
            _airspace = new Airspace(_monitor,_display,_log);
        }


        [Test]
        public void UpdateMonitorEventTrackerTest()
        {
            var data = new Data("",0,0,0,"");

            _monitor.GetTracksInConflict().Returns(new List<Data>()); //need to set, otherwise Error
                                                                      // not important the test functionality
            _airspace.Update(data);

            _monitor.Received(1).EventTracker(data);
        }

        [Test]
        public void UpdateOnEnteringLogReceived()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.ENTERING);

            _airspace.Update(data);

            _log.Received(1).WriteNotification(data, false);
        }

        [Test]
        public void UpdateOnLeavingLogReceived()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.LEAVING);

            _airspace.Update(data);

            _log.Received(1).WriteNotification(data, true);
        }

        [Test]
        public void UpdateOnInsideLogReceived()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.INSIDE);

            _airspace.Update(data);

            _log.DidNotReceiveWithAnyArgs().WriteNotification(null, false);
        }

        [Test]
        public void UpdateOnConflictingMonitorReceived()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.CONFLICTING);
            _monitor.GetTracksInConflict().Returns(new List<Data>());

            _airspace.Update(data);

            _monitor.Received(1).GetTracksInConflict();
        }

        [Test]
        public void UpdateOnConflictingLogReceived()
        {
            var data = new Data("", 0, 0, 0, "");
            var list = new List<Data>();
            list.Add(data);
            list.Add(data);

            _monitor.EventTracker(data).Returns(EventType.CONFLICTING);
            _monitor.GetTracksInConflict().Returns(list);

            _airspace.Update(data);

            _log.Received(1).WriteWarning(list);
        }


    }


}
