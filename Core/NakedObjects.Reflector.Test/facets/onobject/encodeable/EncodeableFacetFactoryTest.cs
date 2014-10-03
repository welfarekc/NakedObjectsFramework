// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System;
using NakedObjects.Architecture.Facets;
using NakedObjects.Architecture.Facets.Objects.Encodeable;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Capabilities;
using NUnit.Framework;

namespace NakedObjects.Reflector.DotNet.Facets.Objects.Encodeable {
    [TestFixture]
    public class EncodeableFacetFactoryTest : AbstractFacetFactoryTest {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp() {
            base.SetUp();
            facetFactory = new EncodeableFacetFactory(Reflector);
        }

        [TearDown]
        public override void TearDown() {
            facetFactory = null;
            base.TearDown();
        }

        #endregion

        private EncodeableFacetFactory facetFactory;

        protected override Type[] SupportedTypes {
            get { return new[] {typeof (IEncodeableFacet)}; }
        }

        protected override IFacetFactory FacetFactory {
            get { return facetFactory; }
        }

        public abstract class EncoderDecoderNoop<T> : IEncoderDecoder<T> {
            #region IEncoderDecoder<T> Members

            public T FromEncodedString(string encodedString) {
                return default(T);
            }

            public string ToEncodedString(T toEncode) {
                return null;
            }

            #endregion
        }

        [Encodeable(EncoderDecoderClass = typeof (MyEncodeableUsingEncoderDecoderClass))]
        public class MyEncodeableUsingEncoderDecoderClass : EncoderDecoderNoop<MyEncodeableUsingEncoderDecoderClass> {
            // Required since is a EncoderDecoder.
        }

        [Encodeable(EncoderDecoderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Encodeable.EncodeableFacetFactoryTest+MyEncodeableUsingEncoderDecoderName")]
        public class MyEncodeableUsingEncoderDecoderName : EncoderDecoderNoop<MyEncodeableUsingEncoderDecoderName> {
            // Required since is an EncoderDecoder
        }

        [Encodeable]
        public class MyEncodeableWithEncoderDecoderSpecifiedUsingConfiguration : EncoderDecoderNoop<MyEncodeableWithEncoderDecoderSpecifiedUsingConfiguration> {
            // Required since is a EncoderDecoder.
        }

        [Encodeable(EncoderDecoderClass = typeof (MyEncodeableWithoutNoArgConstructor))]
        public class MyEncodeableWithoutNoArgConstructor : EncoderDecoderNoop<MyEncodeableWithoutNoArgConstructor> {
            // no no-arg constructor

            public MyEncodeableWithoutNoArgConstructor(int value) {}
        }

        [Encodeable(EncoderDecoderClass = typeof (MyEncodeableWithoutPublicNoArgConstructor))]
        public class MyEncodeableWithoutPublicNoArgConstructor : EncoderDecoderNoop<MyEncodeableWithoutPublicNoArgConstructor> {
            // no public no-arg constructor
            private MyEncodeableWithoutPublicNoArgConstructor() {}

            public MyEncodeableWithoutPublicNoArgConstructor(int value) {}
        }

        public class NonAnnotatedEncodeableEncoderDecoderSpecifiedUsingConfiguration : EncoderDecoderNoop<NonAnnotatedEncodeableEncoderDecoderSpecifiedUsingConfiguration> {
            // Required since is a EncoderDecoder.
        }

        [Test]
        public void TestEncodeableHaveANoArgConstructor() {
            facetFactory.Process(typeof (MyEncodeableWithoutNoArgConstructor), MethodRemover, FacetHolder);
            var encodeableFacet = (EncodeableFacetAbstract<MyEncodeableWithoutNoArgConstructor>) FacetHolder.GetFacet(typeof (IEncodeableFacet));
            Assert.IsNull(encodeableFacet);
        }

        [Test]
        public void TestEncodeableHaveAPublicNoArgConstructor() {
            facetFactory.Process(typeof (MyEncodeableWithoutPublicNoArgConstructor), MethodRemover, FacetHolder);

            var encodeableFacet = (EncodeableFacetAbstract<MyEncodeableWithoutPublicNoArgConstructor>) FacetHolder.GetFacet(typeof (IEncodeableFacet));
            Assert.IsNull(encodeableFacet);
        }

        [Test]
        public void TestEncodeableMustBeAEncoderDecoder() {
            // no test, because compiler prevents us from nominating a class that doesn't
            // implement EncoderDecoder
        }

        [Test]
        public void TestEncodeableUsingEncoderDecoderClass() {
            facetFactory.Process(typeof (MyEncodeableUsingEncoderDecoderClass), MethodRemover, FacetHolder);

            var encodeableFacet = (EncodeableFacetAbstract<MyEncodeableUsingEncoderDecoderClass>) FacetHolder.GetFacet(typeof (IEncodeableFacet));
            Assert.AreEqual(typeof (MyEncodeableUsingEncoderDecoderClass), encodeableFacet.GetEncoderDecoderClass());
        }

        [Test]
        public void TestEncodeableUsingEncoderDecoderName() {
            facetFactory.Process(typeof (MyEncodeableUsingEncoderDecoderName), MethodRemover, FacetHolder);
            var encodeableFacet = (EncodeableFacetAbstract<MyEncodeableUsingEncoderDecoderName>) FacetHolder.GetFacet(typeof (IEncodeableFacet));
            Assert.AreEqual(typeof (MyEncodeableUsingEncoderDecoderName), encodeableFacet.GetEncoderDecoderClass());
        }

        [Test]
        public void TestFacetFacetHolderStored() {
            facetFactory.Process(typeof (MyEncodeableUsingEncoderDecoderName), MethodRemover, FacetHolder);

            var valueFacet = (EncodeableFacetAbstract<MyEncodeableUsingEncoderDecoderName>) FacetHolder.GetFacet(typeof (IEncodeableFacet));
            Assert.AreEqual(FacetHolder, valueFacet.FacetHolder);
        }

        [Test]
        public void TestFacetPickedUp() {
            facetFactory.Process(typeof (MyEncodeableUsingEncoderDecoderName), MethodRemover, FacetHolder);

            var facet = (IEncodeableFacet) FacetHolder.GetFacet(typeof (IEncodeableFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is EncodeableFacetAbstract<MyEncodeableUsingEncoderDecoderName>);
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
        public void TestNoMethodsRemoved() {
            facetFactory.Process(typeof (MyEncodeableUsingEncoderDecoderName), MethodRemover, FacetHolder);
            AssertNoMethodsRemoved();
        }
    }
}