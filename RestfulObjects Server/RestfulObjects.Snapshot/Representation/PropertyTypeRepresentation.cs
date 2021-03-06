// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using NakedObjects.Facade;
using NakedObjects.Facade.Contexts;
using RestfulObjects.Snapshot.Constants;
using RestfulObjects.Snapshot.Utility;

namespace RestfulObjects.Snapshot.Representations {
    [DataContract]
    public class PropertyTypeRepresentation : MemberTypeRepresentation {
        protected PropertyTypeRepresentation(IOidStrategy oidStrategy, HttpRequestMessage req, PropertyTypeContextFacade propertyContext, RestControlFlags flags)
            : base(oidStrategy, req, propertyContext, flags) {
            SetScalars(propertyContext);
            SetLinks(req, propertyContext);
        }

        [DataMember(Name = JsonPropertyNames.Optional)]
        public bool Optional { get; set; }

        private void SetScalars(PropertyTypeContextFacade propertyContext) {
            Id = propertyContext.Property.Id;
            Optional = !propertyContext.Property.IsMandatory;
        }

        private void SetLinks(HttpRequestMessage req, PropertyTypeContextFacade propertyContext) {
            IList<LinkRepresentation> tempLinks = CreateLinks(req, propertyContext);
            tempLinks.Add(LinkRepresentation.Create(OidStrategy ,new DomainTypeRelType(RelValues.ReturnType, new UriMtHelper(OidStrategy ,req, propertyContext.Property.Specification)), Flags));
            Links = tempLinks.ToArray();
        }

        public new static PropertyTypeRepresentation Create(IOidStrategy oidStrategy, HttpRequestMessage req, PropertyTypeContextFacade propertyContext, RestControlFlags flags) {
            return new PropertyTypeRepresentation(oidStrategy ,req, propertyContext, flags);
        }
    }
}