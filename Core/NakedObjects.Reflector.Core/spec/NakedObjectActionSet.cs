// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facets;
using NakedObjects.Architecture.Interactions;
using NakedObjects.Architecture.Reflect;
using NakedObjects.Architecture.Security;
using NakedObjects.Architecture.Spec;
using NakedObjects.Core.Util;

namespace NakedObjects.Reflector.Spec {
    public class NakedObjectActionSet : INakedObjectAction {
        private readonly INakedObjectAction[] actions;
        private readonly string id;
        private readonly string name;

        public NakedObjectActionSet(string id, string name, INakedObjectAction[] actions) {
            this.id = id;
            this.name = name;
            this.actions = actions;
        }

        public virtual bool OnInstance {
            get { return false; }
        }

        #region INakedObjectAction Members

        public virtual string DebugData {
            get { return ""; }
        }

        public virtual INakedObjectAction[] Actions {
            get { return actions; }
        }

        public virtual string Description {
            get { return ""; }
        }

        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual Type[] FacetTypes {
            get { return new Type[0]; }
        }

        public virtual IIdentifier Identifier {
            get { return null; }
        }

        public virtual string Help {
            get { return ""; }
        }

        public virtual string Id {
            get { return id; }
        }

        public virtual string Name {
            get { return name; }
        }

        public virtual INakedObjectSpecification OnType {
            get { return null; }
        }

        public virtual int ParameterCount {
            get { return 0; }
        }

        public virtual INakedObjectSpecification ReturnType {
            get { return null; }
        }

        public virtual Target Target {
            get { return Target.Default; }
        }

        public bool IsContributedTo(INakedObjectSpecification spec) {
            return false;
        }

        public bool IsFinderMethod {
            get { return false; }
        }

        public virtual NakedObjectActionType ActionType {
            get { return NakedObjectActionType.Set; }
        }

        public virtual bool IsContributedMethod {
            get { return false; }
        }

        public virtual bool PromptForParameters(INakedObject nakedObject) {
            return false;
        }

        /// <summary>
        ///     Always returns <c>null</c>
        /// </summary>
        public virtual INakedObjectSpecification Specification {
            get { return null; }
        }

        public virtual INakedObject Execute(INakedObject target, INakedObject[] parameterSet) {
            throw new UnexpectedCallException();
        }

        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual IFacet GetFacet(Type type) {
            return null;
        }

        public virtual T GetFacet<T>() where T : IFacet {
            return default(T);
        }

        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual IFacet[] GetFacets(IFacetFilter filter) {
            return null;
        }

        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual void AddFacet(IFacet facet) {}

        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual void AddFacet(IMultiTypedFacet facet) {}


        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual void RemoveFacet(IFacet facet) {}


        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual bool ContainsFacet(Type facetType) {
            return false;
        }

        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual bool ContainsFacet<T>() where T : IFacet {
            return false;
        }


        /// <summary>
        ///     Does nothing
        /// </summary>
        public virtual void RemoveFacet(Type facetType) {}

        public virtual INakedObjectActionParameter[] Parameters {
            get { return new INakedObjectActionParameter[0]; }
        }

        public virtual INakedObjectActionParameter[] GetParameters(INakedObjectActionParameterFilter filter) {
            return new INakedObjectActionParameter[0];
        }

        public INakedObject[] RealParameters(INakedObject target, INakedObject[] parameterSet) {
            return new INakedObject[] {};
        }

        public virtual bool HasReturn() {
            return false;
        }

        public virtual IConsent IsParameterSetValid(INakedObject nakedObject, INakedObject[] parameterSet) {
            throw new UnexpectedCallException();
        }

        public virtual IConsent IsUsable(ISession session, INakedObject target) {
            return Allow.Default;
        }

        public bool IsNullable {
            get { return false; }
        }

        public virtual bool IsVisible(ISession session, INakedObject target) {
            return true;
        }

        public virtual INakedObject RealTarget(INakedObject target) {
            return null;
        }

        #endregion

        public virtual INakedObject[] GetDefaultParameterValues(INakedObject target) {
            throw new UnexpectedCallException();
        }

        public virtual INakedObject[][] GetChoices(INakedObject target) {
            throw new UnexpectedCallException();
        }

        public virtual IConsent IsParameterSetValidDeclaratively(INakedObject nakedObject, INakedObject[] parameters) {
            throw new UnexpectedCallException();
        }

        public virtual IConsent IsParameterSetValidImperatively(INakedObject nakedObject, INakedObject[] parameters) {
            throw new UnexpectedCallException();
        }

        public virtual bool IsVisible(InteractionContext ic) {
            return true;
        }
    }

    // Copyright (c) Naked Objects Group Ltd.
}