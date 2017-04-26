using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SWT_ATM;

namespace UnitTest
{
    [TestFixture]
    class TransponderDataFormatTest
    {

        private TransponderDataFormat _tdf;

        [SetUp]
        public void Init()
        {
            _tdf = new TransponderDataFormat();
        }

        [Test]
        public void FormatDataTestSuccesfull()
        {
            string rawdata = "test;10;10;10;10";
            Data expecteddata = new Data("test", 10, 10, 10,"10");

            Data actualdata =  _tdf.FormatData(rawdata);
  
            Assert.AreEqual(expecteddata.Altitude, actualdata.Altitude);
            Assert.AreEqual(expecteddata.Tag, actualdata.Tag);
            Assert.AreEqual(expecteddata.Timestamp, actualdata.Timestamp);
            Assert.AreEqual(expecteddata.XCord, actualdata.XCord);
            Assert.AreEqual(expecteddata.YCord, actualdata.YCord);
        }


    }
}

