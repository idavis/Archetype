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
using Archetype.MetaObjects;

#endregion

namespace Archetype
{
    public class DelegatingObject : DynamicMetaObjectProviderBase
    {
        public DelegatingObject( params object[] modules )
        {
            Modules = new List<object>( modules ?? new object[] { } );
        }

        public IList<object> Modules { get; protected set; }

        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            DynamicMetaObject baseMetaObject = base.GetMetaObject( parameter );

            if ( Modules == null ||
                 Modules.Count == 0 )
            {
                return baseMetaObject;
            }

            return new ModuleMetaObject( parameter, this, baseMetaObject, Modules );
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            if ( Modules == null )
            {
                return base.GetDynamicMemberNames();
            }
            return base.GetDynamicMemberNames()
                       .Union( Modules.SelectMany( GetAllMemberNames ), StringComparer.OrdinalIgnoreCase );
        }
    }
}