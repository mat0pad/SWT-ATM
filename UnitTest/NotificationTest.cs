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
    /*public class DisplayFake : IDisplay
    {
        public static int InnerRightLineBound { get { return 0; } }
        public static int OuterRightLineBound { get { return 0; } }

        public void Configure(int width, int height)
        { }

        public void BuildFrame(int width, int height)
        { }

        public void WriteRow(IEnumerable<string> toWrite, int seperation, int startLeft, int startTop)
        { }

        public void ShowTracks(List<IEnumerable<string>> d)
        { }
    }*/


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

            _display.GetInnerRightLineBound().Returns(50);
            _display.GetOuterRightLineBound().Returns(100);
            _formatter.GetHeight().Returns(100);

            _notificationCenter.EnqueWarning(warnings);
            _notificationCenter.SetWarningsSignalHandle();
            //Thread.Sleep(50);
            _display.Received(1).WriteRow(Arg.Is<List<string>>((s => s[0] == "test1" && s[1] == "test2" && s[2] == "CONFLICTING")), 10, _display.GetInnerRightLineBound(), _formatter.GetHeight() / 2 + 1);
        }


        [Test] // Kan ikke testes da den afhænger af variabler i Display som afhænger af Console klassen -> virker ikke medmindre konsolvinduet er åbnet
        public void WarningEnqueueWarningCall2()
        {
            List<string> warning1 = new List<string> { "test1", "test2" };
            List<string> warning2 = new List<string> { "test3", "test4" };

            List<List<string>> warnings = new List<List<string>> { warning1, warning2 };

            _display.GetInnerRightLineBound().Returns(50);
            _display.GetOuterRightLineBound().Returns(100);
            _formatter.GetHeight().Returns(100);

            _notificationCenter.EnqueWarning(warnings);
            _notificationCenter.SetWarningsSignalHandle();
            //Thread.Sleep(50);
            _display.Received(1).WriteRow(Arg.Is<List<string>>((s => s[0] == "test3" && s[1] == "test4" && s[2] == "CONFLICTING")), 10, _display.GetInnerRightLineBound(), _formatter.GetHeight() / 2 + 2);  
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
