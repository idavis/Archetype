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
    public abstract class DynamicMetaObjectProviderBase : DynamicObject
    {
        // TODO: should this be self?
        protected virtual dynamic _this
        {
            get { return this; }
        }

        /// <summary>
        ///     Provides a <see cref="T:System.Dynamic.DynamicMetaObject" /> that dispatches to the dynamic virtual methods.
        ///     The object can be encapsulated inside another <see cref="T:System.Dynamic.DynamicMetaObject" /> to provide
        ///     custom behavior for individual actions. This method supports the Dynamic Language Runtime infrastructure for
        ///     language implementers and it is not intended to be used directly from your code.
        /// </summary>
        /// <param name="parameter">
        ///     The expression that represents <see cref="T:System.Dynamic.DynamicMetaObject" /> to
        ///     dispatch to the dynamic virtual methods.
        /// </param>
        /// <returns>
        ///     An object of the <see cref="T:System.Dynamic.DynamicMetaObject" /> type.
        /// </returns>
        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            DynamicMetaObject baseMetaObject = GetBaseMetaObject( parameter );
            return baseMetaObject;
        }

        protected virtual DynamicMetaObject GetBaseMetaObject( Expression parameter )
        {
            return base.GetMetaObject( parameter );
        }

        public virtual bool RespondsTo( string name )
        {
            IEnumerable<string> dynamicMembers = GetDynamicMemberNames();
            bool respondsTo =
                    dynamicMembers.Any( item => string.Equals( item, name, StringComparison.OrdinalIgnoreCase ) );
            return respondsTo;
        }

        public virtual IEnumerable<string> GetAllMemberNames( object target )
        {
            if ( target == null )
            {
                return Enumerable.Empty<string>();
            }
            return GetDeclaredMemberNames( target )
                    .Union( GetDynamicMemberNames( target ), StringComparer.OrdinalIgnoreCase );
        }

        public virtual IEnumerable<string> GetDeclaredMemberNames( object target )
        {
            Type targetType = target.GetType();
            var properties = targetType.GetTypeInfo().GetAllProperties().Select(type => type.Name);
            var fields = targetType.GetTypeInfo().GetAllFields().Where(type => !type.IsStatic).Select(t => t.Name);
            return fields.Union(properties, StringComparer.OrdinalIgnoreCase);
        }

        public virtual IEnumerable<string> GetDynamicMemberNames( object target )
        {
            var provider = target as IDynamicMetaObjectProvider;
            if ( provider == null )
            {
                return Enumerable.Empty<string>();
            }
            DynamicMetaObject meta = provider.GetMetaObject( Expression.Constant( target ) );
            return meta.GetDynamicMemberNames();
        }
    }

    public static class TypeInfoAllMemberExtensions
    {
        public static IEnumerable<ConstructorInfo> GetAllConstructors(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredConstructors);

        public static IEnumerable<EventInfo> GetAllEvents(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredEvents);

        public static IEnumerable<FieldInfo> GetAllFields(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredFields);

        public static IEnumerable<MemberInfo> GetAllMembers(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredMembers);

        public static IEnumerable<MethodInfo> GetAllMethods(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredMethods);

        public static IEnumerable<TypeInfo> GetAllNestedTypes(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredNestedTypes);

        public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredProperties);

        private static IEnumerable<T> GetAll<T>(TypeInfo typeInfo, Func<TypeInfo, IEnumerable<T>> accessor)
        {
            while (typeInfo != null)
            {
                foreach (var t in accessor(typeInfo))
                {
                    yield return t;
                }

                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }
        }
    }
}