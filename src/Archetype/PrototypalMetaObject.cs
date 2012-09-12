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

namespace Archetype
{
    public class PrototypalMetaObject : DynamicMetaObject
    {
        private readonly DynamicMetaObject _baseMetaObject;
        private readonly DynamicMetaObject _metaObject;
        private readonly PrototypalObject _prototypalObject;
        private readonly object _prototype;

        public PrototypalMetaObject( Expression expression,
                                     PrototypalObject value,
                                     object prototype )
                : base( expression, BindingRestrictions.Empty, value )
        {
            _prototypalObject = value;
            _prototype = prototype;
            _metaObject = CreatePrototypeMetaObject();
            _baseMetaObject = CreateBaseMetaObject();
        }

        protected virtual DynamicMetaObject AddTypeRestrictions( DynamicMetaObject result )
        {
            BindingRestrictions typeRestrictions =
                    GetTypeRestriction().Merge( result.Restrictions );
            var metaObject = new DynamicMetaObject( result.Expression, typeRestrictions, _metaObject.Value );
            return metaObject;
        }

        protected virtual DynamicMetaObject CreatePrototypeMetaObject()
        {
            Expression castExpression = GetLimitedSelf();
            MemberExpression memberExpression = Expression.Property( castExpression, "Prototype" );
            DynamicMetaObject prototypeMetaObject = Create( _prototype, memberExpression );
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
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindBinaryOperation( binder, arg ) );
            return binder.FallbackBinaryOperation( _baseMetaObject, arg, errorSuggestion );
        }

        public override DynamicMetaObject BindConvert( ConvertBinder binder )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindConvert( binder ) );
            return binder.FallbackConvert( _baseMetaObject, errorSuggestion );
        }

        public override DynamicMetaObject BindCreateInstance( CreateInstanceBinder binder, DynamicMetaObject[] args )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindCreateInstance( binder, args ) );
            return binder.FallbackCreateInstance( _baseMetaObject, args, errorSuggestion );
        }

        public override DynamicMetaObject BindDeleteIndex( DeleteIndexBinder binder, DynamicMetaObject[] indexes )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindDeleteIndex( binder, indexes ) );
            return binder.FallbackDeleteIndex( _baseMetaObject, indexes, errorSuggestion );
        }

        public override DynamicMetaObject BindDeleteMember( DeleteMemberBinder binder )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindDeleteMember( binder ) );
            return binder.FallbackDeleteMember( _baseMetaObject, errorSuggestion );
        }

        public override DynamicMetaObject BindGetMember( GetMemberBinder binder )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindGetMember( binder ) );
            return binder.FallbackGetMember( _baseMetaObject, errorSuggestion );
        }

        public override DynamicMetaObject BindGetIndex( GetIndexBinder binder, DynamicMetaObject[] indexes )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindGetIndex( binder, indexes ) );
            return binder.FallbackGetIndex( _baseMetaObject, indexes, errorSuggestion );
        }

        public override DynamicMetaObject BindInvokeMember( InvokeMemberBinder binder, DynamicMetaObject[] args )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindInvokeMember( binder, args ) );
            return binder.FallbackInvokeMember( _baseMetaObject, args, errorSuggestion );
        }

        public override DynamicMetaObject BindInvoke( InvokeBinder binder, DynamicMetaObject[] args )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindInvoke( binder, args ) );
            return binder.FallbackInvoke( _baseMetaObject, args, errorSuggestion );
        }

        public override DynamicMetaObject BindSetIndex( SetIndexBinder binder,
                                                        DynamicMetaObject[] indexes,
                                                        DynamicMetaObject value )
        {
            DynamicMetaObject errorSuggestion =
                    AddTypeRestrictions( _metaObject.BindSetIndex( binder, indexes, value ) );
            return binder.FallbackSetIndex( _baseMetaObject, indexes, errorSuggestion );
        }

        public override DynamicMetaObject BindSetMember( SetMemberBinder binder, DynamicMetaObject value )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindSetMember( binder, value ) );
            return binder.FallbackSetMember( _baseMetaObject, value, errorSuggestion );
        }

        public override DynamicMetaObject BindUnaryOperation( UnaryOperationBinder binder )
        {
            DynamicMetaObject errorSuggestion = AddTypeRestrictions( _metaObject.BindUnaryOperation( binder ) );
            return binder.FallbackUnaryOperation( _baseMetaObject, errorSuggestion );
        }
    }
}