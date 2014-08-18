// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System;
using NUnit.Framework;
using NakedObjects.Architecture.Facets;
using NakedObjects.Architecture.Facets.Naming.Named;
using NakedObjects.Architecture.Facets.Objects.Defaults;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Capabilities;

namespace NakedObjects.Reflector.DotNet.Facets.Objects.Defaults {
    [TestFixture]
    public class DefaultedFacetFactoryTest : AbstractFacetFactoryTest {
        [SetUp]
        public override void SetUp() {
            base.SetUp();
            facetFactory = new DefaultedFacetFactory (reflector);
        }

        [TearDown]
        public override void TearDown() {
            facetFactory = null;
            base.TearDown();
        }

        private DefaultedFacetFactory facetFactory;

        protected override Type[] SupportedTypes {
            get { return new[] {typeof (INamedFacet)}; }
        }

        protected override IFacetFactory FacetFactory {
            get { return facetFactory; }
        }

        public void TestDefaultedMustBeADefaultsProvider() {
            // no test, because compiler prevents us from nominating a class that doesn't
            // implement DefaultsProvider
        }

        public abstract class DefaultsProviderNoop<T> : IDefaultsProvider<T> {
            #region IDefaultsProvider Members

            public abstract T DefaultValue { get; }

            #endregion
        }

        [Defaulted(DefaultsProviderName = "NakedObjects.Reflector.DotNet.Facets.Objects.Defaults.DefaultedFacetFactoryTest+MyDefaultedUsingDefaultsProvider")]
        public class MyDefaultedUsingDefaultsProvider : DefaultsProviderNoop<MyDefaultedUsingDefaultsProvider> {
            // Required since is a DefaultsProvider.

            public override MyDefaultedUsingDefaultsProvider DefaultValue {
                get { return new MyDefaultedUsingDefaultsProvider(); }
            }
        }

        [Defaulted(DefaultsProviderClass = typeof (MyDefaultedUsingDefaultsProviderClass))]
        public class MyDefaultedUsingDefaultsProviderClass : DefaultsProviderNoop<MyDefaultedUsingDefaultsProviderClass> {
            // Required since is a DefaultsProvider.

            public override MyDefaultedUsingDefaultsProviderClass DefaultValue {
                get { return new MyDefaultedUsingDefaultsProviderClass(); }
            }
        }

        [Defaulted]
        public class MyDefaultedWithDefaultsProviderSpecifiedUsingConfiguration : DefaultsProviderNoop<MyDefaultedWithDefaultsProviderSpecifiedUsingConfiguration> {
            // Required since is a DefaultsProvider.

            public override MyDefaultedWithDefaultsProviderSpecifiedUsingConfiguration DefaultValue {
                get { return new MyDefaultedWithDefaultsProviderSpecifiedUsingConfiguration(); }
            }
        }

        [Defaulted(DefaultsProviderClass = typeof (MyDefaultedWithoutNoArgConstructor))]
        public class MyDefaultedWithoutNoArgConstructor : DefaultsProviderNoop<MyDefaultedWithoutNoArgConstructor> {
            // no no-arg constructor

            public MyDefaultedWithoutNoArgConstructor(int value) {}

            public override MyDefaultedWithoutNoArgConstructor DefaultValue {
                get { return new MyDefaultedWithoutNoArgConstructor(0); }
            }
        }

        [Defaulted(DefaultsProviderClass = typeof (MyDefaultedWithoutPublicNoArgConstructor))]
        public class MyDefaultedWithoutPublicNoArgConstructor : DefaultsProviderNoop<MyDefaultedWithoutPublicNoArgConstructor> {
            // no public no-arg constructor
            private MyDefaultedWithoutPublicNoArgConstructor() {}

            public MyDefaultedWithoutPublicNoArgConstructor(int value) {}


            public override MyDefaultedWithoutPublicNoArgConstructor DefaultValue {
                get { return new MyDefaultedWithoutPublicNoArgConstructor(); }
            }
        }

        public class NonAnnotatedDefaultedDefaultsProviderSpecifiedUsingConfiguration : DefaultsProviderNoop<NonAnnotatedDefaultedDefaultsProviderSpecifiedUsingConfiguration> {
            // Required since is a DefaultsProvider.

            public override NonAnnotatedDefaultedDefaultsProviderSpecifiedUsingConfiguration DefaultValue {
                get { return new NonAnnotatedDefaultedDefaultsProviderSpecifiedUsingConfiguration(); }
            }
        }

        [Test]
        public void TestDefaultedHaveAPublicNoArgConstructor() {
            facetFactory.Process(typeof (MyDefaultedWithoutPublicNoArgConstructor), methodRemover, facetHolder);
            var facet = (DefaultedFacetAbstract<MyDefaultedWithoutPublicNoArgConstructor>) facetHolder.GetFacet(typeof (IDefaultedFacet));
            Assert.IsNull(facet);
        }

        [Test]
        public void TestDefaultedMustHaveANoArgConstructor() {
            facetFactory.Process(typeof (MyDefaultedWithoutNoArgConstructor), methodRemover, facetHolder);
            var facet = (DefaultedFacetAbstract<MyDefaultedWithoutNoArgConstructor>) facetHolder.GetFacet(typeof (IDefaultedFacet));
            Assert.IsNull(facet);
        }

        [Test]
        public void TestDefaultedUsingDefaultsProviderClass() {
            facetFactory.Process(typeof (MyDefaultedUsingDefaultsProviderClass), methodRemover, facetHolder);
            var facet = (DefaultedFacetAbstract<MyDefaultedUsingDefaultsProviderClass>) facetHolder.GetFacet(typeof (IDefaultedFacet));
            Assert.AreEqual(typeof (MyDefaultedUsingDefaultsProviderClass), facet.GetDefaultsProviderClass());
        }

        [Test]
        public void TestDefaultedUsingDefaultsProviderName() {
            facetFactory.Process(typeof (MyDefaultedUsingDefaultsProvider), methodRemover, facetHolder);
            var facet = (DefaultedFacetAbstract<MyDefaultedUsingDefaultsProvider>) facetHolder.GetFacet(typeof (IDefaultedFacet));
            Assert.AreEqual(typeof (MyDefaultedUsingDefaultsProvider), facet.GetDefaultsProviderClass());
        }

        [Test]
        public void TestFacetFacetHolderStored() {
            facetFactory.Process(typeof (MyDefaultedUsingDefaultsProvider), methodRemover, facetHolder);
            var valueFacet = (DefaultedFacetAbstract<MyDefaultedUsingDefaultsProvider>) facetHolder.GetFacet(typeof (IDefaultedFacet));
            Assert.AreEqual(facetHolder, valueFacet.FacetHolder);
        }

        [Test]
        public void TestFacetPickedUp() {
            facetFactory.Process(typeof (MyDefaultedUsingDefaultsProvider), methodRemover, facetHolder);

            var facet = (IDefaultedFacet) facetHolder.GetFacet(typeof (IDefaultedFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is DefaultedFacetAbstract<MyDefaultedUsingDefaultsProvider>);
        }

        [Test]
        public override void TestFeatureTypes() {
            NakedObjectFeatureType[] featureTypes = facetFactory.FeatureTypes;
            Assert.IsTrue(Contains(featureTypes, NakedObjectFeatureType.Objects));
            Assert.IsTrue(Contains(featureTypes, NakedObjectFeatureType.Property));
            Assert.IsFalse(Contains(featureTypes, NakedObjectFeatureType.Collection));
            Assert.IsFalse(Contains(featureTypes, NakedObjectFeatureType.Action));
            Assert.IsTrue(Contains(featureTypes, NakedObjectFeatureType.ActionParameter));
        }

        [Test]
        public void TestNoMethodsRemoved() {
            facetFactory.Process(typeof (MyDefaultedUsingDefaultsProvider), methodRemover, facetHolder);
            AssertNoMethodsRemoved();
        }
    }


    // Copyright (c) Naked Objects Group Ltd.
}