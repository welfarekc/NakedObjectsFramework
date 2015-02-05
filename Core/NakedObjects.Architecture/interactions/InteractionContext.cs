// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facets;
using NakedObjects.Architecture.Security;

namespace NakedObjects.Architecture.Interactions {
    /// <summary>
    ///     Represents an interaction between the framework and (a <see cref="IFacet" /> of) the domain object.
    /// </summary>
    /// <para>
    ///     Effectively just wraps up a target object, parameters and a <see cref="ISession" />.
    ///     Defining this as a separate interface makes for a more stable API, however.
    /// </para>
    public sealed class InteractionContext {
        private readonly IIdentifier id;
        private readonly InteractionType interactionType;
        private readonly bool programmatic;
        private readonly INakedObject proposedArgument;
        private readonly INakedObject[] proposedArguments;
        private readonly ISession session;
        private readonly INakedObject target;

        private InteractionContext(InteractionType interactionType,
                                   ISession session,
                                   bool programmatic,
                                   INakedObject target,
                                   IIdentifier id,
                                   INakedObject proposedArgument,
                                   INakedObject[] arguments) {
            this.interactionType = interactionType;
            this.programmatic = programmatic;
            this.id = id;
            this.session = session;
            this.target = target;
            this.proposedArgument = proposedArgument;
            proposedArguments = arguments;
        }

        /// <summary>
        ///     The type of interaction
        /// </summary>
        /// <para>
        ///     Used by <see cref="IFacet" />s that apply only in certain conditions.  For
        ///     example, some facets for collections will care only when an object is
        ///     being added to the collection, but won't care when an object is being removed from
        ///     the collection.
        /// </para>
        /// <para>
        ///     Will be set for all interactions.
        /// </para>
        public InteractionType InteractionType {
            get { return interactionType; }
        }

        /// <summary>
        ///     The  user or role <see cref="ISession" /> that is performing this interaction.
        /// </summary>
        /// <para>
        ///     Will be set for all interactions.
        /// </para>
        public ISession Session {
            get { return session; }
        }

        /// <summary>
        ///     How the interaction was initiated
        /// </summary>
        public bool IsProgrammatic {
            get { return programmatic; }
        }

        /// <summary>
        ///     The target object that this interaction is with.
        /// </summary>
        /// <para>
        ///     Will be set for all interactions.
        /// </para>
        public INakedObject Target {
            get { return target; }
        }

        /// <summary>
        ///     The identifier of the object or member that is being identified with.
        /// </summary>
        /// <para>
        ///     If the <see cref="InteractionType" /> type is <see cref="Interactions.InteractionType.ObjectPersist" />,
        ///     will be the identifier of the <see cref="Target" /> object's specification.
        ///     Otherwise will be the identifier of the member.
        /// </para>
        /// <para>
        ///     Will be set for all interactions.
        /// </para>
        public IIdentifier Id {
            get { return id; }
        }

        /// <summary>
        ///     The proposed value for a property, or object being added/removed from a collection.
        /// </summary>
        /// <para>
        ///     Will be set if the <see cref="InteractionType" /> type is
        ///     <see
        ///         cref="Interactions.InteractionType.PropertyParamModify" />
        ///     ,
        ///     <see cref="Interactions.InteractionType.CollectionAddTo" /> or
        ///     <see
        ///         cref="Interactions.InteractionType.CollectionRemoveFrom" />
        ///     ;
        ///     <c>null</c> otherwise.  In the case of the collection interactions, may be safely downcast
        ///     to <see cref="INakedObject" />
        /// </para>
        public INakedObject ProposedArgument {
            get { return proposedArgument; }
        }

        /// <summary>
        ///     The arguments for a proposed action invocation.
        /// </summary>
        /// <para>
        ///     Will be set if the <see cref="InteractionType" /> type is <see cref="Interactions.InteractionType.ActionInvoke" />;
        ///     <c>null</c> otherwise.
        /// </para>
        public INakedObject[] ProposedArguments {
            get { return proposedArguments; }
        }

        private static IIdentifier GetIdentifier(INakedObject target) {
            return target.Specification.Identifier;
        }


        /// <summary>
        ///     Factory method to create an <see cref="InteractionContext" /> to represent
        ///     <see cref="Interactions.InteractionType.ObjectView" /> showing an object
        /// </summary>
        public static InteractionContext ViewObject(ISession session, INakedObject target) {
            return new InteractionContext(InteractionType.ObjectView,
                                          session,
                                          true, target,
                                          GetIdentifier(target),
                                          null,
                                          null);
        }


        /// <summary>
        ///     Factory method to create an an <see cref="InteractionContext" /> to represent
        ///     <see cref="Interactions.InteractionType.ObjectPersist" /> persisting or updating an object.
        /// </summary>
        public static InteractionContext PersistingObject(ISession session,
                                                          bool programmatic,
                                                          INakedObject target) {
            return new InteractionContext(InteractionType.ObjectPersist,
                                          session,
                                          programmatic,
                                          target,
                                          GetIdentifier(target),
                                          null,
                                          null);
        }

        /// <summary>
        ///     Factory method to create an an <see cref="InteractionContext" /> to represent
        ///     <see cref="Interactions.InteractionType.MemberAccess" />  reading a property.
        /// </summary>
        public static InteractionContext AccessMember(ISession session,
                                                      bool programmatic,
                                                      INakedObject target,
                                                      IIdentifier memberIdentifier) {
            return new InteractionContext(InteractionType.MemberAccess,
                                          session,
                                          programmatic,
                                          target,
                                          memberIdentifier,
                                          null,
                                          null);
        }

        /// <summary>
        ///     Factory method to create an an <see cref="InteractionContext" /> to represent
        ///     <see cref="Interactions.InteractionType.PropertyParamModify" />  modifying a property or parameter.
        /// </summary>
        public static InteractionContext ModifyingPropParam(ISession session,
                                                            bool programmatic,
                                                            INakedObject target,
                                                            IIdentifier propertyIdentifier,
                                                            INakedObject proposedArgument) {
            return new InteractionContext(InteractionType.PropertyParamModify,
                                          session,
                                          programmatic,
                                          target,
                                          propertyIdentifier,
                                          proposedArgument,
                                          null);
        }

        /// <summary>
        ///     Factory method to create an an <see cref="InteractionContext" /> to represent
        ///     <see cref="Interactions.InteractionType.CollectionAddTo" />  adding to a collection.
        /// </summary>
        public static InteractionContext AddingToCollection(ISession session,
                                                            bool programmatic,
                                                            INakedObject target,
                                                            IIdentifier collectionIdentifier,
                                                            INakedObject proposedArgument) {
            return new InteractionContext(InteractionType.CollectionAddTo,
                                          session,
                                          programmatic,
                                          target,
                                          collectionIdentifier,
                                          proposedArgument,
                                          null);
        }

        /// <summary>
        ///     Factory method to create an an <see cref="InteractionContext" /> to represent
        ///     <see cref="Interactions.InteractionType.CollectionRemoveFrom" />  removing from a collection.
        /// </summary>
        public static InteractionContext RemovingFromCollection(ISession session,
                                                                bool programmatic,
                                                                INakedObject target,
                                                                IIdentifier collectionIdentifier,
                                                                INakedObject proposedArgument) {
            return new InteractionContext(InteractionType.CollectionRemoveFrom,
                                          session,
                                          programmatic,
                                          target,
                                          collectionIdentifier,
                                          proposedArgument,
                                          null);
        }

        /// <summary>
        ///     Factory method to create an an <see cref="InteractionContext" /> to represent
        ///     <see cref="Interactions.InteractionType.ActionInvoke" />  invoking an action.
        /// </summary>
        public static InteractionContext InvokingAction(ISession session,
                                                        bool programmatic,
                                                        INakedObject target,
                                                        IIdentifier actionIdentifier,
                                                        INakedObject[] arguments) {
            return new InteractionContext(InteractionType.ActionInvoke,
                                          session,
                                          programmatic,
                                          target,
                                          actionIdentifier,
                                          null,
                                          arguments);
        }

        /// <summary>
        ///     Convenience to allow implementors of <see cref="IValidatingInteractionAdvisor" /> etc to determine
        ///     if the interaction's type applies.
        /// </summary>
        public bool TypeEquals(InteractionType other) {
            return InteractionType.Equals(other);
        }
    }


    // Copyright (c) Naked Objects Group Ltd.
}