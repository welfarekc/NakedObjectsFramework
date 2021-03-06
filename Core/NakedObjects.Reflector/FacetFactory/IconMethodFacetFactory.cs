// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Reflection;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.FacetFactory;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.Spec;
using NakedObjects.Meta.Facet;
using NakedObjects.Meta.Utils;

namespace NakedObjects.Reflect.FacetFactory {
    public sealed class IconMethodFacetFactory : MethodPrefixBasedFacetFactoryAbstract {
        private static readonly string[] FixedPrefixes = {PrefixesAndRecognisedMethods.IconNameMethod};

        public IconMethodFacetFactory(int numericOrder)
            : base(numericOrder, FeatureType.Objects) {}

        public override string[] Prefixes {
            get { return FixedPrefixes; }
        }

        public override void Process(IReflector reflector, Type type, IMethodRemover methodRemover, ISpecificationBuilder specification) {
            MethodInfo method = FindMethod(reflector, type, MethodType.Object, PrefixesAndRecognisedMethods.IconNameMethod, typeof (string), Type.EmptyTypes);
            var attribute = type.GetCustomAttribute<IconNameAttribute>();
            if (method != null) {
                RemoveMethod(methodRemover, method);
                FacetUtils.AddFacet(new IconFacetViaMethod(method, specification, attribute == null ? null : attribute.Value));
            }
            else {
                FacetUtils.AddFacet(Create(attribute, specification));
            }
        }

        private static IIconFacet Create(IconNameAttribute attribute, ISpecification holder) {
            return attribute != null ? new IconFacetAnnotation(attribute.Value, holder) : null;
        }
    }
}