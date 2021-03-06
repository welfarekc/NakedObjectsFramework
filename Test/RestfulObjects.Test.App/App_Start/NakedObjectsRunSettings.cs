// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using NakedObjects.Core.Configuration;
using NakedObjects.Persistor.Entity.Configuration;
using NakedObjects.Value;
using RestfulObjects.Test.Data;
using NakedObjects.Architecture.Menu;
using NakedObjects.Menu;

namespace RestfulObjects.Test.App {
    public class NakedObjectsRunSettings {
        private static Type[] Types {
            get { return new Type[] {
                    typeof (WithValueViewModel),
                    typeof (WithReferenceViewModel),
                    typeof (WithCollectionViewModel),
                    typeof (WithActionViewModel),
                    typeof (VerySimple),
                    typeof (TestEnum),
                    typeof (SetWrapper<MostSimple>),
                    typeof (List<MostSimple>),
                    typeof (HashSet<MostSimple>),
                    typeof (WithScalars), 
                    typeof (WithAttachments), 
                    typeof (VerySimpleEager), 
                    typeof (Immutable), 
                    typeof (MostSimple[]), 
                    typeof (Image), 
                    typeof (FileAttachment), 
                    typeof (EntityCollection<object>), 
                    typeof (ObjectQuery<object>),
                    typeof (EntityCollection<object>), 
                    typeof (ObjectQuery<object>)
            }; }
        }

        private static Type[] Services {
            get {
                return new Type[] {
                    typeof (RestDataRepository),
                    typeof (WithActionService),
                    typeof (ContributorService),
                      typeof (TestTypeCodeMapper)
                };
            }
        }



        public static ReflectorConfiguration ReflectorConfig() {
            return new ReflectorConfiguration(Types, Services, Types.Select(t => t.Namespace).Distinct().ToArray());
        }

        public static EntityObjectStoreConfiguration EntityObjectStoreConfig() {
            var config = new EntityObjectStoreConfiguration();
            config.UsingCodeFirstContext(() => new CodeFirstContext("RestTest"));
            return config;
        }

        /// <summary>
        /// Return an array of IMenus (obtained via the factory, then configured) to
        /// specify the Main Menus for the application. If none are returned then
        /// the Main Menus will be derived automatically from the MenuServices.
        /// </summary>
        public static IMenu[] MainMenus(IMenuFactory factory) {
            return new IMenu[] { };
        }
    }
}