﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Serialization;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Menu;
using NakedObjects.Architecture.SpecImmutable;
using NakedObjects.Core;
using NakedObjects.Core.Util;
using NakedObjects.Menu;
using NakedObjects.Meta.Utils;
using NakedObjects.Architecture.Facet;

namespace NakedObjects.Meta.Menu {
    [Serializable]
    public class MenuImpl : IMenu, IMenuImmutable, ISerializable, IDeserializationCallback {
        private readonly IMetamodel metamodel;

        public MenuImpl(IMetamodel metamodel, Type type, bool addAllActions, string name) {
            this.metamodel = metamodel;
            this.Type = type;
            Name = name ?? ObjectSpec.GetFacet<INamedFacet>().NaturalName;
            Id = type.Name;
            if (addAllActions) {
                AddRemainingNativeActions();
                AddContributedActions();
            }
        }

        #region IMenu Members

        public IMenu WithMenuName(string name) {
            Name = name;
            return this;
        }

        public Type Type { get; set; }

        public IMenu WithId(string id) {
            Id = id;
            return this;
        }

        public IMenu AddAction(string actionName) {
            IActionSpecImmutable actionSpec = ActionsForObject.FirstOrDefault(a => a.Identifier.MemberName == actionName);
            if (actionSpec == null) {
                throw new ReflectionException("No such action: " + actionName + " on " + Type);
            }
            AddMenuItem(new MenuAction(actionSpec));
            return this;
        }

        public IMenu CreateSubMenu(string subMenuName) {
            return CreateMenuImmutableAsSubMenu(subMenuName, null);
        }

        public IMenu GetSubMenu(string menuName) {
            MenuImpl menu = GetSubMenuIfExists(menuName);
            if (menu == null) {
                throw new Exception("No sub-menu named " + menuName);
            }
            return menu;
        }

        public IMenu AddRemainingNativeActions() {
            AddOrderableElementsToMenu(ActionsForObject, this);
            return this;
        }

        public IMenu AddContributedActions() {
            var spec = ObjectSpec as IObjectSpecImmutable;
            if (spec != null) {
                spec.ContributedActions.ForEach(ca => AddContributedAction(ca, spec));
            }
            return this;
        }

        #endregion

        #region other properties

        private ImmutableList<IMenuItemImmutable> items = ImmutableList<IMenuItemImmutable>.Empty;

        /// <summary>
        /// Will only be set if this menu is a sub-menu of another.
        /// </summary>
        public IMenu SuperMenu { get; private set; }

        /// <summary>
        /// The name of the menu -  will typically be rendered on the UI
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id is optional.  It is only included to facilitate backwards compatibility with
        /// existing auto-generated menus.
        /// </summary>
        public string Id { get; set; }

        //Includes both actions and sub-menus
        public IList<IMenuItemImmutable> MenuItems {
            get { return items; }
        }

        protected IMetamodel Metamodel {
            get { return metamodel; }
        }

        #endregion

        private MenuImpl CreateMenuImmutableAsSubMenu(string subMenuName, string id = null) {
            var subMenu = new MenuImpl(Metamodel, Type, false, subMenuName);
            if (id == null) {
                subMenu.Id += "-" + subMenuName + ":";
            } else {
                subMenu.Id = id;
            }
            subMenu.SuperMenu = this;
            AddMenuItem(subMenu);
            return subMenu;
        }

        private ITypeSpecImmutable ObjectSpec {
            get {
                return Metamodel.GetSpecification(Type);
            }
        }

        private IList<IActionSpecImmutable> ActionsForObject {
            get {
                return ObjectSpec.ObjectActions.ToList();
            }
        }

        private bool HasAction(IActionSpecImmutable action) {
            bool nativeAction = MenuItems.OfType<MenuAction>().Any(mi => mi.Action == action);
            if (nativeAction) return true;
            return MenuItems.OfType<MenuImpl>().Any(m => m.HasAction(action));
        }

        private MenuImpl GetSubMenuIfExists(string menuName) {
            return MenuItems.OfType<MenuImpl>().FirstOrDefault(a => a.Name == menuName);
        }

        private void AddOrderableElementsToMenu(IList<IActionSpecImmutable> ordeableElements, MenuImpl toMenu) {
            foreach (IActionSpecImmutable action in ordeableElements) {
                if (action != null) {
                    if (!toMenu.HasAction(action)) {
                        toMenu.AddMenuItem(new MenuAction(action));
                    }
                }
            }
        }

        private void AddMenuItem(IMenuItemImmutable item) {
            items = items.Add(item); //Only way to add to an immutable collection
        }

        private void AddContributedAction(IActionSpecImmutable ca, IObjectSpecImmutable spec) {
            var facet = ca.GetFacet<IContributedActionFacet>();
            string subMenuName = facet.SubMenuWhenContributedTo(spec);
            if (subMenuName != null) {
                string id = facet.IdWhenContributedTo(spec);
                MenuImpl subMenu = GetSubMenuIfExists(subMenuName) ?? CreateMenuImmutableAsSubMenu(subMenuName, id);
                subMenu.AddOrderableElementsToMenu(new List<IActionSpecImmutable> { ca }, subMenu);
            } else {
                //i.e. no sub-menu
                AddMenuItem(new MenuAction(ca));
            }
        }

        #region ISerializable

        private readonly IList<IMenuItemImmutable> tempItems;

        // The special constructor is used to deserialize values. 
        protected MenuImpl(SerializationInfo info, StreamingContext context) {
            tempItems = info.GetValue<IList<IMenuItemImmutable>>("items");
            SuperMenu = info.GetValue<IMenu>("SuperMenu");
            Name = info.GetValue<string>("Name");
            Id = info.GetValue<string>("Id");
            metamodel = info.GetValue<IMetamodel>("Metamodel");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue<IList<IMenuItemImmutable>>("items", items.ToList());
            info.AddValue<IMenu>("SuperMenu", SuperMenu);
            info.AddValue<string>("Name", Name);
            info.AddValue<string>("Id", Id);
            info.AddValue<IMetamodel>("metamodel", metamodel);
        }

        public void OnDeserialization(object sender) {
            items = tempItems.ToImmutableList();
        }

        #endregion
    }
}