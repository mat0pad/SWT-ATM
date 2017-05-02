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
        private IDisplayFormatter _displayFormatter;
        private IMonitor _monitor;


        [SetUp]
        public void Init()
        {
            _log = Substitute.For<ILog>();
            _displayFormatter = Substitute.For<IDisplayFormatter>();

            _monitor = Substitute.For<IMonitor>();
            _airspace = new Airspace(_monitor,_displayFormatter,_log);
        }


        [Test]
        public void UpdateMonitorEventTrackerException()
        {
            Assert.Throws<NullReferenceException>(() => _airspace.Update(new Data("",0,0,0,"")));

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
        public void UpdateOnLeavingDataFormatReceived()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.LEAVING);

            _airspace.Update(data);

            _log.Received(1).WriteNotification(data, true);

            _displayFormatter.ShowTracks(Arg.Any<List<Data>>());
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


        [Test]
        public void UpdateOnConflictingEnteringMonitorReceived()
        {
            var data = new Data("", 0, 0, 0, "");
            var list = new List<Data>();
            list.Add(data);
            list.Add(data);

            _monitor.EventTracker(data).Returns(EventType.CONFLICTING_ENTERING);
            _monitor.GetTracksInConflict().Returns(list);

            _airspace.Update(data);

            _monitor.Received(1).GetTracksInConflict(); //test
        }

        [Test]
        public void UpdateOnConflictingEnteringLogReceived()
        {
            var data = new Data("", 0, 0, 0, "");
            var list = new List<Data>();

            list.Add(data);

            _monitor.EventTracker(data).Returns(EventType.CONFLICTING_ENTERING);
            _monitor.GetTracksInConflict().Returns(list);

            _airspace.Update(data);

            list.Add(data);
            _log.Received(1).WriteWarning(list);
        }

        [Test]
        public void UpdateOnConflictingLeavingMonitorReceived()
        {
            var data = new Data("", 0, 0, 0, "");
            var list = new List<Data>();

            list.Add(data);
            list.Add(data);

            _monitor.EventTracker(data).Returns(EventType.CONFLICTING_LEAVING);
            _monitor.GetTracksInConflict().Returns(list);

            _airspace.Update(data);

            _monitor.Received(1).GetTracksInConflict();
        }

        [Test]
        public void UpdateOnConflictingLeavingDisplayFormatReceived()
        {
            var data = new Data("", 0, 0, 0, "");
            var list = new List<Data>();

            list.Add(data);
            list.Add(data);

            _monitor.EventTracker(data).Returns(EventType.CONFLICTING_LEAVING);
            _monitor.GetTracksInConflict().Returns(list);

            _airspace.Update(data);

            _monitor.Received(1).GetTracksInConflict();
            _displayFormatter.Received(1).ShowTracks(list);
        }

        [Test]
        public void UpdateOnConflictingLeavingLogReceivd()
        {
            var data = new Data("", 0, 0, 0, "");
            var list = new List<Data>();

            list.Add(data);
            list.Add(data);

            _monitor.EventTracker(data).Returns(EventType.CONFLICTING_LEAVING);
            _monitor.GetTracksInConflict().Returns(list);

            _airspace.Update(data);

            _log.Received(1).WriteWarning(list);
        }

        [Test]
        public void UpdateOnOutside()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.OUTSIDE);

            _displayFormatter.DidNotReceiveWithAnyArgs().ShowTracks(null);
            _log.DidNotReceiveWithAnyArgs().WriteNotification(null,false);
        }


        [Test]
        public void UpdateOnEnteringTracks()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.ENTERING);

            _airspace.Update(data);

            Assert.That(_airspace.GetTracks().Count > 0);
        }

        [Test]
        public void UpdateOnLeavingTracks()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.ENTERING);

            _airspace.Update(data);

            _monitor.EventTracker(data).Returns(EventType.LEAVING);

            _airspace.Update(data);

            Assert.That(_airspace.GetTracks().Count == 0);
        }

        [Test]
        public void UpdateNullTrackException()
        {
            var data = new Data("", 0, 0, 0, "");

            _monitor.EventTracker(data).Returns(EventType.ENTERING);

            Assert.Throws<NullReferenceException>(() => _airspace.Update(null));

        }


        [Test]
        public void UpdateOnLeavingTracksNotFound()
        {
            var data1 = new Data("test1", 0, 0, 0, "test1");

            var data2 = new Data("test2", 0, 0, 0, "test2");

            _monitor.EventTracker(data1).Returns(EventType.ENTERING);
            _monitor.EventTracker(data2).Returns(EventType.LEAVING);

            _airspace.Update(data1);
            _airspace.Update(data2);


            Assert.That(_airspace.GetTracks().Count == 1);

        }



    }


}
