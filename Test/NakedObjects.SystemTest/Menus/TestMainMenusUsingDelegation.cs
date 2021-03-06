﻿using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NakedObjects.Architecture.Configuration;
using NakedObjects.Architecture.Menu;
using NakedObjects.Core.Configuration;
using NakedObjects.Menu;
using NakedObjects.Meta.Menu;
using NakedObjects.Xat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestObjectMenu;

namespace NakedObjects.SystemTest.Menus.Service {

    [TestClass]
    public class TestMainMenusUsingDelegation : AbstractSystemTest<MenusDbContext> {
        #region Setup/Teardown

        [ClassCleanup]
        public static void ClassCleanup() {
            CleanupNakedObjectsFramework(new TestMainMenusUsingDelegation());
            Database.Delete(MenusDbContext.DatabaseName);
        }

        [TestInitialize()]
        public void TestInitialize() {
            InitializeNakedObjectsFrameworkOnce();
            StartTest();
        }

        [TestCleanup()]
        public void TestCleanup() { }

        #endregion

        #region System Config
        protected override object[] SystemServices {
            get {
                return new object[] { 
                    new FooService(), 
                    new ServiceWithSubMenus(),
                    new BarService(), 
                    new QuxService() };
            }
        }

        protected override void RegisterTypes(IUnityContainer container) {
            base.RegisterTypes(container);
            container.RegisterType<IMenuFactory, MenuFactory>();
            container.RegisterInstance<IReflectorConfiguration>(MyReflectorConfig(), (new ContainerControlledLifetimeManager()));
        }

        private IReflectorConfiguration MyReflectorConfig() {
            return new ReflectorConfiguration(
                this.Types ?? new Type[] { },
                this.Services,
                Types.Select(t => t.Namespace).Distinct().ToArray(),
                LocalMainMenus.MainMenus);
        }


        #endregion

        [TestMethod]
        public virtual void TestMainMenus() {
            var menus = AllMainMenus();

            menus[0].AssertNameEquals("Foo Service");
            menus[1].AssertNameEquals("Bars"); //Picks up Named attribute on service
            menus[2].AssertNameEquals("Subs"); //Named attribute overridden in menu construction

            var foo = menus[0];
            foo.AssertItemCountIs(3);
            Assert.AreEqual(3, foo.AllItems().OfType<ITestMenuItem>().Count());

            foo.AllItems()[0].AssertNameEquals("Foo Action0");
            foo.AllItems()[1].AssertNameEquals("Foo Action1");
            foo.AllItems()[2].AssertNameEquals("Foo Action2");
        }
    }

    #region Classes used in test

    public class LocalMainMenus {

        public static IMenu[] MainMenus(IMenuFactory factory) {
            var menuDefs = new Dictionary<Type, Action<IMenu>>();
             menuDefs.Add(typeof(FooService), FooService.Menu);
            menuDefs.Add(typeof(BarService), BarService.Menu);
            menuDefs.Add(typeof(ServiceWithSubMenus), ServiceWithSubMenus.Menu);

            var menus = new List<IMenu>();
            foreach (var menuDef in menuDefs) {
                var menu = factory.NewMenu(menuDef.Key);
                menuDef.Value(menu);
                menus.Add(menu);
            }
            return menus.ToArray();
        }
    }

    public class FooService {

        public static void Menu(IMenu menu) {
            menu.Type = typeof(FooService);
            menu.AddRemainingNativeActions();
        }

        public void FooAction0() { }

        public void FooAction1() { }

        public void FooAction2(string p1, int p2) { }
    }

    [Named("Subs")]
    public class ServiceWithSubMenus {
        public static void Menu(IMenu menu) {
            menu.Type = typeof(ServiceWithSubMenus);
            var sub1 = menu.CreateSubMenu("Sub1");
            sub1.AddAction("Action1");
            sub1.AddAction("Action3");
            var sub2 = menu.CreateSubMenu("Sub2");
            sub2.AddAction("Action2");
            sub2.AddAction("Action0");
        }

        public void Action0() { }

        public void Action1() { }

        public void Action2() { }

        public void Action3() { }
    }

    [Named("Bars")]
    public class BarService {

        public static void Menu(IMenu menu) {
            menu.Type = typeof(BarService);
            menu.AddRemainingNativeActions();
        }

        [MemberOrder(10)]
        public void BarAction0() { }

        [MemberOrder(1)]
        public void BarAction1() { }

        public void BarAction2() { }

        public void BarAction3() { }

    }

    #endregion
}
