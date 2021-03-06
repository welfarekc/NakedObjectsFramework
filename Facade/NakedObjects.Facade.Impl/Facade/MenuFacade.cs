﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using System.Linq;
using NakedObjects.Architecture.Menu;

namespace NakedObjects.Facade.Impl {
    public class MenuFacade : IMenuFacade {
        public MenuFacade(IMenuImmutable wrapped, IFrameworkFacade facade, INakedObjectsFramework framework) {
            Wrapped = wrapped;
            MenuItems = wrapped.MenuItems.Select(i => Wrap(i, facade, framework)).ToList();
            Name = wrapped.Name;
            Id = wrapped.Id;
        }

        #region IMenuFacade Members

        public object Wrapped { get; private set; }
        public IList<IMenuItemFacade> MenuItems { get; private set; }
        public string Name { get; private set; }
        public string Id { get; private set; }

        #endregion

        private static IMenuItemFacade Wrap(IMenuItemImmutable menu, IFrameworkFacade facade, INakedObjectsFramework framework) {
            var immutable = menu as IMenuActionImmutable;
            if (immutable != null) {
                return new MenuActionFacade(immutable, facade, framework);
            }

            var menuImmutable = menu as IMenuImmutable;
            return menuImmutable != null ? (IMenuItemFacade) new MenuFacade(menuImmutable, facade, framework) : new MenuItemFacade(menu);
        }
    }
}