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
    class PositonCalcTest
    {
        private IPositionCalc _positionCalc;

        [SetUp]
        public void Init()
        {
            _positionCalc = new PositionCalc();
        }


        [Test]
        public void VelocityCalculation()
        {
            int sec = 0;

            // Kan bruges til at opdatere position på nedenstående data
            Data data1 = new Data("timeTest1", 0, 0, 0, "201708302054" + $"{sec:00}" + "166");
            Data data2 = new Data("timeTest1", 1, 0, 0, "201708302054" + $"{sec + 1:00}" + "166");


            Assert.IsTrue("1" == _positionCalc.CalcVelocity(data2, data1));
        }

        [Test]
        public void CompassCourseCalculation()
        {
            int sec = 0;

            // Kan bruges til at opdatere position på nedenstående data
            Data data1 = new Data("timeTest1", 0, 0, 0, "201708302054" + $"{sec:00}" + "166");
            Data data2 = new Data("timeTest1", 1, 0, 0, "201708302054" + $"{sec + 1:00}" + "166");


            Assert.IsTrue("90" == _positionCalc.CalcCourse(data2, data1));
        }

        [Test]
        public void FormattedData()
        {
            int sec = 0;

            // Kan bruges til at opdatere position på nedenstående data
            Data data1 = new Data("timeTest1", 0, 0, 0, "201708302054" + $"{sec:00}" + "166");
            Data data2 = new Data("timeTest1", 1, 0, 0, "201708302054" + $"{sec + 1:00}" + "166");

            var formatted = _positionCalc.FormatTrackData(data2, data1).ToList();

            

            Assert.IsTrue(formatted[0] == "timeTest1");
            Assert.IsTrue(formatted[1] == "1");
            Assert.IsTrue(formatted[2] == "0");
            Assert.IsTrue(formatted[3] == "0");
            Assert.IsTrue(formatted[4] == "1");
            Assert.IsTrue(formatted[5] == "90");
        }
    }
}
