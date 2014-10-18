// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Spec;

namespace NakedObjects.Metamodel.Facet {
    public class DotNetArrayFacet : DotNetCollectionFacet {
        public DotNetArrayFacet(ISpecification holder)
            : base(holder) {}

        public DotNetArrayFacet(ISpecification holder, Type elementType)
            : base(holder, elementType) {}


        public override void Init(INakedObject collection, INakedObject[] initData) {
            Array newCollection = Array.CreateInstance(collection.GetDomainObject().GetType().GetElementType(), initData.Length);
            collection.ReplacePoco(newCollection);

            int i = 0;
            foreach (INakedObject nakedObject in initData) {
                AsCollection(collection)[i++] = nakedObject.Object;
            }
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}