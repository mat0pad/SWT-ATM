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
        private Subject<Data> subject;

        [SetUp]
        public void Init()
        {
            subject = new CoordinateMapper(null);
        }

        [Test]
        public void NotifyObserverReceived()
        {
            SWT_ATM.IObserver<Data> observer = Substitute.For<SWT_ATM.IObserver<Data>>();
            Data data = new Data("", 0, 0, 0, "");

            subject.Attach(observer);

            subject.Notify(data);

            observer.Received(1).Update(data);
        }

        [Test]
        public void NotifyObserverNotReceived()
        {
            SWT_ATM.IObserver<Data> observer = Substitute.For<SWT_ATM.IObserver<Data>>();
            Data data = new Data("", 0, 0, 0, "");

            subject.Attach(observer);

            subject.Deattach(observer);

            subject.Notify(data);

            observer.DidNotReceive().Update(data);
        }
    }
}