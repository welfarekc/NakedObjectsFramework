﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Linq;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Component;
using NakedObjects.Facade.Translation;

namespace NakedObjects.Facade.Impl.Implementation {
    public class OidTranslatorSemiColonSeparatedList : IOidTranslator {
        private readonly ILifecycleManager lifecycleManager;

        public OidTranslatorSemiColonSeparatedList(ILifecycleManager lifecycleManager) {
            this.lifecycleManager = lifecycleManager;
        }

        #region IOidTranslator Members

        public IOidTranslation GetOidTranslation(params string[] id) {
            if (id.Count() != 1) {
                throw new ObjectResourceNotFoundNOSException(id.Aggregate((s, t) => s + " " + t));
            }
            if (string.IsNullOrEmpty(id.First())) {
                return null;
            }

            return new OidTranslationSemiColonSeparatedList(id.First());
        }

        public IOidTranslation GetOidTranslation(IObjectFacade objectFacade) {
            if (objectFacade.IsViewModel) {
                var vm = ((ObjectFacade) objectFacade).WrappedNakedObject;
                lifecycleManager.PopulateViewModelKeys(vm);
            }

            var oid = objectFacade.Oid;
            var id = GetObjectId(oid);
            return GetOidTranslation(id);
        }

        #endregion

        private string Encode(IEncodedToStrings encoder) {
            return encoder.ToShortEncodedStrings().Aggregate((a, b) => a + ";" + b);
        }

        // todo is this best place for this
        //private string GetObjectId(INakedObjectAdapter objectFacade) {
        //    if (objectFacade.Spec.IsViewModel) {
        //        // todo this always repopulates oid now - see core - look into optimizing
        //        framework.LifecycleManager.PopulateViewModelKeys(objectFacade);
        //    }
        //    else if (objectFacade.Oid == null) {
        //        return "";
        //    }

        //    return GetObjectId(objectFacade.Oid);
        //}

        private string GetObjectId(IOidFacade oid) {
            return Encode(((IEncodedToStrings) oid.Value));
        }
    }
}