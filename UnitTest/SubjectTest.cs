using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SWT_ATM;

namespace UnitTest
{
    [TestFixture]
    class SubjectTest
    {
        private Subject<List<Data>> subject;

        [SetUp]
        public void Init()
        {
            subject = new CoordinateMapper(null);
        }

        [Test]
        public void NotifyObserverReceived()
        {
            SWT_ATM.IObserver<List<Data>> observer = Substitute.For<SWT_ATM.IObserver<List<Data>>>();
            Data data = new Data("", 0, 0, 0, "");

            subject.Attach(observer);

            var list = new List<Data> {data};

            subject.Notify(list);

            observer.Received(1).Update(list);
        }

        [Test]
        public void NotifyObserverNotReceived()
        {
            SWT_ATM.IObserver<List<Data>> observer = Substitute.For<SWT_ATM.IObserver<List<Data>>>();
            Data data = new Data("", 0, 0, 0, "");

            subject.Attach(observer);

            subject.Deattach(observer);

            subject.Notify(new List<Data> { data });

            observer.DidNotReceive().Update(new List<Data> { data });
        }
    }
}