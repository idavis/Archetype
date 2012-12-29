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

        public virtual IEnumerable<string> GetDynamicMemberNames( object target )
        {
            if ( target == null )
            {
                return Enumerable.Empty<string>();
            }
            var results = new HashSet<string>( StringComparer.OrdinalIgnoreCase );
            Type type = target.GetType();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            IEnumerable<string> names = type.GetMembers( flags ).Select( member => member.Name );
            foreach ( string name in names )
            {
                results.Add( name );
            }

            var provider = target as IDynamicMetaObjectProvider;
            if ( provider != null )
            {
                DynamicMetaObject meta = provider.GetMetaObject( Expression.Constant( target ) );
                foreach ( string name in meta.GetDynamicMemberNames() )
                {
                    results.Add( name );
                }
            }
            return results.ToList();
        }
    }
}