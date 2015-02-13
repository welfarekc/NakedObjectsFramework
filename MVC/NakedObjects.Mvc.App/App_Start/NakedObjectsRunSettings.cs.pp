﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using NakedObjects.Core.Configuration;
using NakedObjects.Persistor.Entity.Configuration;
using NakedObjects.Architecture.Menu;
using NakedObjects.Menu;
using NakedObjects.Meta.Audit;
using NakedObjects.Meta.Authorization;
using NakedObjects.Web.Mvc.Models;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Objects;

namespace $rootnamespace$ {

    // Use this class to configure the application running under Naked Objects
    public class NakedObjectsRunSettings {
        
        // Specify any types that need to be reflected-over by the framework and that
        // will not be discovered via the services
		private static Type[] Types {
            get {
                return new Type[] {
				    // TODO: This list is a temporary kludge pending completion of ticket #4135. 
					// When released, NOF 7 will register these (and other) generic collection types
					// transparently, not explicitly here.
				    typeof (EnumerableQuery<object>),
                    typeof (EntityCollection<object>),
                    typeof (ObjectQuery<object>),
                    typeof (ActionResultModelQ<object>),
                    typeof (NakedObjects.Value.Image),
                    typeof (NakedObjects.Value.FileAttachment)
                };
            }
        }

        private static Type[] Services {
            get {
                return new Type[] {
                };
            }
        }

        public static ReflectorConfiguration ReflectorConfig() {
            return new ReflectorConfiguration(Types, Services, MainMenus);
        }

        public static EntityObjectStoreConfiguration EntityObjectStoreConfig() {
            var config = new EntityObjectStoreConfiguration();
			//config.UsingCodeFirstContext(() => new MyDbContext());
			return config;
        }

		public static IAuditConfiguration AuditConfig() {
            return null; //No auditing set up
			//Example:
            //var config = new AuditConfiguration<MyDefaultAuditor>();
            //config.AddNamespaceAuditor<FooAuditor>("MySpace.Foo");
            //return config;
        }

		public static IAuthorizationConfiguration AuthorizationConfig() {
            return null; //No authorization set up
			//Example:
			//var config = new AuthorizationConfiguration<MyDefaultAuthorizer>();
    		//config.AddTypeAuthorizer<Foo, FooAuthorizer>();
			//config.AddNamespaceAuthorizer<BarAuthorizer>("MySpace.Bar");
			//return config;
        }

		/// <summary>
        /// Return an array of IMenus (obtained via the factory, then configured) to
        /// specify the Main Menus for the application. If none are returned then
        /// the Main Menus will be derived automatically from the Services.
        /// </summary>
		public static IMenu[] MainMenus(IMenuFactory factory) {
            return new IMenu[] {};
        }
    }
}