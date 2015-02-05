// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System;
using NakedObjects.Architecture.Facets;

namespace NakedObjects.Architecture.Adapter.Value {
    public interface ISbyteValueFacet : IFacet {
        SByte SByteValue(INakedObject nakedObject);

        INakedObject CreateValue(SByte sbyteValue);
    }

    // Copyright (c) Naked Objects Group Ltd.
}