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

namespace Archetype
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

    public class PrototypalObject : DelegatingObject
    {
        private const BindingFlags DefaultBindingFlags =
                BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public;

        public PrototypalObject()
                : this( null )
        {
        }

        public PrototypalObject( object prototype )
                : base( prototype )
        {
        }

        public virtual TryBinaryOperationMissing TryBinaryOperationMissing { get; set; }
        public virtual TryConvertMissing TryConvertMissing { get; set; }
        public virtual TryCreateInstanceMissing TryCreateInstanceMissing { get; set; }
        public virtual TryDeleteIndexMissing TryDeleteIndexMissing { get; set; }
        public virtual TryDeleteMemberMissing TryDeleteMemberMissing { get; set; }
        public virtual TryGetIndexMissing TryGetIndexMissing { get; set; }
        public virtual TryGetMemberMissing TryGetMemberMissing { get; set; }
        public virtual TryInvokeMemberMissing TryInvokeMemberMissing { get; set; }
        public virtual TryInvokeMissing TryInvokeMissing { get; set; }
        public virtual TrySetIndexMissing TrySetIndexMissing { get; set; }
        public virtual TrySetMemberMissing TrySetMemberMissing { get; set; }
        public virtual TryUnaryOperationMissing TryUnaryOperationMissing { get; set; }

        public override bool TryBinaryOperation( BinaryOperationBinder binder, object arg, out object result )
        {
            if ( TryBinaryOperationMissing == null )
            {
                result = null;
                return false;
            }
            return TryBinaryOperationMissing( binder, arg, out result );
        }

        public override bool TryConvert( ConvertBinder binder, out object result )
        {
            if ( TryConvertMissing == null )
            {
                result = null;
                return false;
            }
            return TryConvertMissing( binder, out result );
        }

        public override bool TryCreateInstance( CreateInstanceBinder binder, object[] args, out object result )
        {
            if ( TryCreateInstanceMissing == null )
            {
                result = null;
                return false;
            }
            return TryCreateInstanceMissing( binder, args, out result );
        }

        public override bool TryDeleteIndex( DeleteIndexBinder binder, object[] indexes )
        {
            if ( TryDeleteIndexMissing == null )
            {
                return true;
            }
            return TryDeleteIndexMissing( binder, indexes );
        }

        public override bool TryDeleteMember( DeleteMemberBinder binder )
        {
            if ( TryDeleteMemberMissing == null )
            {
                return true;
            }
            return TryDeleteMemberMissing( binder );
        }

        public override bool TryGetIndex( GetIndexBinder binder, object[] indexes, out object result )
        {
            if ( TryGetIndexMissing == null )
            {
                result = null;
                return false;
            }
            return TryGetIndexMissing( binder, indexes, out result );
        }

        public override bool TryGetMember( GetMemberBinder binder, out object result )
        {
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
            if ( TryInvokeMissing == null )
            {
                result = null;
                return false;
            }
            return TryInvokeMissing( binder, args, out result );
        }

        public override bool TrySetIndex( SetIndexBinder binder, object[] indexes, object value )
        {
            if ( TrySetIndexMissing == null )
            {
                return false;
            }
            return TrySetIndexMissing( binder, indexes, value );
        }

        public override bool TrySetMember( SetMemberBinder binder, object value )
        {
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
            if ( TryUnaryOperationMissing == null )
            {
                result = null;
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
            foreach ( DynamicObject prototype in Modules.OfType<DynamicObject>() )
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
            foreach ( DynamicObject prototype in Modules.OfType<DynamicObject>() )
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
            return Modules.OfType<DynamicObject>().Any( prototype => prototype.TrySetMember( binder, value ) );
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
                 prototypalObject.Modules != null )
            {
                return prototypalObject.Modules.Any( module => RespondsTo( name, module ) );
            }
            return false;
        }
    }
}