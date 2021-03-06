// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Runtime.Serialization;
using Common.Logging;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Interactions;
using NakedObjects.Architecture.Spec;

namespace NakedObjects.Meta.Facet {
    [Serializable]
    public abstract class FacetAbstract : IFacet, IDeserializationCallback {
        private static readonly ILog Log = LogManager.GetLogger(typeof (FacetAbstract));
        private readonly Type facetType;
        private ISpecification holder;

        protected FacetAbstract(Type facetType, ISpecification holder) {
            this.facetType = facetType;
            this.holder = holder;
        }

        /// <summary>
        ///     Convenience for creating exceptions by those facets that implement
        ///     <see cref="IValidatingInteractionAdvisor" />, <see cref="IHidingInteractionAdvisor" /> or
        ///     <see cref="IDisablingInteractionAdvisor" />
        /// </summary>
        protected internal virtual string SpecificationId {
            get { return Specification.Identifier.ToIdentityString(IdentifierDepth.Class); }
        }

        #region IDeserializationCallback Members

        public virtual void OnDeserialization(object sender) {
            Log.DebugFormat("OnDeserialization {0}", GetType());
        }

        #endregion

        #region IFacet Members

        public virtual ISpecification Specification {
            get { return holder; }
            set { holder = value; }
        }

        /// <summary>
        ///     Assume implementation is <i>not</i> a no-op.
        /// </summary>
        /// <para>
        ///     No-op implementations should override and return <c>true</c>.
        /// </para>
        public virtual bool IsNoOp {
            get { return false; }
        }

        public Type FacetType {
            get { return facetType; }
        }

        /// <summary>
        ///     Default implementation of this method that returns <c>true</c>, ie
        ///     should replace non-<see cref="IsNoOp" /> implementations.
        /// </summary>
        /// <para>
        ///     Implementations that don't wish to replace non-<see cref="IsNoOp" /> implementations
        ///     should override and return <c>false</c>.
        /// </para>
        public virtual bool CanAlwaysReplace {
            get { return true; }
        }

        #endregion

        public override string ToString() {
            string details = "";
            if (typeof (IValidatingInteractionAdvisor).IsAssignableFrom(GetType())) {
                details += "Validating";
            }
            if (typeof (IDisablingInteractionAdvisor).IsAssignableFrom(GetType())) {
                details += (details.Length > 0 ? ";" : "") + "Disabling";
            }
            if (typeof (IHidingInteractionAdvisor).IsAssignableFrom(GetType())) {
                details += (details.Length > 0 ? ";" : "") + "Hiding";
            }
            if (!"".Equals(details)) {
                details = "interaction=" + details + ",";
            }
            if (GetType() != FacetType) {
                string sFacetType = FacetType.FullName;
                details += "type=" + sFacetType.Substring(sFacetType.LastIndexOf('.') + 1);
            }
            string stringValues = ToStringValues();
            if (!"".Equals(stringValues)) {
                details += ",";
            }
            string typeName = GetType().FullName;
            int last = typeName.IndexOf('`');
            if (last == -1) {
                last = typeName.Length - 1;
            }
            return typeName.Substring(typeName.LastIndexOf('.', last) + 1) + "[" + details + stringValues + "]";
        }

        protected virtual string ToStringValues() {
            return "";
        }
    }
}