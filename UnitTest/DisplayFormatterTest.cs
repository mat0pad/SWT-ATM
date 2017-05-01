using System;
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


        [SetUp]
        public void Init()
        {
            _notificationCenter = Substitute.For<INotificationCenter>();
            _calc = Substitute.For<IPositionCalc>();
            _display = Substitute.For<IDisplay>();
            _displayFormatter = new DisplayFormatter(_display, _calc, _notificationCenter);
        }


        [Test]
        public void NotificationShowNotificationCall()
        {

            Data data = new Data("test", 0, 0, 0, "");

            _displayFormatter.ShowNotification(data, EventType.CONFLICTING);

            var item = new List<string> { data.Tag, EventType.CONFLICTING.ToString() };

            _notificationCenter.Received(1).EnqueNotification(Arg.Any<List<string>>());//Arg.Is<List<string>>(s =>
            //s[0] == "test" && s[1] == EventType.CONFLICTING.ToString()));

            _notificationCenter.Received(1).SetNotificationSignalHandle();
        }
    }
}
