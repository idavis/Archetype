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

namespace Archetype.MetaObjects
{
    public class ModuleMetaObject : DelegatingMetaObject
    {
        private readonly IList<object> _Modules;

        public ModuleMetaObject( Expression expression,
                                 object value,
                                 DynamicMetaObject baseMetaObject,
                                 IList<object> modules )
                : base( expression, value, baseMetaObject )
        {
            _Modules = modules;
        }

        protected override DynamicMetaObject Resolve( Func<DynamicMetaObject, DynamicMetaObject> bindTarget )
        {
            for ( int index = _Modules.Count - 1; index >= 0; index-- )
            {
                object module = _Modules[index];
                DynamicMetaObject metaObject = CreateBoundDynamicMetaObject( bindTarget, module );

                if ( BindingHasFailed( metaObject ) )
                {
                    continue;
                }

                return metaObject;
            }
            return null;
        }
    }
}