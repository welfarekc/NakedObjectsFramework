// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using Common.Logging;
using NakedObjects.Facade;
using RestfulObjects.Snapshot.Constants;
using RestfulObjects.Snapshot.Utility;

namespace RestfulObjects.Snapshot.Representations {
    [DataContract]
    public class HomePageRepresentation : Representation {
        private static readonly ILog Log = LogManager.GetLogger(typeof (HomePageRepresentation));

        protected HomePageRepresentation(IOidStrategy oidStrategy, HttpRequestMessage req, RestControlFlags flags)
            : base(oidStrategy, flags) {
            Log.DebugFormat("HomePageRepresentation");
            SelfRelType = new HomePageRelType(RelValues.Self, new UriMtHelper(oidStrategy ,req));
            SetLinks(req);
            SetExtensions();
            SetHeader();
        }

        [DataMember(Name = JsonPropertyNames.Links)]
        public LinkRepresentation[] Links { get; set; }

        [DataMember(Name = JsonPropertyNames.Extensions)]
        public MapRepresentation Extensions { get; set; }

        private void SetHeader() {
            caching = CacheType.NonExpiring;
        }

        private void SetExtensions() {
            Extensions = new MapRepresentation();
        }

        private void SetLinks(HttpRequestMessage req) {
            var tempLinks = new List<LinkRepresentation> {
                LinkRepresentation.Create(OidStrategy ,SelfRelType, Flags),
                LinkRepresentation.Create(OidStrategy, new UserRelType(new UriMtHelper(OidStrategy ,req)), Flags),
                LinkRepresentation.Create(OidStrategy,new ListRelType(RelValues.Services, SegmentValues.Services, new UriMtHelper(OidStrategy ,req)), Flags),
                LinkRepresentation.Create(OidStrategy,new VersionRelType(new UriMtHelper(OidStrategy ,req)), Flags)
            };

            if (Flags.FormalDomainModel) {
                tempLinks.Add(LinkRepresentation.Create(OidStrategy ,new TypesRelType(new UriMtHelper(OidStrategy, req)), Flags));
            }

            Links = tempLinks.ToArray();
        }

        public static HomePageRepresentation Create(IOidStrategy oidStrategy,  HttpRequestMessage req, RestControlFlags flags) {
            return new HomePageRepresentation(oidStrategy, req, flags);
        }
    }
}