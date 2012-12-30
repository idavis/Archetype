#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace Archetype.MetaObjects
{
    public abstract class DelegatingMetaObject : DynamicMetaObject
    {
        private readonly DynamicMetaObject _BaseMetaObject;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DelegatingMetaObject" /> class.
        /// </summary>
        /// <param name="expression">
        ///     The expression representing this <see cref="DynamicMetaObject" /> during the dynamic binding process.
        /// </param>
        /// <param name="value">
        ///     The runtime value represented by the <see cref="DynamicMetaObject" />.
        /// </param>
        /// <param name="baseMetaObject">
        ///     The <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding against the primary delegating object.
        /// </param>
        protected DelegatingMetaObject( Expression expression,
                                        object value,
                                        DynamicMetaObject baseMetaObject )
                : base( expression, BindingRestrictions.Empty, value )
        {
            _BaseMetaObject = baseMetaObject;
        }

        /// <summary>
        ///     Performs the binding of the dynamic binary operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.BinaryOperationBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="arg">
        ///     An instance of the <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the right hand side of the binary operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindBinaryOperation( BinaryOperationBinder binder, DynamicMetaObject arg )
        {
            return ApplyBinding( meta => meta.BindBinaryOperation( binder, arg ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackBinaryOperation( target, arg, errorSuggestion ) );
        }

        /// <summary>
        ///     Performs the binding of the dynamic conversion operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.ConvertBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindConvert( ConvertBinder binder )
        {
            return ApplyBinding( meta => Convert( binder, meta ), binder.FallbackConvert );
        }

        /// <summary>
        ///     Performs the binding of the dynamic create instance operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.CreateInstanceBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="args">
        ///     An array of <see cref="T:System.Dynamic.DynamicMetaObject" /> instances - arguments to the create instance operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindCreateInstance( CreateInstanceBinder binder, DynamicMetaObject[] args )
        {
            return ApplyBinding( meta => meta.BindCreateInstance( binder, args ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackCreateInstance( target, args, errorSuggestion ) );
        }

        /// <summary>
        ///     Performs the binding of the dynamic delete index operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.DeleteIndexBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="indexes">
        ///     An array of <see cref="T:System.Dynamic.DynamicMetaObject" /> instances - indexes for the delete index operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindDeleteIndex( DeleteIndexBinder binder, DynamicMetaObject[] indexes )
        {
            return ApplyBinding( meta => meta.BindDeleteIndex( binder, indexes ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackDeleteIndex( target, indexes, errorSuggestion ) );
        }

        /// <summary>
        ///     Performs the binding of the dynamic delete member operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.DeleteMemberBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindDeleteMember( DeleteMemberBinder binder )
        {
            return ApplyBinding( meta => meta.BindDeleteMember( binder ), binder.FallbackDeleteMember );
        }

        /// <summary>
        ///     Performs the binding of the dynamic get member operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.GetMemberBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindGetMember( GetMemberBinder binder )
        {
            return ApplyBinding( meta => meta.BindGetMember( binder ), binder.FallbackGetMember );
        }

        /// <summary>
        ///     Performs the binding of the dynamic get index operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.GetIndexBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="indexes">
        ///     An array of <see cref="T:System.Dynamic.DynamicMetaObject" /> instances - indexes for the get index operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindGetIndex( GetIndexBinder binder, DynamicMetaObject[] indexes )
        {
            return ApplyBinding( meta => meta.BindGetIndex( binder, indexes ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackGetIndex( target, indexes, errorSuggestion ) );
        }

        /// <summary>
        ///     Performs the binding of the dynamic invoke member operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.InvokeMemberBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="args">
        ///     An array of <see cref="T:System.Dynamic.DynamicMetaObject" /> instances - arguments to the invoke member operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindInvokeMember( InvokeMemberBinder binder, DynamicMetaObject[] args )
        {
            return ApplyBinding( meta => meta.BindInvokeMember( binder, args ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackInvokeMember( target, args, errorSuggestion ) );
        }

        /// <summary>
        ///     Performs the binding of the dynamic invoke operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.InvokeBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="args">
        ///     An array of <see cref="T:System.Dynamic.DynamicMetaObject" /> instances - arguments to the invoke operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindInvoke( InvokeBinder binder, DynamicMetaObject[] args )
        {
            return ApplyBinding( meta => meta.BindInvoke( binder, args ),
                                 ( target, errorSuggestion ) => binder.FallbackInvoke( target, args, errorSuggestion ) );
        }

        /// <summary>
        ///     Performs the binding of the dynamic set index operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.SetIndexBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="indexes">
        ///     An array of <see cref="T:System.Dynamic.DynamicMetaObject" /> instances - indexes for the set index operation.
        /// </param>
        /// <param name="value">
        ///     The <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the value for the set index operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindSetIndex( SetIndexBinder binder,
                                                        DynamicMetaObject[] indexes,
                                                        DynamicMetaObject value )
        {
            return ApplyBinding( meta => meta.BindSetIndex( binder, indexes, value ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackSetIndex( target, indexes, value, errorSuggestion ) );
        }

        /// <summary>
        ///     Performs the binding of the dynamic set member operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.SetMemberBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <param name="value">
        ///     The <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the value for the set member operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindSetMember( SetMemberBinder binder, DynamicMetaObject value )
        {
            return ApplyBinding( meta => meta.BindSetMember( binder, value ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackSetMember( target, value, errorSuggestion ) );
        }

        /// <summary>
        ///     Performs the binding of the dynamic unary operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.UnaryOperationBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindUnaryOperation( UnaryOperationBinder binder )
        {
            return ApplyBinding( meta => meta.BindUnaryOperation( binder ), binder.FallbackUnaryOperation );
        }

        /// <summary>
        ///     Binds the dynamic operation to the base meta object and attaches the fallback error suggestion if it can be bound.
        /// </summary>
        /// <param name="bindTarget">
        ///     Performs the binding of the dynamic operation if the target dynamic object cannot bind.
        ///     <param>The target of the dynamic operation.</param>
        ///     <returns>
        ///         The <see cref="DynamicMetaObject" /> representing the result of the binding.
        ///     </returns>
        /// </param>
        /// <param name="bindFallback">
        ///     Performs the binding of the dynamic set member operation if the target dynamic object cannot bind.
        ///     <param>The target of the dynamic set member operation.</param>
        ///     <param>The binding result to use if binding fails, or null.</param>
        ///     <returns>
        ///         The <see cref="DynamicMetaObject" /> representing the result of the binding.
        ///     </returns>
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        protected virtual DynamicMetaObject ApplyBinding( Func<DynamicMetaObject, DynamicMetaObject> bindTarget,
                                                          Func<DynamicMetaObject, DynamicMetaObject, DynamicMetaObject>
                                                                  bindFallback )
        {
            DynamicMetaObject errorSuggestion = Resolve( bindTarget );
            if ( BindingHasFailed( errorSuggestion ) )
            {
                return bindTarget( _BaseMetaObject );
            }
            return bindFallback( _BaseMetaObject, errorSuggestion );
        }

        /// <summary>
        ///     Implemented by inheritors, this function should determine the receiver for the binding of the dynamic operation and
        ///     use the <paramref name="bindTarget" /> to perform the binding and return the result of that binding operation.
        /// </summary>
        /// <param name="bindTarget">
        ///     Performs the binding of the dynamic operation if the target dynamic object cannot bind.
        ///     <param>The target of the dynamic operation.</param>
        ///     <returns>
        ///         The <see cref="DynamicMetaObject" /> representing the result of the binding.
        ///     </returns>
        /// </param>
        /// <remarks>
        ///     The <paramref name="bindTarget" /> can be used multiple times to attempt potential binding operations.
        /// </remarks>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        protected abstract DynamicMetaObject Resolve( Func<DynamicMetaObject, DynamicMetaObject> bindTarget );

        /// <summary>
        ///     Performs the binding of the dynamic operation and relaxes the type restrictions for the target of the delegation.
        /// </summary>
        /// <param name="bindTarget">
        ///     Performs the binding of the dynamic operation if the target dynamic object cannot bind.
        ///     <param>The target of the dynamic operation.</param>
        ///     <returns>
        ///         The <see cref="DynamicMetaObject" /> representing the result of the binding.
        ///     </returns>
        /// </param>
        /// <param name="target">The target of the dynamic operation.</param>
        /// <remarks>
        ///     When bindings are evaluated, they are restricted to the type being operated upon. In order to support delegation
        ///     to other types, we have to relax the type restrictions on the <see cref="DynamicMetaObject" />.
        /// </remarks>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        protected virtual DynamicMetaObject CreateBoundDynamicMetaObject(
                Func<DynamicMetaObject, DynamicMetaObject> bindTarget,
                object target )
        {
            DynamicMetaObject metaObject = Create( target, Expression.Constant( target ) );
            DynamicMetaObject boundMetaObject = bindTarget( metaObject );
            DynamicMetaObject result = RelaxTypeRestrictions( boundMetaObject, boundMetaObject.Value );
            return result;
        }

        /// <summary>
        ///     Relaxes the type restrictions for the current <see cref="T:System.Dynamic.DynamicMetaObject" /> based on the new delegation target.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="target">The target of the dynamic operation.</param>
        /// <returns></returns>
        protected virtual DynamicMetaObject RelaxTypeRestrictions( DynamicMetaObject result, object target )
        {
            BindingRestrictions typeRestrictions =
                    GetTypeRestriction().Merge( result.Restrictions );
            var metaObject = new DynamicMetaObject( result.Expression, typeRestrictions, target );
            return metaObject;
        }

        /// <summary>
        ///     Gets the type restrictions for the current target <see cref="Value" /> and <see cref="Expression" />.
        /// </summary>
        /// <returns></returns>
        protected virtual BindingRestrictions GetTypeRestriction()
        {
            if ( Value == null && HasValue )
            {
                return BindingRestrictions.GetInstanceRestriction( Expression, null );
            }
            return BindingRestrictions.GetTypeRestriction( Expression, LimitType );
        }

        protected virtual Expression GetLimitedSelf()
        {
            return AreEquivalent( Expression.Type, LimitType )
                           ? Expression
                           : Expression.Convert( Expression, LimitType );
        }

        protected bool AreEquivalent( Type lhs, Type rhs )
        {
            return lhs == rhs || lhs.IsEquivalentTo( rhs );
        }

        protected static bool TryConvert( ConvertBinder binder, DynamicMetaObject instance, out DynamicMetaObject result )
        {
            if ( instance.HasValue &&
                 instance.RuntimeType.IsValueType )
            {
                result = instance.BindConvert( binder );
                return true;
            }

            if ( binder.Type.IsInterface )
            {
                Expression expression = Convert( instance.Expression, binder.Type );
                result = new DynamicMetaObject( expression, BindingRestrictions.Empty, instance.Value );
                result = result.BindConvert( binder );
                return true;
            }

            if ( typeof (IDynamicMetaObjectProvider).IsAssignableFrom( instance.RuntimeType ) )
            {
                result = instance.BindConvert( binder );
                return true;
            }

            result = null;
            return false;
        }

        protected static DynamicMetaObject Convert( ConvertBinder binder, DynamicMetaObject instance )
        {
            DynamicMetaObject result;
            return TryConvert( binder, instance, out result ) ? result : instance;
        }

        protected static Expression Convert( Expression expression, Type type )
        {
            return expression.Type == type ? expression : Expression.Convert( expression, type );
        }

        protected static bool BindingHasFailed( DynamicMetaObject metaObject )
        {
            return metaObject == null ||
                   metaObject.Expression.NodeType == ExpressionType.Throw;
        }
    }
}