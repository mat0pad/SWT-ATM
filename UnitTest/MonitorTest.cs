
using NUnit.Framework;
using SWT_ATM;
//using Monitor = SWT_ATM.Monitor;

namespace UnitTest
{
    [TestFixture]
    class MonitorTest
    {
        private Monitor monitor;


        [SetUp]
        public void init()
        {
           monitor = new Monitor();           
        }


        [Test]
        public void XStartTest()
        {
            int[] array = new int[2] {100, 1000};
            monitor.SetX(array[0], array[1]);
            Assert.AreEqual(array,monitor.GetX());
            
        }


    }
}
