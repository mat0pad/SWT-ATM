﻿
using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SWT_ATM;
using System.Diagnostics;
//using Monitor = SWT_ATM.Monitor;

namespace UnitTest
{
    [TestFixture]
    class MonitorTest
    {
        private IMonitor _monitor;


        [SetUp]
        public void Init()
        {
           _monitor = new Monitor();           
        }


        [TestCase(0)]
        [TestCase(1)]
        public void SetGetXTest(int i)
        {
            int[] array = new int[2] {100, 1000};
            _monitor.SetX(array[0], array[1]);
            Assert.That(array[i].Equals(_monitor.GetX()[i]));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void SetGetYTest(int i)
        {
            int[] array = new int[2] { 100, 1000 };
            _monitor.SetY(array[0], array[1]);
            Assert.That(array[i].Equals(_monitor.GetY()[i]));
        }

        [TestCase(0)]
        [TestCase(1)]
        public void SetGetZTest(int i)
        {
            int[] array = new int[2] { 100, 1000 };
            _monitor.SetZ(array[0], array[1]);
            Assert.That(array[i].Equals(_monitor.GetZ()[i]));
        }

       /* [Test]
        public void EventTrackException()
        {
            Assert.Throws<NullReferenceException>(() => _monitor.EventTracker(null));
        }*/

        [Test]
        public void GetAllConflicts()
        {
            Data data1 = new Data("1", 0, 0, 500, "");
            Data data2 = new Data("2", 4999, 4999, 799, "");
            Data data3 = new Data("3", 5000, 5000, 800, "");

            var list = new List<Data> { data1, data2, data3 };

            _monitor.SetShareList(ref list);

            var testConflicts =_monitor.GetAllConflicts();

            List<Data> conflict1 = testConflicts[0];
            List<Data> conflict2 = testConflicts[1];

            Debug.WriteLine("GetAllConflicts TEST: " + conflict1.Count); 
            Debug.WriteLine("GetAllConflicts TEST: " + conflict1[0].Tag + " " + conflict1[1].Tag);
            Debug.WriteLine("GetAllConflicts TEST: " + conflict2[0].Tag + " " + conflict2[1].Tag);

            Assert.IsTrue(conflict1[0].Tag == "3");
            Assert.IsTrue(conflict1[1].Tag == "2");
            Assert.IsTrue(conflict2[0].Tag == "2");
            Assert.IsTrue(conflict2[1].Tag == "1");
        }


        [Test]
        public void EventTrackEntering()
        {
            setBoundary(100, 1000);
            Data data = new Data("Data1", 200, 500, 500, "data");

            List<Data> list = new List<Data>();
            
            _monitor.SetShareList(ref list);

            Assert.That(_monitor.EventTracker(data).Equals(EventType.ENTERING));
        }

        [Test]
        public void EventTrackLeaving()
        {
            setBoundary(100, 1000);
            Data data = new Data("Data1", 50, 500, 500, "data");

            List<Data> list = new List<Data>();
            list.Add(data);
            _monitor.SetShareList(ref list);

            Assert.That(_monitor.EventTracker(data).Equals(EventType.LEAVING));
        }

        [Test]
        public void EventTrackInside()
        {
            setBoundary(100, 1000);
            Data data = new Data("Data1", 200, 500, 500, "data");

            List<Data> list = new List<Data>();
            list.Add(data);
            _monitor.SetShareList(ref list);

            Assert.That(_monitor.EventTracker(data).Equals(EventType.INSIDE));
        }

        [Test]
        public void EventTrackOutside()
        {
            setBoundary(100, 1000);
            Data data = new Data("Data1", 0, 0, 0, "data");

            List<Data> list = new List<Data>();
            _monitor.SetShareList(ref list);

            Assert.That(_monitor.EventTracker(data).Equals(EventType.OUTSIDE));
        }

        [Test]
        public void EventTrackConflicting()
        {
            setBoundary(100, 2000);
            Data data1 = new Data("Data1", 400, 400, 400, "data1");
            Data data2 = new Data("Data2", 400, 400, 400, "data2");

            List<Data> list = new List<Data>();
          
            list.Add(data1);
            list.Add(data2);

            _monitor.SetShareList(ref list);

            Assert.That(_monitor.EventTracker(data2).Equals(EventType.CONFLICTING));
        }


        [Test]
        public void EventTrackConflictingEntering()
        {
            setBoundary(100, 2000);
            Data data1 = new Data("Data1", 400, 400, 400, "data1");
            Data data2 = new Data("Data2", 400, 400, 400, "data2");

            List<Data> list = new List<Data>();

            list.Add(data1);

            _monitor.SetShareList(ref list);

            Assert.That(_monitor.EventTracker(data2).Equals(EventType.CONFLICTING_ENTERING));
        }

        [Test]
        public void EventTrackConflictingLeaving()
        {
            setBoundary(100, 2000);
            Data data1 = new Data("Data1", 200, 400, 400, "data1");
            Data data2 = new Data("Data2", 50, 400, 400, "data2");

            List<Data> list = new List<Data>();

            list.Add(data1);
            list.Add(data2);

            _monitor.SetShareList(ref list);

            Assert.That(_monitor.EventTracker(data2).Equals(EventType.CONFLICTING_LEAVING));
        }




        [Test]
        public void GetTracksInConflictSizeZero()
        {
            Assert.AreEqual(0,_monitor.GetTracksInConflict().Count);
        }



        [Test]
        public void GetTracksInConflictAddConflict()
        {
            List<Data> list = new List<Data>();
            Data data1 = new Data("Data1", 200, 200, 200, "Data1");
            Data data2 = new Data("Data2", 200, 200, 200, "Data2");

            setBoundary(100, 1000);

            list.Add(data1);
            list.Add(data2); 

            _monitor.SetShareList(ref list);
            _monitor.EventTracker(data2); //adds conflicting data
            
            Console.WriteLine(_monitor.GetTracksInConflict().Count);
            Assert.AreEqual(1,_monitor.GetTracksInConflict().Count);
        }

        private void setBoundary(int start, int end)
        {
            _monitor.SetX(start, end);
            _monitor.SetY(start, end);
            _monitor.SetZ(start, end);
        }

    }
}
