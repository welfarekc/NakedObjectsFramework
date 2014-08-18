// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System;
using System.Globalization;
using NakedObjects.Architecture.Facets;
using NUnit.Framework;

namespace NakedObjects.Reflector.DotNet.Value {
    [TestFixture]
    public class DateTimeValueSemanticsProviderTest : ValueSemanticsProviderAbstractTestCase<DateTime> {
        [SetUp]
        public override void SetUp() {
            base.SetUp();
            SetupSpecification(typeof (DateTime));
            holder = new FacetHolderImpl();
            SetValue(adapter = new DateTimeValueSemanticsProvider(reflector, holder));
        }

        private DateTimeValueSemanticsProvider adapter;
        private IFacetHolder holder;

        private void AssertEntry(string entry, int year, int month, int day, int hour, int minute, int second) {
            object obj = adapter.ParseTextEntry(entry);
            Assert.AreEqual(new DateTime(year, month, day, hour, minute, second), obj);
        }

        [Test]
        public void TestDecode() {
            DateTime decoded = adapter.FromEncodedString("2003-08-17T21:30:25");
            Assert.AreEqual(new DateTime(TestClock.GetTicks()), decoded);
        }

        [Test]
        public void TestEmptyClears() {
            Assert.IsNull(adapter.ParseTextEntry(""));
        }

        [Test]
        public void TestEncode() {
            string encoded = adapter.ToEncodedString(new DateTime(TestClock.GetTicks()));
            Assert.AreEqual("2003-08-17T21:30:25", encoded);
        }

        [Test]
        public void TestParseInvariant() {
            var d1 = new DateTime(2014, 7, 10, 14, 52, 0, DateTimeKind.Utc); 
            var s1 = d1.ToString(CultureInfo.InvariantCulture); 
            var d2 = adapter.ParseInvariant(s1);
            Assert.AreEqual(d1, d2);
        }


        [Test]
        public void TestEntryWithLongISOFormat() {
            var dt = new DateTime(2007, 5, 21, 10, 30, 0);
            dt = dt.ToUniversalTime();
            string entry = dt.ToString("u");
            AssertEntry(entry, 2007, 5, 21, 10, 30, 0);
        }

        [Test]
        public void TestEntryWithMediumFormat() {
            var dt = new DateTime(2007, 5, 21, 10, 30, 0);
            string entry = dt.ToString("f");
            // "21-May-2007 10:30"
            AssertEntry(entry, 2007, 5, 21, 10, 30, 0);
        }

        [Test]
        public void TestEntryWithShortFormat() {
            var dt = new DateTime(2007, 5, 21, 10, 30, 0);
            string entry = dt.ToString("g");
            const int year = 2007;
            const int month = 5;
            const int day = 21;
            const int hour = 10;
            const int minute = 30;
            AssertEntry(entry, year, month, day, hour, minute, 0);
        }

        [Test]
        public void TestEntryWithShortISOFormat() {
            // not currently recognised
            //assertEntry("20070521T1030", 2007, 5, 21, 10, 30, 0);
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}