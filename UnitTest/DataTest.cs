using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SWT_ATM;

namespace UnitTest
{
    [TestFixture]
    class DataTest
    {

        private Data data;
        [SetUp]
        public void Init()
        {
            data = new Data("", 0, 0, 0, "");
        }

        [Test]
        public void velocityTest()
        {
            data.Velocity = 2.00;
            Assert.That(data.Velocity == 2.00);
        }

        [Test]
        public void CompassCourseTest()
        {
            data.CompassCourse = 90;
            Assert.That(data.CompassCourse == 90);
        }






    }
}
