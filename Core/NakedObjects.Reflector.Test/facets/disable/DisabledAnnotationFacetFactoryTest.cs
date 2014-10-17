// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections;
using System.Reflection;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.FacetFactory;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Metamodel.Facet;
using NakedObjects.Reflector.FacetFactory;
using NUnit.Framework;

namespace NakedObjects.Reflector.DotNet.Facets.Disable {
    [TestFixture]
    public class DisabledAnnotationFacetFactoryTest : AbstractFacetFactoryTest {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp() {
            base.SetUp();
            facetFactory = new DisabledAnnotationFacetFactory(Reflector);
        }

        [TearDown]
        public override void TearDown() {
            facetFactory = null;
            base.TearDown();
        }

        #endregion

        private DisabledAnnotationFacetFactory facetFactory;

        protected override Type[] SupportedTypes {
            get { return new[] {typeof (IDisabledFacet)}; }
        }

        protected override IFacetFactory FacetFactory {
            get { return facetFactory; }
        }

        private class Customer {
            [Disabled]
            public int NumberOfOrders {
                get { return 0; }
            }
        }

        private class Customer1 {
            [Disabled]
            public IList Orders {
                get { return null; }
            }
        }

        private class Customer2 {
            [Disabled]
            public void SomeAction() {}
        }

        private class Customer3 {
            [Disabled(WhenTo.Always)]
            public void SomeAction() {}
        }

        private class Customer4 {
            [Disabled(WhenTo.Never)]
            public void SomeAction() {}
        }

        private class Customer5 {
            [Disabled(WhenTo.OncePersisted)]
            public void SomeAction() {}
        }

        private class Customer6 {
            [Disabled(WhenTo.UntilPersisted)]
            public void SomeAction() {}
        }

        [Test]
        public void TestDisabledAnnotationPickedUpOnAction() {
            MethodInfo actionMethod = FindMethod(typeof (Customer2), "SomeAction");
            facetFactory.Process(actionMethod, MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IDisabledFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is DisabledFacetAbstract);
            AssertNoMethodsRemoved();
        }

        [Test]
        public void TestDisabledAnnotationPickedUpOnCollection() {
            PropertyInfo property = FindProperty(typeof (Customer1), "Orders");
            facetFactory.Process(property, MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IDisabledFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is DisabledFacetAbstract);
            AssertNoMethodsRemoved();
        }

        [Test]
        public void TestDisabledAnnotationPickedUpOnProperty() {
            PropertyInfo property = FindProperty(typeof (Customer), "NumberOfOrders");
            facetFactory.Process(property, MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IDisabledFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is DisabledFacetAbstract);
            AssertNoMethodsRemoved();
        }

        [Test]
        public void TestDisabledWhenAlwaysAnnotationPickedUpOn() {
            MethodInfo actionMethod = FindMethod(typeof (Customer3), "SomeAction");
            facetFactory.Process(actionMethod, MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IDisabledFacet));
            var disabledFacetAbstract = (DisabledFacetAbstract) facet;
            Assert.AreEqual(WhenTo.Always, disabledFacetAbstract.Value);
        }

        [Test]
        public void TestDisabledWhenNeverAnnotationPickedUpOn() {
            MethodInfo actionMethod = FindMethod(typeof (Customer4), "SomeAction");
            facetFactory.Process(actionMethod, MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IDisabledFacet));
            var disabledFacetAbstract = (DisabledFacetAbstract) facet;
            Assert.AreEqual(WhenTo.Never, disabledFacetAbstract.Value);
        }

        [Test]
        public void TestDisabledWhenOncePersistedAnnotationPickedUpOn() {
            MethodInfo actionMethod = FindMethod(typeof (Customer5), "SomeAction");
            facetFactory.Process(actionMethod, MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IDisabledFacet));
            var disabledFacetAbstract = (DisabledFacetAbstract) facet;
            Assert.AreEqual(WhenTo.OncePersisted, disabledFacetAbstract.Value);
        }

        [Test]
        public void TestDisabledWhenUntilPersistedAnnotationPickedUpOn() {
            MethodInfo actionMethod = FindMethod(typeof (Customer6), "SomeAction");
            facetFactory.Process(actionMethod, MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IDisabledFacet));
            var disabledFacetAbstract = (DisabledFacetAbstract) facet;
            Assert.AreEqual(WhenTo.UntilPersisted, disabledFacetAbstract.Value);
        }

        [Test]
        public override void TestFeatureTypes() {
            FeatureType[] featureTypes = facetFactory.FeatureTypes;
            Assert.IsFalse(Contains(featureTypes, FeatureType.Objects));
            Assert.IsTrue(Contains(featureTypes, FeatureType.Property));
            Assert.IsTrue(Contains(featureTypes, FeatureType.Collection));
            Assert.IsTrue(Contains(featureTypes, FeatureType.Action));
            Assert.IsFalse(Contains(featureTypes, FeatureType.ActionParameter));
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}