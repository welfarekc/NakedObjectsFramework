// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Reflection;
using NakedObjects.Architecture.Facet; 
using NakedObjects.Metamodel.Facet; 
using NakedObjects.Architecture.FacetFactory;
using NakedObjects.Architecture.Reflect;
using NUnit.Framework;

namespace NakedObjects.Reflector.DotNet.Facets.Propparam.Validate.Mask {
    [TestFixture]
    public class MaskAnnotationFacetFactoryTest : AbstractFacetFactoryTest {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp() {
            base.SetUp();
            facetFactory = new MaskAnnotationFacetFactory(Reflector);
        }

        [TearDown]
        public override void TearDown() {
            facetFactory = null;
            base.TearDown();
        }

        #endregion

        private MaskAnnotationFacetFactory facetFactory;

        protected override Type[] SupportedTypes {
            get { return new[] {typeof (IMaskFacet)}; }
        }

        protected override IFacetFactory FacetFactory {
            get { return facetFactory; }
        }

        [Mask("###")]
        private class Customer {}

        private class Customer1 {
            [Mask("###")]
            public string FirstName {
                get { return null; }
            }
        }

        private class Customer2 {
            public void SomeAction([Mask("###")] string foo) {}
        }

        private class Customer3 {
            [Mask("###")]
            public int NumberOfOrders {
                get { return 0; }
            }
        }

        private class Customer4 {
            public void SomeAction([Mask("###")] int foo) {}
        }

        [Test]
        public override void TestFeatureTypes() {
            FeatureType[] featureTypes = facetFactory.FeatureTypes;
            Assert.IsTrue(Contains(featureTypes, FeatureType.Objects));
            Assert.IsTrue(Contains(featureTypes, FeatureType.Property));
            Assert.IsFalse(Contains(featureTypes, FeatureType.Collection));
            Assert.IsFalse(Contains(featureTypes, FeatureType.Action));
            Assert.IsTrue(Contains(featureTypes, FeatureType.ActionParameter));
        }

        [Test]
        public void TestMaskAnnotationNotIgnoredForNonStringsProperty() {
            PropertyInfo property = FindProperty(typeof (Customer3), "NumberOfOrders");
            facetFactory.Process(property, MethodRemover, Specification);
            Assert.IsNotNull(Specification.GetFacet(typeof (IMaskFacet)));
        }

        [Test]
        public void TestMaskAnnotationNotIgnoredForPrimitiveOnActionParameter() {
            MethodInfo method = FindMethod(typeof (Customer4), "SomeAction", new[] {typeof (int)});
            facetFactory.ProcessParams(method, 0, Specification);
            Assert.IsNotNull(Specification.GetFacet(typeof (IMaskFacet)));
        }

        [Test]
        public void TestMaskAnnotationPickedUpOnActionParameter() {
            MethodInfo method = FindMethod(typeof (Customer2), "SomeAction", new[] {typeof (string)});
            facetFactory.ProcessParams(method, 0, Specification);
            IFacet facet = Specification.GetFacet(typeof (IMaskFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is MaskFacetAnnotation);
            var maskFacet = (MaskFacetAnnotation) facet;
            Assert.AreEqual("###", maskFacet.Value);
        }

        [Test]
        public void TestMaskAnnotationPickedUpOnClass() {
            facetFactory.Process(typeof (Customer), MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IMaskFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is MaskFacetAnnotation);
            var maskFacet = (MaskFacetAnnotation) facet;
            Assert.AreEqual("###", maskFacet.Value);
        }

        [Test]
        public void TestMaskAnnotationPickedUpOnProperty() {
            PropertyInfo property = FindProperty(typeof (Customer1), "FirstName");
            facetFactory.Process(property, MethodRemover, Specification);
            IFacet facet = Specification.GetFacet(typeof (IMaskFacet));
            Assert.IsNotNull(facet);
            Assert.IsTrue(facet is MaskFacetAnnotation);
            var maskFacet = (MaskFacetAnnotation) facet;
            Assert.AreEqual("###", maskFacet.Value);
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}