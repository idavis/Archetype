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
            DynamicMetaObject baseMetaObject = base.GetMetaObject( parameter );

            if ( Modules == null ||
                 Modules.Count == 0 )
            {
                return baseMetaObject;
            }

            return new ModuleMetaObject( parameter, this, baseMetaObject, Modules );
        }

        /// <summary>
        ///     Returns the enumeration of all dynamic member names.
        /// </summary>
        /// <returns>
        ///     A sequence that contains dynamic member names.
        /// </returns>
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