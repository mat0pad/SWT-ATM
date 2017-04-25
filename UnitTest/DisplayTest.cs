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
    class DisplayTest
    {
        private Display _display;

        [SetUp]
        public void Init()
        {
            _display = new Display();
        }

    }
}
