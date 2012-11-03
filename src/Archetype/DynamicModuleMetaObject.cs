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
    public class DynamicModuleMetaObject : DynamicMetaObject
    {
        private readonly DynamicMetaObject _BaseMetaObject;
        private readonly IList<object> _Modules;

        public DynamicModuleMetaObject( Expression expression,
                                        object value,
                                        DynamicMetaObject baseMetaObject,
                                        IList<object> modules )
                : base( expression, BindingRestrictions.Empty, value )
        {
            _Modules = modules;
            _BaseMetaObject = baseMetaObject;
        }

        protected DynamicMetaObject BaseMetaObject
        {
            get { return _BaseMetaObject; }
        }

        protected IList<object> Modules
        {
            get { return _Modules; }
        }

        protected virtual DynamicMetaObject AddTypeRestrictions( DynamicMetaObject result, object value )
        {
            BindingRestrictions typeRestrictions =
                    GetTypeRestriction().Merge( result.Restrictions );
            var metaObject = new DynamicMetaObject( result.Expression, typeRestrictions, value );
            return metaObject;
        }

        protected virtual DynamicMetaObject CreatePrototypeMetaObject( object module )
        {
            DynamicMetaObject prototypeMetaObject = Create( module, Expression.Constant( module ) );
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
            DynamicMetaObject errorSuggestion = ResolveModuleChain( bindTarget );
            if ( errorSuggestion == null )
            {
                return bindTarget( BaseMetaObject );
            }
            return bindFallback( BaseMetaObject, errorSuggestion );
        }

        private DynamicMetaObject ResolveModuleChain( Func<DynamicMetaObject, DynamicMetaObject> bindTarget )
        {
            for ( int i = Modules.Count - 1; i >= 0; i-- )
            {
                DynamicMetaObject newValue = GetDynamicMetaObjectFromModule( bindTarget, i );

                if ( newValue == null ||
                     newValue.Expression.NodeType == ExpressionType.Throw )
                {
                    continue;
                }

                return newValue;
            }
            return null;
        }

        private DynamicMetaObject GetDynamicMetaObjectFromModule( Func<DynamicMetaObject, DynamicMetaObject> bindTarget,
                                                                  int index )
        {
            object prototype = Modules[index];

            DynamicMetaObject prototypeMetaObject = CreatePrototypeMetaObject( prototype );
            DynamicMetaObject bound = bindTarget( prototypeMetaObject );
            DynamicMetaObject newValue = AddTypeRestrictions( bound, bound.Value );
            return newValue;
        }

        private static bool TryConvert( ConvertBinder binder, DynamicMetaObject instance, out DynamicMetaObject result )
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
            
            if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(instance.RuntimeType))
            {
                result = instance.BindConvert( binder );
                return true;
            }

            result = null;
            return false;
        }

        private static DynamicMetaObject Convert( ConvertBinder binder, DynamicMetaObject instance )
        {
            DynamicMetaObject result;
            return TryConvert( binder, instance, out result ) ? result : instance;
        }

        private static Expression Convert( Expression expression, Type type )
        {
            return expression.Type == type ? expression : Expression.Convert( expression, type );
        }
    }
}