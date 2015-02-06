// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Security;

namespace NakedObjects.Architecture.Facets.Disable {
    public class DisableForSessionFacetNone : DisableForSessionFacetAbstract {
        public DisableForSessionFacetNone(IFacetHolder holder)
            : base(holder) {}

        public override bool IsNoOp {
            get { return true; }
        }

        public override string DisabledReason(ISession session, INakedObject target) {
            return null;
        }
    }


    // Copyright (c) Naked Objects Group Ltd.
}