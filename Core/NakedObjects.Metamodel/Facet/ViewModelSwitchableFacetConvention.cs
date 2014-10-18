// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using NakedObjects.Architecture;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Spec;

namespace NakedObjects.Metamodel.Facet {
    public class ViewModelSwitchableFacetConvention : ViewModelFacetConvention {
        public ViewModelSwitchableFacetConvention(ISpecification holder) : base(Type, holder) {}

        private static Type Type {
            get { return typeof (IViewModelFacet); }
        }


        public override bool IsEditView(INakedObject nakedObject) {
            var target = nakedObject.GetDomainObject<IViewModelSwitchable>();

            if (target != null) {
                return target.IsEditView();
            }

            throw new NakedObjectSystemException(nakedObject.Object == null ? "Null domain object" : "Wrong type of domain object: " + nakedObject.Object.GetType().FullName);
        }
    }
}