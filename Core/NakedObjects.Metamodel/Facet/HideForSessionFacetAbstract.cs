// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using NakedObjects.Architecture;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Facet;
using NakedObjects.Architecture.Interactions;
using NakedObjects.Architecture.Spec;

namespace NakedObjects.Meta.Facet {
    /// <summary>
    ///     Hide a property, collection or action based on the current session.
    /// </summary>
    /// <para>
    ///     In the standard Naked Objects Programming Model, corresponds to
    ///     invoking the <c>HideXxx</c> support method for the member.
    /// </para>
    /// 
    [Serializable]
    public abstract class HideForSessionFacetAbstract : FacetAbstract, IHideForSessionFacet {
        protected HideForSessionFacetAbstract(ISpecification holder)
            : base(Type, holder) {}

        public static Type Type {
            get { return typeof (IHideForSessionFacet); }
        }

        #region IHideForSessionFacet Members

        public virtual string Hides(InteractionContext ic, ILifecycleManager lifecycleManager, IMetamodelManager manager) {
            return HiddenReason(ic.Session, ic.Target, lifecycleManager, manager);
        }

        public virtual HiddenException CreateExceptionFor(InteractionContext ic, ILifecycleManager lifecycleManager, IMetamodelManager manager) {
            return new HiddenException(ic, Hides(ic, lifecycleManager, manager));
        }

        public abstract string HiddenReason(ISession session, INakedObject target, ILifecycleManager persistor, IMetamodelManager manager);

        #endregion
    }


    // Copyright (c) Naked Objects Group Ltd.
}