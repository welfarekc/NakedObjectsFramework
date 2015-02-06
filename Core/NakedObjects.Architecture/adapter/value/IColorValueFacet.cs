// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using NakedObjects.Architecture.Facets;

namespace NakedObjects.Architecture.Adapter.Value {
    public interface IColorValueFacet : IFacet {
        int ColorValue(INakedObject nakedObject);

        INakedObject CreateValue(INakedObject nakedObject, int color);
    }

    // Copyright (c) Naked Objects Group Ltd.
}