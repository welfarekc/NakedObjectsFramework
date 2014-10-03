// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System;
using NakedObjects.Architecture.Facets;
using NakedObjects.Architecture.Facets.Objects.Defaults;
using NakedObjects.Architecture.Facets.Objects.Encodeable;
using NakedObjects.Architecture.Facets.Objects.EqualByContent;
using NakedObjects.Architecture.Facets.Objects.Ident.Title;
using NakedObjects.Architecture.Facets.Objects.Immutable;
using NakedObjects.Architecture.Facets.Objects.Parseable;
using NakedObjects.Architecture.Facets.Objects.TypicalLength;
using NakedObjects.Architecture.Facets.Objects.Value;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Capabilities;
using NUnit.Framework;

namespace NakedObjects.Reflector.DotNet.Facets.Objects.Value {
    [TestFixture]
    public class ValueFacetFactoryTest : AbstractFacetFactoryTest {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp() {
            base.SetUp();
            facetFactory = new ValueFacetFactory(Reflector);
        }

        [TearDown]
        public override void TearDown() {
            facetFactory = null;
            base.TearDown();
        }

        #endregion

        private ValueFacetFactory facetFactory;


        protected override Type[] SupportedTypes {
            get { return new[] {typeof (IValueFacet)}; }
        }

        protected override IFacetFactory FacetFactory {
            get { return facetFactory; }
        }

        [Value]
        private class MyNumberEqualByContentDefault {}

        [Value]
        private class MyNumberImmutableDefault {}

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyParseableUsingParserName2")]
        public class MyParseableUsingParserName2 : ValueSemanticsProviderImpl<MyParseableUsingParserName2> {
            // Required since is a IParser.
        }

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyValueSemanticsProviderThatIsADefaultsProvider")]
        public class MyValueSemanticsProviderThatIsADefaultsProvider : ValueSemanticsProviderImpl<MyValueSemanticsProviderThatIsADefaultsProvider>, IDefaultsProvider<MyValueSemanticsProviderThatIsADefaultsProvider> {
            // Required since is a ValueSemanticsProvider.

            #region IDefaultsProvider<MyValueSemanticsProviderThatIsADefaultsProvider> Members

            public MyValueSemanticsProviderThatIsADefaultsProvider DefaultValue {
                get { return new MyValueSemanticsProviderThatIsADefaultsProvider(); }
            }

            #endregion
        }

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyValueSemanticsProviderThatIsAnEncoderDecoder")]
        public class MyValueSemanticsProviderThatIsAnEncoderDecoder : ValueSemanticsProviderImpl<MyValueSemanticsProviderThatIsAnEncoderDecoder>, IEncoderDecoder<MyValueSemanticsProviderThatIsAnEncoderDecoder> {
            // Required since is a ValueSemanticsProvider.

            #region IEncoderDecoder<MyValueSemanticsProviderThatIsAnEncoderDecoder> Members

            public MyValueSemanticsProviderThatIsAnEncoderDecoder FromEncodedString(string encodedString) {
                return null;
            }

            public string ToEncodedString(MyValueSemanticsProviderThatIsAnEncoderDecoder toEncode) {
                return null;
            }

            #endregion
        }

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyValueSemanticsProviderThatIsAParser")]
        public class MyValueSemanticsProviderThatIsAParser : ValueSemanticsProviderImpl<MyValueSemanticsProviderThatIsAParser>, IParser<MyValueSemanticsProviderThatIsAParser> {
            // Required since is a ValueSemanticsProvider.

            #region IParser<MyValueSemanticsProviderThatIsAParser> Members

            public object ParseTextEntry(string entry) {
                return null;
            }

            public object ParseInvariant(string entry) {
                return null;
            }

            public string InvariantString(MyValueSemanticsProviderThatIsAParser obj) {
                throw new NotImplementedException();
            }

            public string DisplayTitleOf(MyValueSemanticsProviderThatIsAParser obj) {
                return null;
            }

            public string EditableTitleOf(MyValueSemanticsProviderThatIsAParser existing) {
                return null;
            }

            public string TitleWithMaskOf(string mask, MyValueSemanticsProviderThatIsAParser obj) {
                return null;
            }

            public int TypicalLength {
                get { return 0; }
            }

            #endregion
        }

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyValueSemanticsProviderThatSpecifiesEqualByContentSemantic")]
        public class MyValueSemanticsProviderThatSpecifiesEqualByContentSemantic : ValueSemanticsProviderImpl<MyValueSemanticsProviderThatSpecifiesEqualByContentSemantic> {
            // Required since is a ValueSemanticsProvider.


            public MyValueSemanticsProviderThatSpecifiesEqualByContentSemantic()
                : base(true, true) {}
        }

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyValueSemanticsProviderThatSpecifiesImmutableSemantic")]
        public class MyValueSemanticsProviderThatSpecifiesImmutableSemantic : ValueSemanticsProviderImpl<MyValueSemanticsProviderThatSpecifiesImmutableSemantic> {
            // Required since is a ValueSemanticsProvider.


            public MyValueSemanticsProviderThatSpecifiesImmutableSemantic()
                : base(true, true) {}
        }

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyValueSemanticsProviderThatSpecifiesNotEqualByContentSemantic")]
        public class MyValueSemanticsProviderThatSpecifiesNotEqualByContentSemantic : ValueSemanticsProviderImpl<MyValueSemanticsProviderThatSpecifiesNotEqualByContentSemantic> {
            // Required since is a ValueSemanticsProvider.


            public MyValueSemanticsProviderThatSpecifiesNotEqualByContentSemantic()
                : base(false, false) {}
        }

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyValueSemanticsProviderThatSpecifiesNotImmutableSemantic")]
        public class MyValueSemanticsProviderThatSpecifiesNotImmutableSemantic : ValueSemanticsProviderImpl<MyValueSemanticsProviderThatSpecifiesNotImmutableSemantic> {
            // Required since is a ValueSemanticsProvider.


            public MyValueSemanticsProviderThatSpecifiesNotImmutableSemantic()
                : base(false, true) {}
        }

        [Value(SemanticsProviderClass = typeof (MyValueSemanticsProviderUsingSemanticsProviderClass))]
        public class MyValueSemanticsProviderUsingSemanticsProviderClass : ValueSemanticsProviderImpl<MyValueSemanticsProviderUsingSemanticsProviderClass> {
            // Required since is a ValueSemanticsProvider.
        }

        [Value(SemanticsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Value.ValueFacetFactoryTest+MyValueSemanticsProviderUsingSemanticsProviderName")]
        public class MyValueSemanticsProviderUsingSemanticsProviderName : ValueSemanticsProviderImpl<MyValueSemanticsProviderUsingSemanticsProviderName> {
            // Required since is a ValueSemanticsProvider.
        }

        [Value(SemanticsProviderClass = typeof (MyValueSemanticsProviderWithoutNoArgConstructor))]
        public class MyValueSemanticsProviderWithoutNoArgConstructor : ValueSemanticsProviderImpl<MyValueSemanticsProviderWithoutNoArgConstructor> {
            // no no-arg constructor

            // pass in false for an immutable, which isn't the default
            public MyValueSemanticsProviderWithoutNoArgConstructor(int value)
                : base(false, false) {}
        }

        [Value(SemanticsProviderClass = typeof (MyValueSemanticsProviderWithoutPublicNoArgConstructor))]
        public class MyValueSemanticsProviderWithoutPublicNoArgConstructor : ValueSemanticsProviderImpl<MyValueSemanticsProviderWithoutPublicNoArgConstructor> {
            // no public no-arg constructor

            // pass in false for an immutable, which isn't the default
            private MyValueSemanticsProviderWithoutPublicNoArgConstructor()
                : base(false, false) {}

            public MyValueSemanticsProviderWithoutPublicNoArgConstructor(int value) {}
        }

        [Value]
        public class MyValueWithSemanticsProviderSpecifiedUsingConfiguration : ValueSemanticsProviderImpl<MyValueWithSemanticsProviderSpecifiedUsingConfiguration>, IParser<MyValueWithSemanticsProviderSpecifiedUsingConfiguration> {
            // Required since is a SemanticsProvider.

            #region IParser<MyValueWithSemanticsProviderSpecifiedUsingConfiguration> Members

            public object ParseTextEntry(string entry) {
                return null;
            }

            public object ParseInvariant(string entry) {
                return null;
            }

            public string InvariantString(MyValueWithSemanticsProviderSpecifiedUsingConfiguration obj) {
                throw new NotImplementedException();
            }

            public string DisplayTitleOf(MyValueWithSemanticsProviderSpecifiedUsingConfiguration obj) {
                return null;
            }

            public string TitleWithMaskOf(string mask, MyValueWithSemanticsProviderSpecifiedUsingConfiguration obj) {
                return null;
            }

            public string EditableTitleOf(MyValueWithSemanticsProviderSpecifiedUsingConfiguration existing) {
                return null;
            }

            public int TypicalLength {
                get { return 0; }
            }

            #endregion
        }

        public class NonAnnotatedValueSemanticsProviderSpecifiedUsingConfiguration : ValueSemanticsProviderImpl<NonAnnotatedValueSemanticsProviderSpecifiedUsingConfiguration>, IParser<NonAnnotatedValueSemanticsProviderSpecifiedUsingConfiguration> {
            // Required since is a SemanticsProvider.

            #region IParser<NonAnnotatedValueSemanticsProviderSpecifiedUsingConfiguration> Members

            public object ParseTextEntry(string entry) {
                return null;
            }

            public object ParseInvariant(string entry) {
                return null;
            }

            public string InvariantString(NonAnnotatedValueSemanticsProviderSpecifiedUsingConfiguration obj) {
                throw new NotImplementedException();
            }

            public string DisplayTitleOf(NonAnnotatedValueSemanticsProviderSpecifiedUsingConfiguration obj) {
                return null;
            }

            public string TitleWithMaskOf(string mask, NonAnnotatedValueSemanticsProviderSpecifiedUsingConfiguration obj) {
                return null;
            }

            public string EditableTitleOf(NonAnnotatedValueSemanticsProviderSpecifiedUsingConfiguration existing) {
                return null;
            }

            public int TypicalLength {
                get { return 0; }
            }

            #endregion
        }

        [Test]
        public void TestEqualByContentFacetsIsInstalledIfNoSemanticsProviderSpecified() {
            facetFactory.Process(typeof (MyNumberEqualByContentDefault), MethodRemover, FacetHolder);

            var facet = (IEqualByContentFacet) FacetHolder.GetFacet(typeof (IEqualByContentFacet));
            Assert.IsNotNull(facet);
        }


        [Test]
        public void TestEqualByContentFacetsIsInstalledIfSpecifiesEqualByContent() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatSpecifiesEqualByContentSemantic), MethodRemover, FacetHolder);

            var facet = (IEqualByContentFacet) FacetHolder.GetFacet(typeof (IEqualByContentFacet));
            Assert.IsNotNull(facet);
        }


        [Test]
        public void TestEqualByContentFacetsIsNotInstalledIfSpecifiesNotEqualByContent() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatSpecifiesNotEqualByContentSemantic), MethodRemover, FacetHolder);
            var facet = (IEqualByContentFacet) FacetHolder.GetFacet(typeof (IEqualByContentFacet));
            Assert.IsNull(facet);
        }

        [Test]
        public void TestFacetFacetHolderStored() {
            facetFactory.Process(typeof (MyParseableUsingParserName2), MethodRemover, FacetHolder);

            var valueFacet = (ValueFacetAnnotation<MyParseableUsingParserName2>) FacetHolder.GetFacet(typeof (IValueFacet));
            Assert.AreEqual(FacetHolder, valueFacet.FacetHolder);
        }

        [Test]
        public void TestFacetPickedUp() {
            facetFactory.Process(typeof (MyParseableUsingParserName2), MethodRemover, FacetHolder);

            var facet = (IValueFacet) FacetHolder.GetFacet(typeof (IValueFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is ValueFacetAnnotation<MyParseableUsingParserName2>);
        }

        [Test]
        public override void TestFeatureTypes() {
            NakedObjectFeatureType[] featureTypes = facetFactory.FeatureTypes;
            Assert.IsTrue(Contains(featureTypes, NakedObjectFeatureType.Objects));
            Assert.IsFalse(Contains(featureTypes, NakedObjectFeatureType.Property));
            Assert.IsFalse(Contains(featureTypes, NakedObjectFeatureType.Collection));
            Assert.IsFalse(Contains(featureTypes, NakedObjectFeatureType.Action));
            Assert.IsFalse(Contains(featureTypes, NakedObjectFeatureType.ActionParameter));
        }

        [Test]
        public void TestImmutableFacetsIsInstalledIfNoSemanticsProviderSpecified() {
            facetFactory.Process(typeof (MyNumberImmutableDefault), MethodRemover, FacetHolder);

            var facet = (IImmutableFacet) FacetHolder.GetFacet(typeof (IImmutableFacet));
            Assert.IsNotNull(facet);
        }


        [Test]
        public void TestImmutableFacetsIsInstalledIfSpecifiesImmutable() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatSpecifiesImmutableSemantic), MethodRemover, FacetHolder);

            var facet = (IImmutableFacet) FacetHolder.GetFacet(typeof (IImmutableFacet));
            Assert.IsNotNull(facet);
        }


        [Test]
        public void TestImmutableFacetsIsNotInstalledIfSpecifiesNotImmutable() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatSpecifiesNotImmutableSemantic), MethodRemover, FacetHolder);

            var facet = (IImmutableFacet) FacetHolder.GetFacet(typeof (IImmutableFacet));
            Assert.IsNull(facet);
        }

        [Test]
        public void TestNoMethodsRemoved() {
            facetFactory.Process(typeof (MyParseableUsingParserName2), MethodRemover, FacetHolder);

            AssertNoMethodsRemoved();
        }

        [Test]
        public void TestPickUpSemanticsProviderViaClassAndInstallsValueFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderUsingSemanticsProviderClass), MethodRemover, FacetHolder);

            Assert.IsNotNull(FacetHolder.GetFacet(typeof (IValueFacet)));
        }

        [Test]
        public void TestPickUpSemanticsProviderViaNameAndInstallsValueFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderUsingSemanticsProviderName), MethodRemover, FacetHolder);

            Assert.IsNotNull(FacetHolder.GetFacet(typeof (IValueFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderMustBeAValueSemanticsProvider() {
            // no test, because compiler prevents us from nominating a class that doesn't
            // implement ValueSemanticsProvider
        }


        [Test]
        public void TestValueSemanticsProviderMustHaveANoArgConstructor() {
            facetFactory.Process(typeof (MyValueSemanticsProviderWithoutNoArgConstructor), MethodRemover, FacetHolder);

            // the fact that we have an immutable means that the provider wasn't picked up
            Assert.IsNotNull(FacetHolder.GetFacet(typeof (IImmutableFacet)));
        }


        [Test]
        public void TestValueSemanticsProviderMustHaveAPublicNoArgConstructor() {
            facetFactory.Process(typeof (MyValueSemanticsProviderWithoutPublicNoArgConstructor), MethodRemover, FacetHolder);

            // the fact that we have an immutable means that the provider wasn't picked up
            Assert.IsNotNull(FacetHolder.GetFacet(typeof (IImmutableFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderThatIsADefaultsProviderInstallsDefaultedFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatIsADefaultsProvider), MethodRemover, FacetHolder);
            Assert.IsNotNull(FacetHolder.GetFacet(typeof (IDefaultedFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderThatIsAParserInstallsParseableFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatIsAParser), MethodRemover, FacetHolder);
            Assert.IsNotNull(FacetHolder.GetFacet(typeof (IParseableFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderThatIsAParserInstallsTitleFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatIsAParser), MethodRemover, FacetHolder);
            Assert.IsNotNull(FacetHolder.GetFacet(typeof (ITitleFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderThatIsAParserInstallsTypicalLengthFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatIsAParser), MethodRemover, FacetHolder);
            Assert.IsNotNull(FacetHolder.GetFacet(typeof (ITypicalLengthFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderThatIsAnEncoderInstallsEncodeableFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderThatIsAnEncoderDecoder), MethodRemover, FacetHolder);

            Assert.IsNotNull(FacetHolder.GetFacet(typeof (IEncodeableFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderThatIsNotADefaultsProviderDoesNotInstallDefaultedFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderUsingSemanticsProviderClass), MethodRemover, FacetHolder);
            Assert.IsNull(FacetHolder.GetFacet(typeof (IDefaultedFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderThatIsNotAParserDoesNotInstallParseableFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderUsingSemanticsProviderClass), MethodRemover, FacetHolder);
            Assert.IsNull(FacetHolder.GetFacet(typeof (IParseableFacet)));
        }

        [Test]
        public void TestValueSemanticsProviderThatIsNotAnEncoderDecoderDoesNotInstallEncodeableFacet() {
            facetFactory.Process(typeof (MyValueSemanticsProviderUsingSemanticsProviderClass), MethodRemover, FacetHolder);

            Assert.IsNull(FacetHolder.GetFacet(typeof (IEncodeableFacet)));
        }
    }
}