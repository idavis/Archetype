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
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace Archetype
{
    public class PrototypalMetaObject : DynamicMetaObject
    {
        private readonly DynamicMetaObject _baseMetaObject;
        private readonly IPrototypalMetaObjectProvider _prototypalObject;
        private readonly IList<object> _prototypes;

        public PrototypalMetaObject( Expression expression,
                                     IPrototypalMetaObjectProvider value,
                                     IList<object> prototypes )
                : base( expression, BindingRestrictions.Empty, value )
        {
            _prototypalObject = value;
            _prototypes = prototypes;
            _baseMetaObject = CreateBaseMetaObject();
        }

        protected virtual DynamicMetaObject AddTypeRestrictions( DynamicMetaObject result, object value )
        {
            BindingRestrictions typeRestrictions =
                    GetTypeRestriction().Merge( result.Restrictions );
            var metaObject = new DynamicMetaObject( result.Expression, typeRestrictions, value );
            return metaObject;
        }

        protected virtual DynamicMetaObject CreatePrototypeMetaObject( object prototype )
        {
            DynamicMetaObject prototypeMetaObject = Create( prototype, Expression.Constant( prototype ) );
            return prototypeMetaObject;
        }

        protected virtual BindingRestrictions GetTypeRestriction()
        {
            if ( Value == null && HasValue )
            {
                return BindingRestrictions.GetInstanceRestriction( Expression, null );
            }
            return BindingRestrictions.GetTypeRestriction( Expression, LimitType );
        }

        protected virtual DynamicMetaObject CreateBaseMetaObject()
        {
            DynamicMetaObject baseMetaObject = _prototypalObject.GetBaseMetaObject( Expression );
            return baseMetaObject;
        }

        protected Expression GetLimitedSelf()
        {
            return AreEquivalent( Expression.Type, LimitType )
                           ? Expression
                           : Expression.Convert( Expression, LimitType );
        }

        protected bool AreEquivalent( Type t1, Type t2 )
        {
            return t1 == t2 || t1.IsEquivalentTo( t2 );
        }

        public override DynamicMetaObject BindBinaryOperation( BinaryOperationBinder binder, DynamicMetaObject arg )
        {
            return ApplyBinding( meta => meta.BindBinaryOperation( binder, arg ),
                                 ( target, errorSuggestion ) =>
                                 binder.FallbackBinaryOperation( target, arg, errorSuggestion ) );
        }

        public override DynamicMetaObject BindConvert( ConvertBinder binder )
        {
            return ApplyBinding( meta => meta/*meta.BindConvert( binder )*/, binder.FallbackConvert );
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
            DynamicMetaObject errorSuggestion = ResolveModuleChain( bindTarget, bindFallback );
            return bindFallback( _baseMetaObject, errorSuggestion );
        }

        private DynamicMetaObject ResolveModuleChain( Func<DynamicMetaObject, DynamicMetaObject> bindTarget,
                                                      Func<DynamicMetaObject, DynamicMetaObject, DynamicMetaObject> bindFallback )
        {
            DynamicMetaObject errorSuggestion = null;
            for ( int i = _prototypes.Count - 1; i >= 0; i-- )
            {
                object prototype = _prototypes[i];

                DynamicMetaObject prototypeMetaObject = CreatePrototypeMetaObject( prototype );
                DynamicMetaObject newValue = AddTypeRestrictions( bindTarget( prototypeMetaObject ), prototype );
                if (newValue == null || newValue.Expression.NodeType == ExpressionType.Throw)
                {
                    continue;
                }

                errorSuggestion = errorSuggestion == null ? newValue : bindFallback(newValue, errorSuggestion);
            }
            return errorSuggestion;
        }
    }
}
