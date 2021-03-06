// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NakedObjects.Services;

namespace NakedObjects.SystemTest.ParentChild {
    namespace ParentChild {
        [TestClass]
        public class TestParentChildPersistence : AbstractSystemTest<ParentChildDbContext> {
            #region Setup/Teardown

            [ClassInitialize]
            public static void ClassInitialize(TestContext tc) {
                InitializeNakedObjectsFramework(new TestParentChildPersistence());
            }

            [ClassCleanup]
            public static void ClassCleanup() {
                CleanupNakedObjectsFramework(new TestParentChildPersistence());
                Database.Delete(ParentChildDbContext.DatabaseName);
            }

            [TestInitialize()]
            public void TestInitialize() {
                InitializeNakedObjectsFrameworkOnce();
                StartTest();
            }

            #endregion

            protected override string[] Namespaces {
                get { return new[] { typeof(Parent).Namespace }; }
            }

            protected override object[] MenuServices {
                get {
                    return new object[] {
                        new SimpleRepository<Parent>(),
                        new SimpleRepository<Parent2>(),
                        new SimpleRepository<Child>()
                    };
                }
            }

            [TestMethod]
            public virtual void CannotSaveParentIfChildHasMandatoryFieldsMissing() {
                var parent = GetTestService("Parents").GetAction("New Instance").InvokeReturnObject();
                var parent0 = parent.GetPropertyByName("Prop0").AssertIsMandatory().AssertIsEmpty();
                parent.AssertCannotBeSaved();
                parent0.SetValue("Foo");
                parent.AssertCannotBeSaved();

                var childProp = parent.GetPropertyByName("Child");
                var child = childProp.ContentAsObject;
                child.AssertIsType(typeof (Child));

                var child0 = child.GetPropertyByName("Prop0").AssertIsMandatory().AssertIsEmpty();
                child.AssertCannotBeSaved();
                child0.SetValue("Bar");
                child.AssertCanBeSaved();

                parent.AssertCanBeSaved();
                parent.Save();
            }
        }

        #region Classes used in tests

        public class ParentChildDbContext : DbContext {
            public const string DatabaseName = "TestParentChild";
            public ParentChildDbContext() : base(DatabaseName) {}

            public DbSet<Parent> Parents { get; set; }
            public DbSet<Parent2> Parent2s { get; set; }
            public DbSet<Child> Children { get; set; }
        }

        public class Parent {

            public IDomainObjectContainer Container { set; protected get; }

            public void Created() {
                Child = Container.NewTransientInstance<Child>();
            }

            [NakedObjectsIgnore]
            public virtual int Id { get; set; }

            public virtual string Prop0 { get; set; }

            public virtual Child Child { get; set; }
        }

        public class Parent2 {
            [NakedObjectsIgnore]
            public virtual int Id { get; set; }

            public virtual string Prop0 { get; set; }

            #region Children (collection)

            private ICollection<Child> myChildren = new List<Child>();

            public virtual ICollection<Child> Children {
                get { return myChildren; }
                set { myChildren = value; }
            }

            public virtual void AddToChildren(Child value) {
                if (!(myChildren.Contains(value))) {
                    myChildren.Add(value);
                }
            }

            public virtual void RemoveFromChildren(Child value) {
                if (myChildren.Contains(value)) {
                    myChildren.Remove(value);
                }
            }

            #endregion
        }

        public class Child {
            [NakedObjectsIgnore]
            public virtual int Id { get; set; }

            public virtual string Prop0 { get; set; }
        }

        #endregion
    }
}