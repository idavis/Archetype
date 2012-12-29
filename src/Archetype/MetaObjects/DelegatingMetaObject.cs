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

        protected DelegatingMetaObject( Expression expression,
                                        object value,
                                        DynamicMetaObject baseMetaObject )
                : base( expression, BindingRestrictions.Empty, value )
        {
            _BaseMetaObject = baseMetaObject;
        }

        protected virtual DynamicMetaObject AddTypeRestrictions( DynamicMetaObject result, object value )
        {
            BindingRestrictions typeRestrictions =
                    GetTypeRestriction().Merge( result.Restrictions );
            var metaObject = new DynamicMetaObject( result.Expression, typeRestrictions, value );
            return metaObject;
        }

        protected virtual DynamicMetaObject CreateMetaObject( object module )
        {
            DynamicMetaObject metaObject = Create( module, Expression.Constant( module ) );
            return metaObject;
        }

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

        public override DynamicMetaObject BindBinaryOperation( BinaryOperationBinder binder, DynamicMetaObject arg )
        {
            return ApplyBinding( meta => meta.BindBinaryOperation( binder, arg ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackBinaryOperation( target, arg, errorSuggestion ) );
        }

        public override DynamicMetaObject BindConvert( ConvertBinder binder )
        {
            return ApplyBinding( meta => Convert( binder, meta ), binder.FallbackConvert );
        }

        public override DynamicMetaObject BindCreateInstance( CreateInstanceBinder binder, DynamicMetaObject[] args )
        {
            return ApplyBinding( meta => meta.BindCreateInstance( binder, args ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackCreateInstance( target, args, errorSuggestion ) );
        }

        public override DynamicMetaObject BindDeleteIndex( DeleteIndexBinder binder, DynamicMetaObject[] indexes )
        {
            return ApplyBinding( meta => meta.BindDeleteIndex( binder, indexes ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackDeleteIndex( target, indexes, errorSuggestion ) );
        }

        public override DynamicMetaObject BindDeleteMember( DeleteMemberBinder binder )
        {
            return ApplyBinding( meta => meta.BindDeleteMember( binder ), binder.FallbackDeleteMember );
        }

        public override DynamicMetaObject BindGetMember( GetMemberBinder binder )
        {
            return ApplyBinding( meta => meta.BindGetMember( binder ), binder.FallbackGetMember );
        }

        public override DynamicMetaObject BindGetIndex( GetIndexBinder binder, DynamicMetaObject[] indexes )
        {
            return ApplyBinding( meta => meta.BindGetIndex( binder, indexes ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackGetIndex( target, indexes, errorSuggestion ) );
        }

        public override DynamicMetaObject BindInvokeMember( InvokeMemberBinder binder, DynamicMetaObject[] args )
        {
            return ApplyBinding( meta => meta.BindInvokeMember( binder, args ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackInvokeMember( target, args, errorSuggestion ) );
        }

        public override DynamicMetaObject BindInvoke( InvokeBinder binder, DynamicMetaObject[] args )
        {
            return ApplyBinding( meta => meta.BindInvoke( binder, args ),
                                 ( target, errorSuggestion ) => binder.FallbackInvoke( target, args, errorSuggestion ) );
        }

        public override DynamicMetaObject BindSetIndex( SetIndexBinder binder,
                                                        DynamicMetaObject[] indexes,
                                                        DynamicMetaObject value )
        {
            return ApplyBinding( meta => meta.BindSetIndex( binder, indexes, value ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackSetIndex( target, indexes, value, errorSuggestion ) );
        }

        public override DynamicMetaObject BindSetMember( SetMemberBinder binder, DynamicMetaObject value )
        {
            return ApplyBinding( meta => meta.BindSetMember( binder, value ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackSetMember( target, value, errorSuggestion ) );
        }

        public override DynamicMetaObject BindUnaryOperation( UnaryOperationBinder binder )
        {
            return ApplyBinding( meta => meta.BindUnaryOperation( binder ), binder.FallbackUnaryOperation );
        }

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

        protected abstract DynamicMetaObject Resolve( Func<DynamicMetaObject, DynamicMetaObject> bindTarget );

        protected virtual DynamicMetaObject CreateBoundDynamicMetaObject(
                Func<DynamicMetaObject, DynamicMetaObject> bindTarget,
                object value )
        {
            DynamicMetaObject metaObject = CreateMetaObject( value );
            DynamicMetaObject boundMetaObject = bindTarget( metaObject );
            DynamicMetaObject result = AddTypeRestrictions( boundMetaObject, boundMetaObject.Value );
            return result;
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
                result = new DynamicMetaObject( Convert( instance.Expression, binder.Type ),
                                                BindingRestrictions.Empty,
                                                instance.Value );
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