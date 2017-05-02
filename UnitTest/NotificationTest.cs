using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;
using SWT_ATM;

namespace UnitTest
{
    [TestFixture]
    class NotificationTest
    {
        private INotificationCenter _notificationCenter;
        private IDisplay _display;

        [SetUp]
        public void Init()
        {

            _display = Substitute.For<IDisplay>();
            _notificationCenter = new NotificationCenter(_display);
        }


        [Test]
        public void NotificationEnqueueNotificationCall()
        {
            _notificationCenter.EnqueNotification(new List<string> {"test1", "test2"});
            _notificationCenter.SetNotificationSignalHandle();
            _display.Received(1).WriteRow(Arg.Is<List<string>>((s => s[0] == "test1" && s[1] == "test2")), 10, Display.InnerRightLineBound, 2);
        }

        /*[Test] // Kan ikke testes da den afhænger af variabler i Display som afhænger af Console klassen -> virker ikke medmindre konsolvinduet er åbnet
        public void WarningEnqueueWarningCall()
        {
            List<string> warning1 = new List<string> {"test1", "test2"};
            List<string> warning2 = new List<string> { "test3", "test4" };

            List<List<string>> warnings = new List<List<string>> { warning1, warning2 };

            _notificationCenter.EnqueWarning(warnings);
            _notificationCenter.SetWarningsSignalHandle();
            //_display.Received(1).WriteRow(Arg.Is<List<string>>((s => s[0] == "test1" && s[1] == "test2" && s[2] == "CONFLICT")), 10, Display.InnerRightLineBound, DisplayFormatter.Height / 2 + 1);
            _display.Received(1).WriteRow(Arg.Any<IEnumerable<string>>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>());
        }*/

        [Test]
        public void Delay()
        {
            bool b = false;

            _notificationCenter.ExecuteDelayed(() => { b = true; }, 5000);
            Thread.Sleep(5050);

            Assert.IsTrue(b);
        }
    }
}
