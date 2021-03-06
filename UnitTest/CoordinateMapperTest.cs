﻿using System;
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
    class CoordinateMapperTest
    {
        private CoordinateMapper _mapper;
        private ITransponderDataFormat _format;

        [SetUp]
        public void Init()
        {
            _format = Substitute.For<ITransponderDataFormat>();
            _mapper = new CoordinateMapper(_format);
        }

        [Test]
        public void MapTrack()
        {
            List<string> list = new List<string>();

            list.Add("test");
            _mapper.MapTrack(list);

            _format.Received(1).FormatData("test");
        }

    }
}
