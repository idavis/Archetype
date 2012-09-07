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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Prototype.Ps
{
    public delegate bool TryBinaryOperationMissing( BinaryOperationBinder binder, object arg, out object result );

    public delegate bool TryConvertMissing( ConvertBinder binder, out object result );

    public delegate bool TryCreateInstanceMissing( CreateInstanceBinder binder, object[] args, out object result );

    public delegate bool TryDeleteIndexMissing( DeleteIndexBinder binder, object[] indexes );

    public delegate bool TryDeleteMemberMissing( DeleteMemberBinder binder );

    public delegate bool TryGetIndexMissing( GetIndexBinder binder, object[] indexes, out object result );

    public delegate bool TryGetMemberMissing( GetMemberBinder binder, out object result );

    public delegate bool TryInvokeMemberMissing( InvokeMemberBinder binder, object[] args, out object result );

    public delegate bool TryInvokeMissing( InvokeBinder binder, object[] args, out object result );

    public delegate bool TrySetIndexMissing( SetIndexBinder binder, object[] indexes, object value );

    public delegate bool TrySetMemberMissing( SetMemberBinder binder, object value );

    public delegate bool TryUnaryOperationMissing( UnaryOperationBinder binder, out object result );

    public class PrototypalObject : DynamicObject
    {
        private const BindingFlags DefaultBindingFlags =
                BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public;

        public PrototypalObject()
                : this( null )
        {
        }

        public PrototypalObject( IDynamicMetaObjectProvider prototype )
        {
            Prototype = prototype;
        }

        public IDynamicMetaObjectProvider Prototype { get; protected set; }
        public TryBinaryOperationMissing TryBinaryOperationMissing { get; set; }
        public TryConvertMissing TryConvertMissing { get; set; }
        public TryCreateInstanceMissing TryCreateInstanceMissing { get; set; }
        public TryDeleteIndexMissing TryDeleteIndexMissing { get; set; }
        public TryDeleteMemberMissing TryDeleteMemberMissing { get; set; }
        public TryGetIndexMissing TryGetIndexMissing { get; set; }
        public TryGetMemberMissing TryGetMemberMissing { get; set; }
        public TryInvokeMemberMissing TryInvokeMemberMissing { get; set; }
        public TryInvokeMissing TryInvokeMissing { get; set; }
        public TrySetIndexMissing TrySetIndexMissing { get; set; }
        public TrySetMemberMissing TrySetMemberMissing { get; set; }
        public TryUnaryOperationMissing TryUnaryOperationMissing { get; set; }

        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            if ( Prototype == null )
            {
                return GetBaseMetaObject( parameter );
            }
            return new PrototypalMetaObject( parameter, this, Prototype );
        }

        public virtual DynamicMetaObject GetBaseMetaObject( Expression parameter )
        {
            return base.GetMetaObject( parameter );
        }

        public override bool TryBinaryOperation( BinaryOperationBinder binder, object arg, out object result )
        {
            if ( base.TryBinaryOperation( binder, arg, out result ) )
            {
                return true;
            }

            if ( TrySetMemberMissing == null )
            {
                return false;
            }
            return TryBinaryOperationMissing( binder, arg, out result );
        }

        public override bool TryConvert( ConvertBinder binder, out object result )
        {
            if ( base.TryConvert( binder, out result ) )
            {
                return true;
            }

            if ( TryConvertMissing == null )
            {
                return false;
            }
            return TryConvertMissing( binder, out result );
        }

        public override bool TryCreateInstance( CreateInstanceBinder binder, object[] args, out object result )
        {
            if ( base.TryCreateInstance( binder, args, out result ) )
            {
                return true;
            }

            if ( TryCreateInstanceMissing == null )
            {
                return false;
            }
            return TryCreateInstanceMissing( binder, args, out result );
        }

        public override bool TryDeleteIndex( DeleteIndexBinder binder, object[] indexes )
        {
            if ( base.TryDeleteIndex( binder, indexes ) )
            {
                return true;
            }

            if ( TryDeleteIndexMissing == null )
            {
                return true;
            }
            return TryDeleteIndexMissing( binder, indexes );
        }

        public override bool TryDeleteMember( DeleteMemberBinder binder )
        {
            if ( base.TryDeleteMember( binder ) )
            {
                return true;
            }

            if ( TryDeleteMemberMissing == null )
            {
                return true;
            }
            return TryDeleteMemberMissing( binder );
        }

        public override bool TryGetIndex( GetIndexBinder binder, object[] indexes, out object result )
        {
            if ( base.TryGetIndex( binder, indexes, out result ) )
            {
                return true;
            }

            if ( TryGetIndexMissing == null )
            {
                return false;
            }
            return TryGetIndexMissing( binder, indexes, out result );
        }

        public override bool TryGetMember( GetMemberBinder binder, out object result )
        {
            if ( base.TryGetMember( binder, out result ) )
            {
                return true;
            }
            if ( TryGetStaticMember( binder, out result ) )
            {
                return true;
            }
            if ( TryGetMemberMissing == null )
            {
                return false;
            }
            return TryGetMemberMissing( binder, out result );
        }

        public override bool TryInvokeMember( InvokeMemberBinder binder, object[] args, out object result )
        {
            if ( base.TryInvokeMember( binder, args, out result ) )
            {
                return true;
            }
            if ( TryInvokeStaticMember( binder, args, out result ) )
            {
                return true;
            }
            if ( TryInvokeMemberMissing == null )
            {
                return false;
            }
            return TryInvokeMemberMissing( binder, args, out result );
        }

        public override bool TryInvoke( InvokeBinder binder, object[] args, out object result )
        {
            if ( base.TryInvoke( binder, args, out result ) )
            {
                return true;
            }

            if ( TryInvokeMissing == null )
            {
                return false;
            }
            return TryInvokeMissing( binder, args, out result );
        }

        public override bool TrySetIndex( SetIndexBinder binder, object[] indexes, object value )
        {
            if ( base.TrySetIndex( binder, indexes, value ) )
            {
                return true;
            }

            if ( TrySetIndexMissing == null )
            {
                return false;
            }
            return TrySetIndexMissing( binder, indexes, value );
        }

        public override bool TrySetMember( SetMemberBinder binder, object value )
        {
            if ( base.TrySetMember( binder, value ) )
            {
                return true;
            }
            if ( TrySetStaticMember( binder, value ) )
            {
                return true;
            }
            if ( TrySetMemberMissing == null )
            {
                return false;
            }
            return TrySetMemberMissing( binder, value );
        }

        public override bool TryUnaryOperation( UnaryOperationBinder binder, out object result )
        {
            if ( base.TryUnaryOperation( binder, out result ) )
            {
                return true;
            }
            if ( TryUnaryOperationMissing == null )
            {
                return false;
            }
            return TryUnaryOperationMissing( binder, out result );
        }

        protected virtual bool TryInvokeStaticMember( InvokeMemberBinder binder, object[] args, out object result )
        {
            MethodInfo method = GetType().GetMethod( binder.Name, DefaultBindingFlags );
            if ( method != null )
            {
                result = method.Invoke( null, args );
                return true;
            }
            var prototype = Prototype as DynamicObject;
            if ( prototype != null )
            {
                if ( prototype.TryInvokeMember( binder, args, out result ) )
                {
                    return true;
                }
            }
            result = null;
            return false;
        }

        protected virtual bool TryGetStaticMember( GetMemberBinder binder, out object result )
        {
            PropertyInfo property = GetType().GetProperty( binder.Name, DefaultBindingFlags );
            if ( property != null )
            {
                result = property.GetValue( null, null );
                return true;
            }
            FieldInfo field = GetType().GetField( binder.Name, DefaultBindingFlags );
            if ( field != null )
            {
                result = field.GetValue( null );
                return true;
            }
            var prototype = Prototype as DynamicObject;
            if ( prototype != null )
            {
                if ( prototype.TryGetMember( binder, out result ) )
                {
                    return true;
                }
            }
            result = null;
            return false;
        }

        protected virtual bool TrySetStaticMember( SetMemberBinder binder, object value )
        {
            PropertyInfo property = GetType().GetProperty( binder.Name, DefaultBindingFlags );
            if ( property != null )
            {
                property.SetValue( null, value, null );
                return true;
            }
            FieldInfo field = GetType().GetField( binder.Name, DefaultBindingFlags );
            if ( field != null )
            {
                field.SetValue( null, value );
                return true;
            }
            var prototype = Prototype as DynamicObject;
            if ( prototype != null )
            {
                if ( prototype.TrySetMember( binder, value ) )
                {
                    return true;
                }
            }
            return false;
        }

        public static PrototypalObject AsPrototypalObject( IDynamicMetaObjectProvider prototype )
        {
            if ( prototype == null )
            {
                return new PrototypalObject();
            }
            return prototype as PrototypalObject ?? new PrototypalObject( prototype );
        }

        public virtual bool RespondsTo( string name )
        {
            return RespondsTo( name, this );
        }

        public virtual bool RespondsTo( string name, object target )
        {
            var provider = target as IDynamicMetaObjectProvider;
            IEnumerable<string> dynamicMembers = new string[0];
            if ( provider != null )
            {
                DynamicMetaObject meta = provider.GetMetaObject( Expression.Constant( target ) );
                dynamicMembers = meta.GetDynamicMemberNames();
            }

            Type type = target.GetType();
            IEnumerable<string> members = type.GetMembers().Select( member => member.Name );
            members = members.Union( dynamicMembers );

            bool respondsTo = members.Any( item => String.Equals( item, name, StringComparison.OrdinalIgnoreCase ) );
            if ( respondsTo )
            {
                return true;
            }
            var prototypalObject = target as PrototypalObject;
            if ( prototypalObject != null &&
                 prototypalObject.Prototype != null )
            {
                return RespondsTo( name, prototypalObject.Prototype );
            }
            return false;
        }

        #region Nested type: PrototypalMetaObject

        public class PrototypalMetaObject : DynamicMetaObject
        {
            private readonly DynamicMetaObject _baseMetaObject;
            private readonly DynamicMetaObject _metaObject;
            private readonly PrototypalObject _prototypalObject;
            private readonly IDynamicMetaObjectProvider _prototype;

            public PrototypalMetaObject( Expression expression,
                                         PrototypalObject value,
                                         IDynamicMetaObjectProvider prototype )
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
                DynamicMetaObject prototypeMetaObject = _prototype.GetMetaObject( memberExpression );
                return prototypeMetaObject;
            }

            internal BindingRestrictions GetTypeRestriction()
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

            private Expression GetLimitedSelf()
            {
                return AreEquivalent( Expression.Type, LimitType )
                               ? Expression
                               : Expression.Convert( Expression, LimitType );
            }

            private bool AreEquivalent( Type t1, Type t2 )
            {
                return t1 == t2 || t1.IsEquivalentTo( t2 );
            }

            public override DynamicMetaObject BindBinaryOperation( BinaryOperationBinder binder, DynamicMetaObject arg )
            {
                return binder.FallbackBinaryOperation( _baseMetaObject,
                                                       arg,
                                                       AddTypeRestrictions( _metaObject.BindBinaryOperation( binder, arg ) ) );
            }

            public override DynamicMetaObject BindConvert( ConvertBinder binder )
            {
                return binder.FallbackConvert( _baseMetaObject, AddTypeRestrictions( _metaObject.BindConvert( binder ) ) );
            }

            public override DynamicMetaObject BindCreateInstance( CreateInstanceBinder binder, DynamicMetaObject[] args )
            {
                return binder.FallbackCreateInstance( _baseMetaObject,
                                                      args,
                                                      AddTypeRestrictions( _metaObject.BindCreateInstance( binder, args ) ) );
            }

            public override DynamicMetaObject BindDeleteIndex( DeleteIndexBinder binder, DynamicMetaObject[] indexes )
            {
                return binder.FallbackDeleteIndex( _baseMetaObject,
                                                   indexes,
                                                   AddTypeRestrictions( _metaObject.BindDeleteIndex( binder, indexes ) ) );
            }

            public override DynamicMetaObject BindDeleteMember( DeleteMemberBinder binder )
            {
                return binder.FallbackDeleteMember( _baseMetaObject,
                                                    AddTypeRestrictions( _metaObject.BindDeleteMember( binder ) ) );
            }

            public override DynamicMetaObject BindGetMember( GetMemberBinder binder )
            {
                return binder.FallbackGetMember( _baseMetaObject,
                                                 AddTypeRestrictions( _metaObject.BindGetMember( binder ) ) );
            }

            public override DynamicMetaObject BindGetIndex( GetIndexBinder binder, DynamicMetaObject[] indexes )
            {
                return binder.FallbackGetIndex( _baseMetaObject,
                                                indexes,
                                                AddTypeRestrictions( _metaObject.BindGetIndex( binder, indexes ) ) );
            }

            public override DynamicMetaObject BindInvokeMember( InvokeMemberBinder binder, DynamicMetaObject[] args )
            {
                return binder.FallbackInvokeMember( _baseMetaObject,
                                                    args,
                                                    AddTypeRestrictions( _metaObject.BindInvokeMember( binder, args ) ) );
            }

            public override DynamicMetaObject BindInvoke( InvokeBinder binder, DynamicMetaObject[] args )
            {
                return binder.FallbackInvoke( _baseMetaObject,
                                              args,
                                              AddTypeRestrictions( _metaObject.BindInvoke( binder, args ) ) );
            }

            public override DynamicMetaObject BindSetIndex( SetIndexBinder binder,
                                                            DynamicMetaObject[] indexes,
                                                            DynamicMetaObject value )
            {
                return binder.FallbackSetIndex( _baseMetaObject,
                                                indexes,
                                                AddTypeRestrictions( _metaObject.BindSetIndex( binder, indexes, value ) ) );
            }

            public override DynamicMetaObject BindSetMember( SetMemberBinder binder, DynamicMetaObject value )
            {
                DynamicMetaObject prototypeMetaObject = AddTypeRestrictions( _metaObject.BindSetMember( binder, value ) );
                return binder.FallbackSetMember( _baseMetaObject, value, prototypeMetaObject );
            }

            public override DynamicMetaObject BindUnaryOperation( UnaryOperationBinder binder )
            {
                return binder.FallbackUnaryOperation( _baseMetaObject,
                                                      AddTypeRestrictions( _metaObject.BindUnaryOperation( binder ) ) );
            }
        }

        #endregion
    }
}