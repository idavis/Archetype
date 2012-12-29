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
    public class PrototypalObject<T> : DynamicMetaObjectProviderBase
    {
        public PrototypalObject( T prototype )
        {
            Prototype = prototype;
        }

        public T Prototype { get; set; }

        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            DynamicMetaObject baseMetaObject = base.GetMetaObject( parameter );

            if ( ReferenceEquals( Prototype, null ) )
            {
                return baseMetaObject;
            }

            return new PrototypalMetaObject( parameter, this, baseMetaObject, Prototype );
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return base.GetDynamicMemberNames()
                       .Union( GetDynamicMemberNames( Prototype ), StringComparer.OrdinalIgnoreCase );
        }
    }
}