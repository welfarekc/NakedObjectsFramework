﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using NakedObjects.Facade;
using RestfulObjects.Snapshot.Constants;

namespace RestfulObjects.Snapshot.Utility {
    public class TypeActionInvokeContext {
        #region ActionType enum

        public enum ActionType {
            IsSubtypeOf,
            IsSupertypeOf
        };

        #endregion

        private readonly ActionType aType;
        private readonly string typeName;

        public TypeActionInvokeContext(string actionName, string typeName) {
            this.typeName = typeName;
            if (actionName == WellKnownIds.IsSupertypeOf) {
                aType = ActionType.IsSupertypeOf;
            }
            else if (actionName == WellKnownIds.IsSubtypeOf) {
                aType = ActionType.IsSubtypeOf;
            }
            else {
                throw new TypeActionResourceNotFoundException(actionName, typeName);
            }
        }

        public string TypeName {
            get { return typeName; }
        }

        public string Id {
            get { return aType == ActionType.IsSubtypeOf ? WellKnownIds.IsSubtypeOf : WellKnownIds.IsSupertypeOf; }
        }

        public string ParameterId {
            get { return aType == ActionType.IsSubtypeOf ? JsonPropertyNames.SuperType : JsonPropertyNames.SubType; }
        }

        public bool Value {
            get { return aType == ActionType.IsSubtypeOf ? ThisSpecification.IsOfType(OtherSpecification) : OtherSpecification.IsOfType(ThisSpecification); }
        }

        public ITypeFacade ThisSpecification { get; set; }
        public ITypeFacade OtherSpecification { get; set; }
    }
}