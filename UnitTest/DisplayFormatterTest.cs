using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SWT_ATM;

namespace UnitTest
{
    [TestFixture]
    class DisplayFormatterTest
    {
        private IDisplayFormatter _displayFormatter;
        private INotificationCenter _notificationCenter;
        private IPositionCalc _calc;
        private IDisplay _display;

        private int _testWidth = 0;
        private int _testHeight = 0;


        [SetUp]
        public void Init()
        {
            _notificationCenter = Substitute.For<INotificationCenter>();
            _calc = Substitute.For<IPositionCalc>();
            _display = Substitute.For<IDisplay>();
            _displayFormatter = new DisplayFormatter(_display, _calc, _notificationCenter, _testWidth, _testHeight);
        }

        [Test]
        public void GetHeight()
        {
            Assert.IsTrue(_displayFormatter.GetHeight() == _testHeight);
        }


        [Test]
        public void NotificationShowNotificationCall()
        {
            Data data = new Data("test", 0, 0, 0, "");

            _displayFormatter.ShowNotification(data, EventType.ENTERING);

            var item = new List<string> { data.Tag, EventType.ENTERING.ToString() };

            _notificationCenter.Received(1).EnqueNotification(Arg.Is<List<string>>(s =>
            s[0] == "test" && s[1] == EventType.ENTERING.ToString()));

            _notificationCenter.Received(1).SetNotificationSignalHandle();
        }


        [Test]
        public void NotificationShowNotification()
        {
            _displayFormatter.ShowNotification(null, null);

            _notificationCenter.DidNotReceive().SetNotificationSignalHandle();
        }

        [Test]
        public void WarningShowWarningCall()
        {
            List<Data> data1 = new List<Data> { new Data("test1", 0, 0, 0, ""), new Data("test2", 0, 0, 0, "") };
            List<Data> data2 = new List<Data> { new Data("test3", 0, 0, 0, ""), new Data("test4", 0, 0, 0, "") };

            List<List<Data>> warnings = new List<List<Data>> {data1, data2};

            _displayFormatter.ShowWarning(warnings);

            _notificationCenter.Received(1).EnqueWarning(Arg.Is<List<List<string>>>(s =>
            s[0][0] == "test1" && s[0][1] == "test2" && s[1][0] == "test3" && s[1][1] == "test4"));

            _notificationCenter.Received(1).SetWarningsSignalHandle();
        }

        [Test]
        public void SetSizeCall()
        {
            _displayFormatter.SetSize(_testWidth, _testHeight);
            _display.Received(2).BuildFrame(_testWidth, _testHeight);
        }

        [Test]
        public void SetWidthCall()
        {
            _displayFormatter.SetWidth(_testWidth);
            _display.Received(2).BuildFrame(_testWidth, _testHeight);
        }

        [Test]
        public void SetHeightCall()
        {
            _displayFormatter.SetHeight(_testHeight);
            _display.Received(2).BuildFrame(_testWidth, _testHeight);
        }

        [Test]
        public void showTracksNullArgs()
        {
            Assert.Throws<NullReferenceException>(() => _displayFormatter.ShowTracks(null));
        }


        [Test]
        public void ShowTracksCastCalc()
        {
            Data data1 = new Data("test1", 0, 0, 0, "");
            Data data2 = new Data("test2", 0, 0, 0, "");

            List<Data> list = new List<Data> { data1, data2 };

            _displayFormatter.ShowTracks(list);

            _displayFormatter.ShowTracks(list);

            _calc.Received(4).FormatTrackData(Arg.Any<Data>(),Arg.Any<Data>());
        }

        [Test]
        public void ShowTracksC()
        {
            Data data1 = new Data("test1", 0, 0, 0, "");
            Data data2 = new Data("test2", 0, 0, 0, "");

            List<Data> list = new List<Data> { data1, data2 };

            _displayFormatter.ShowTracks(list);

            Data data4 = new Data("test1", 1, 20, 20, "");
            Data data3 = new Data("test2", 10, 20, 20, "");

            List<Data> list1 = new List<Data> { data3, data4 };

            _displayFormatter.ShowTracks(list);

            _calc.Received(4).FormatTrackData(Arg.Any<Data>(), Arg.Any<Data>());
        }

        [Test]
        public void ShowTracksCall()
        {
            Data data1 = new Data("test1", 0, 0, 0, "");
            Data data2 = new Data("test2", 0, 0, 0, "");

            List<Data> list = new List<Data> {data1, data2};

            _displayFormatter.ShowTracks(list);

            _display.Received(1).ShowTracks(Arg.Any<List<IEnumerable<string>>>());
        }

        [Test]
        public void ShowTracksCallWithPrev()
        {
            Data data1 = new Data("test1", 0, 0, 0, "");
            Data data2 = new Data("test2", 0, 0, 0, "");

            List<Data> list = new List<Data> { data1, data2 };

            _displayFormatter.ShowTracks(list);
            _displayFormatter.ShowTracks(list);

            _display.Received(2).ShowTracks(Arg.Any<List<IEnumerable<string>>>());
        }
    }
}
