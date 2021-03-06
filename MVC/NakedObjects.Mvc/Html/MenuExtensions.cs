﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using NakedObjects.Facade;
using NakedObjects.Facade.Utility;
using NakedObjects.Facade.Utility.Restricted;
using NakedObjects.Resources;

namespace NakedObjects.Web.Mvc.Html {
    public static class MenuExtensions {
        /// <summary>
        ///     Create menu from actions of domainObject
        /// </summary>
        public static MvcHtmlString ObjectMenu(this HtmlHelper html, object domainObject) {
            return html.ObjectMenu(domainObject, false);
        }

        /// <summary>
        ///     Create menu from actions of domainObject
        /// </summary>
        public static MvcHtmlString ObjectMenuOnTransient(this HtmlHelper html, object domainObject) {
            return html.ObjectMenu(domainObject, true);
        }

        private static MvcHtmlString ObjectMenu(this HtmlHelper html, object domainObject, bool isEdit) {
            var nakedObject = html.Facade().GetObject(domainObject);
            var objectMenu = nakedObject.Specification.Menu;
            return html.MenuAsHtml(objectMenu, nakedObject, isEdit, true);
        }

        /// <summary>
        /// Create main menus for all menus in ViewData
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString MainMenus(this HtmlHelper html) {
            var mainMenus = html.Facade().GetMainMenus();
            return RenderMainMenus(html, mainMenus);
        }

        private static IMenuFacade GetMenu(HtmlHelper html, object service) {
            return html.Facade().GetObject(service).Specification.Menu;
        }

        public static MvcHtmlString MainMenu(this HtmlHelper html, object service) {
            var menu = GetMenu(html, service);
            return html.MenuAsHtml(menu, null, false, false);
        }

        private static MvcHtmlString RenderMainMenus(this HtmlHelper html, IEnumerable<IMenuFacade> menus) {
            var tag = new TagBuilder("div");
            tag.AddCssClass(IdConstants.ServicesContainerName);
            foreach (var menu in menus) {
                tag.InnerHtml += html.MenuAsHtml(menu, null, false, false);
            }
            return MvcHtmlString.Create(tag.ToString());
        }

        private static MvcHtmlString MenuAsHtml(this HtmlHelper html, IMenuFacade menu, IObjectFacade nakedObject, bool isEdit, bool defaultToEmptyMenu) {
            var descriptors = new List<ElementDescriptor>();
            foreach (IMenuItemFacade item in menu.MenuItems) {
                var descriptor = MenuItemAsElementDescriptor(html, item, nakedObject, isEdit);
                if (IsDuplicateAndIsVisibleActions(html, item, menu.MenuItems, nakedObject)) {
                    //Test that both items are in fact visible
                    //The Id is set just to preseve backwards compatiblity
                    string id = menu.Id;
                    if (id.EndsWith("Actions")) {
                        id = id.Split('-').First() + "-DuplicateAction";
                    }
                    descriptor = new ElementDescriptor {
                        TagType = "div",
                        Value = item.Name,
                        Attributes = new RouteValueDictionary(new {
                            id,
                            @class = IdConstants.ActionName,
                            title = MvcUi.DuplicateAction
                        })
                    };
                }
                if (descriptor != null) {
                    //Would be null for an invisible action
                    descriptors.Add(descriptor);
                }
            }
            if (descriptors.Count == 0 && !defaultToEmptyMenu) {
                return null;
            }
            return CommonHtmlHelper.BuildMenuContainer(descriptors,
                IdConstants.MenuContainerName,
                menu.Id,
                menu.Name);
        }

        private static bool IsDuplicateAndIsVisibleActions(HtmlHelper html,
                                                           IMenuItemFacade item,
                                                           IList<IMenuItemFacade> items,
                                                           IObjectFacade nakedObject) {
            var itemsOfSameName = items.Where(i => i.Name == item.Name).ToArray();
            if (itemsOfSameName.Count() == 1) { return false; }
            return itemsOfSameName.Count(i => MenuActionAsElementDescriptor(html, i as IMenuActionFacade, nakedObject, false) != null) > 1;
        }

        private static ElementDescriptor MenuItemAsElementDescriptor(this HtmlHelper html, IMenuItemFacade item, IObjectFacade nakedObject, bool isEdit) {
            ElementDescriptor descriptor = null;
            if (item is IMenuActionFacade) {
                descriptor = MenuActionAsElementDescriptor(html, item as IMenuActionFacade, nakedObject, isEdit);
            }
            else if (item is IMenuFacade) {
                descriptor = SubMenuAsElementDescriptor(html, item as IMenuFacade, nakedObject, isEdit);
            }
            else if (item is CustomMenuItem) {
                descriptor = CustomMenuItemAsDescriptor(html, item as CustomMenuItem);
            }
            return descriptor;
        }

        private static ElementDescriptor MenuActionAsElementDescriptor(this HtmlHelper html, IMenuActionFacade menuAction, IObjectFacade nakedObject, bool isEdit) {
            var actionIm = menuAction.Action;
            var actionSpec = actionIm.ReturnType;
            if (nakedObject == null) {
                var serviceIm = actionIm.OnType;

                if (serviceIm == null) {
                    throw new Exception("Action is not on a known service");
                }
                nakedObject = html.Facade().GetServices().List.SingleOrDefault(s => s.Specification.Equals(serviceIm));
            }

            if (nakedObject == null) {
                // service may not be visible 
                return null;
            }

            var actionContext = new ActionContext(html.IdHelper(), false, nakedObject, actionIm);

            RouteValueDictionary attributes;
            string tagType;
            string value;
            if (!actionContext.Action.IsVisible(actionContext.Target)) {
                return null;
            }
            var consent = actionContext.Action.IsUsable(actionContext.Target);
            if (consent.IsVetoed) {
                tagType = html.GetVetoedAction(actionContext, consent, out value, out attributes);
            }
            else if (isEdit) {
                tagType = html.GetActionAsButton(actionContext, out value, out attributes);
            }
            else {
                tagType = html.GetActionAsForm(actionContext, html.Facade().GetObjectTypeShortName(actionContext.Target.GetDomainObject()), new {id = html.Facade().OidTranslator.GetOidTranslation(actionContext.Target).Encode()}, out value, out attributes);
            }

            return new ElementDescriptor {
                TagType = tagType,
                Value = value,
                Attributes = attributes
            };
        }

        private static ElementDescriptor SubMenuAsElementDescriptor(this HtmlHelper html, IMenuFacade subMenu, IObjectFacade nakedObject, bool isEdit) {
            string tagType = "div";
            string value = CommonHtmlHelper.WrapInDiv(subMenu.Name, IdConstants.MenuNameLabel).ToString();
            RouteValueDictionary attributes = new RouteValueDictionary(new {
                @class = IdConstants.SubMenuName,
                @id = subMenu.Id
            });
            var visibleSubMenuItems = subMenu.MenuItems.Select(item => html.MenuItemAsElementDescriptor(item, nakedObject, isEdit)).Where(x => x != null);
            if (visibleSubMenuItems.Any()) {
                return new ElementDescriptor {
                    TagType = tagType,
                    Value = value,
                    Attributes = attributes,
                    Children = visibleSubMenuItems.WrapInCollection("div", new {@class = IdConstants.SubMenuItemsName})
                };
            }
            return null;
        }

        private static ElementDescriptor CustomMenuItemAsDescriptor(HtmlHelper html, CustomMenuItem customItem) {
            return html.ObjectActionAsElementDescriptor(customItem, false);
        }
    }
}