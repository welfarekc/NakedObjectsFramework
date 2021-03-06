// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using NakedObjects.Architecture.Spec;

namespace NakedObjects.Meta.Spec {
    /// <summary>
    ///     For base subclasses or, more likely, to help write tests
    /// </summary>
    public sealed class NamedAndDescribedSpecification : Specification, INamedAndDescribed {
        private readonly string description;
        private readonly string name;

        public NamedAndDescribedSpecification(string name)
            : this(name, null) {}

        public NamedAndDescribedSpecification(string name, string description) {
            this.name = name;
            this.description = description;
        }

        #region INamedAndDescribed Members

        public string Name {
            get { return name; }
        }

        public string Description {
            get { return description; }
        }

        #endregion
    }
}