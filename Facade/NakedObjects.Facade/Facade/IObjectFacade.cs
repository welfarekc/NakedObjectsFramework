// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using System.Reflection;
using NakedObjects.Facade.Contexts;

namespace NakedObjects.Facade {
    public interface IObjectFacade : IFacadeHolder {
        ITypeFacade Specification { get; }
        ITypeFacade ElementSpecification { get; }
        object Object { get; }
        IOidFacade Oid { get; }
        IVersionFacade Version { get; }
        IActionFacade MementoAction { get; }
        string EnumIntegralValue { get; }
        bool IsPaged { get; }
        bool IsCollectionMemento { get; }
        bool IsTransient { get; }
        bool IsDestroyed { get; }
        bool IsUserPersistable { get; }
        bool IsNotPersistent { get; }
        string TitleString { get; }
        string InvariantString { get; }
        bool IsViewModelEditView { get; }
        bool IsViewModel { get; }
        IDictionary<string, object> ExtensionData { get; }
        IEnumerable<IObjectFacade> ToEnumerable();
        PropertyInfo[] GetKeys();
        IObjectFacade Page(int page, int size);
        IObjectFacade Select(object[] selection, bool forceEnumerable);
        int Count();
        AttachmentContextFacade GetAttachment();
        object[] GetSelected();
        void Resolve();
        void SetIsNotQueryableState(bool state);
    }
}