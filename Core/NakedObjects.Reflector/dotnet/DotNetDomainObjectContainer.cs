// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facets.Objects.Validation;
using NakedObjects.Architecture.Resolve;
using NakedObjects.Architecture.Spec;
using NakedObjects.Services;
using NakedObjects.UtilInternal;

namespace NakedObjects.Reflector.DotNet {
    public class DotNetDomainObjectContainer : IDomainObjectContainer, IInternalAccess {
        private readonly INakedObjectsFramework framework;

        public DotNetDomainObjectContainer(INakedObjectsFramework framework) {
            this.framework = framework;
        }

        #region IDomainObjectContainer Members

        public IQueryable<T> Instances<T>() where T : class {
            return framework.LifecycleManager.Instances<T>();
        }

        public IQueryable Instances(Type type) {
            return framework.LifecycleManager.Instances(type);
        }

        public void DisposeInstance(object persistentObject) {
            if (persistentObject == null) {
                throw new ArgumentException(Resources.NakedObjects.DisposeReferenceError);
            }
            INakedObject adapter = framework.LifecycleManager.GetAdapterFor(persistentObject);
            if (!IsPersistent(persistentObject)) {
                throw new DisposeFailedException(string.Format(Resources.NakedObjects.NotPersistentMessage, adapter));
            }
            framework.UpdateNotifier.AddDisposedObject(adapter);
            framework.LifecycleManager.DestroyObject(adapter);
        }

        public IPrincipal Principal {
            get { return framework.Session.Principal; }
        }

        public void InformUser(string message) {
            framework.MessageBroker.AddMessage(message);
        }

        public bool IsPersistent(object obj) {
            return !AdapterFor(obj).Oid.IsTransient;
        }

        public void Persist<T>(ref T transientObject) {
            INakedObject adapter = framework.LifecycleManager.GetAdapterFor(transientObject);
            if (IsPersistent(transientObject)) {
                throw new PersistFailedException(string.Format(Resources.NakedObjects.AlreadyPersistentMessage, adapter));
            }
            Validate(adapter);
            framework.LifecycleManager.MakePersistent(adapter);
            transientObject = adapter.GetDomainObject<T>();
        }

        public T NewTransientInstance<T>() where T : new() {
            return (T) NewTransientInstance(typeof (T));
        }

        public T NewViewModel<T>() where T : IViewModel, new() {
            return (T) NewViewModel(typeof (T));
        }

        public IViewModel NewViewModel(Type type) {
            INakedObjectSpecification spec = framework.Reflector.LoadSpecification(type);
            if (spec.IsViewModel) {
                return framework.LifecycleManager.CreateViewModel(spec).GetDomainObject<IViewModel>();
            }
            return null;
        }

        public object NewTransientInstance(Type type) {
            INakedObjectSpecification spec = framework.Reflector.LoadSpecification(type);
            return framework.LifecycleManager.CreateInstance(spec).Object;
        }

        public void ObjectChanged(object obj) {
            if (obj != null) {
                INakedObject adapter = AdapterFor(obj);
                Validate(adapter);
                framework.LifecycleManager.ObjectChanged(adapter);
            }
        }

        public void RaiseError(string message) {
            throw new DomainException(message);
        }

        public void Refresh(object obj) {
            INakedObject nakedObject = AdapterFor(obj);
            framework.LifecycleManager.Refresh(nakedObject);
            ObjectChanged(obj);
        }

        public void Resolve(object parent) {
            INakedObject adapter = AdapterFor(parent);
            if (adapter.ResolveState.IsResolvable()) {
                framework.LifecycleManager.ResolveImmediately(adapter);
            }
        }

        public void Resolve(object parent, object field) {
            if (field == null) {
                Resolve(parent);
            }
        }

        public void WarnUser(string message) {
            framework.MessageBroker.AddWarning(message);
        }

        public void AbortCurrentTransaction() {
            framework.LifecycleManager.UserAbortTransaction();
        }

        #endregion

        #region IInternalAccess Members

        public PropertyInfo[] GetKeys(Type type) {
            return framework.LifecycleManager.GetKeys(type);
        }

        public object FindByKeys(Type type, object[] keys) {
            return framework.LifecycleManager.FindByKeys(type, keys).GetDomainObject();
        }

        #endregion

        private void Validate(INakedObject adapter) {
            if (adapter.Specification.ContainsFacet<IValidateProgrammaticUpdatesFacet>()) {
                string state = adapter.ValidToPersist();
                if (state != null) {
                    throw new PersistFailedException(string.Format(Resources.NakedObjects.PersistStateError, adapter.Specification.ShortName, adapter.TitleString(), state));
                }
            }
        }

        private INakedObject AdapterFor(object obj) {
            return framework.LifecycleManager.CreateAdapter(obj, null, null);
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}