using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.Extensions;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;
using SWT_ATM;

namespace UnitTest
{
    [TestFixture]
    class NotificationTest
    {
        private INotificationCenter _notificationCenter;
        private IDisplayFormatter _formatter;
        private IDisplay _display;

        [SetUp]
        public void Init()
        {

            _display = Substitute.For<IDisplay>();
            _formatter = Substitute.For<IDisplayFormatter>();
            _notificationCenter = new NotificationCenter(_display);
            _notificationCenter.SetFormatter(_formatter);
        }


        [Test]
        public void NotificationEnqueueNotificationCall()
        {
            _notificationCenter.EnqueNotification(new List<string> {"test1", "test2"});
            _notificationCenter.SetNotificationSignalHandle();
            Thread.Sleep(50);
            _display.Received(1).WriteRow(Arg.Is<List<string>>((s => s[0] == "test1" && s[1] == "test2")), 10, _display.GetInnerRightLineBound(), 2);
        }

        [Test] // Kan ikke testes da den afhænger af variabler i Display som afhænger af Console klassen -> virker ikke medmindre konsolvinduet er åbnet
        public void WarningEnqueueWarningCall()
        {
            List<string> warning1 = new List<string> {"test1", "test2"};
            List<string> warning2 = new List<string> { "test3", "test4" };

            List<List<string>> warnings = new List<List<string>> { warning1, warning2 };

            int returnV = 50;

            _display.GetInnerRightLineBound().Returns(returnV);
            _display.GetOuterRightLineBound().Returns(100);
            _formatter.GetHeight().Returns(100);

            _notificationCenter.EnqueWarning(warnings);
            _notificationCenter.SetWarningsSignalHandle();
            Thread.Sleep(500);
            _display.Received(1).WriteRow(Arg.Is<List<string>>((s => s[0] == "test1" && s[1] == "test2" && s[2] == "CONFLICTING")), 10, returnV, _formatter.GetHeight() / 2 + 1);
        }


        [Test] // Kan ikke testes da den afhænger af variabler i Display som afhænger af Console klassen -> virker ikke medmindre konsolvinduet er åbnet
        public void WarningEnqueueWarningCall2()
        {
            List<string> warning1 = new List<string> { "test1", "test2" };
            List<string> warning2 = new List<string> { "test3", "test4" };

            List<List<string>> warnings = new List<List<string>> { warning1, warning2 };

            int returnV = 50;

            _display.GetInnerRightLineBound().Returns(returnV);
            _display.GetOuterRightLineBound().Returns(100);
            _formatter.GetHeight().Returns(100);

            _notificationCenter.EnqueWarning(warnings);
            _notificationCenter.SetWarningsSignalHandle();
            Thread.Sleep(500);
            _display.Received(1).WriteRow(Arg.Is<List<string>>((s => s[0] == "test3" && s[1] == "test4" && s[2] == "CONFLICTING")), 10, returnV, _formatter.GetHeight() / 2 + 2);  
        }


        [Test]
        public void EnqueueWarningNoThrowTest()
        {
            List<List<string>> list = new List<List<string>>();
           Assert.DoesNotThrow(() =>  _notificationCenter.EnqueWarning(list)); 
        }

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
