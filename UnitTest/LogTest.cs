using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SWT_ATM;

namespace UnitTest
{
    [TestFixture]
    class LogTest
    {

        private Log _log;
        private string path;

        [SetUp]
        public void Init()
        {
            path = Directory.GetCurrentDirectory() + @"\test.txt";
            _log = new Log(path);            
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WriteNotificationTest(bool isleaving)
        {
            Data data = new Data("test1", 100, 200, 300, "1203");
            string line;

            _log.WriteNotification(data, isleaving);

            using (var file = new StreamReader(path, true))
            {
                line = file.ReadToEnd();
            }

            if(isleaving)
                Assert.That(line.Contains("LEAVING"));
            else
                Assert.That(line.Contains("ENTERING"));
        }


        [Test]
        public void ExceptionCallConstutNoParameter()
        {
            var file = new StreamReader(path);
            Assert.Throws<System.IO.IOException>(() => new Log(path));
            file.Close();

        }

        [Test]
        public void ExceptionCallConstutWithParameter()
        {
            var file = new StreamReader(Directory.GetCurrentDirectory() + @"\log.txt");
            Assert.Throws<System.IO.IOException>(() => new Log(Directory.GetCurrentDirectory() + @"\log.txt"));
            file.Close();

        }

        [Test]
        public void WriteWarningTest()
        {
            string line;
            Data data = new Data("test1", 100, 200, 300, "1203");
            List<Data> list = new List<Data>();

            list.Add(data);

            _log.WriteWarning(list);

            using (var file = new StreamReader(path, true))
            {
                line = file.ReadToEnd();
            }
            Assert.That(line.Contains("1203"));
        }

    }
}
