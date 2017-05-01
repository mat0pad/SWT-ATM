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
    class DisplayTest
    {
        private Display _display;
        private INotificationCenter notificationCenter;

        [SetUp]
        public void Init()
        {
            notificationCenter = Substitute.For<INotificationCenter>();
            _display = new Display();
        }

        [Test]
        public void ShowNotification()
        {
            _display.ShowNotification(new Data("test", 0, 0, 0, "0"), EventType.LEAVING);

            notificationCenter.Received(1).GetNotificationQueue().Enqueue(null);
        }



    }
}
