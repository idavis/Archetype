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

namespace Archetype.MetaObjects
{
    public class PrototypalMetaObject : DelegatingMetaObject
    {
        private readonly object _Prototype;

        public PrototypalMetaObject( Expression expression,
                                     object value,
                                     DynamicMetaObject baseMetaObject,
                                     object prototype )
                : base( expression, value, baseMetaObject )
        {
            _Prototype = prototype;
        }

        protected override DynamicMetaObject Resolve( Func<DynamicMetaObject, DynamicMetaObject> bindTarget )
        {
            DynamicMetaObject metaObject = CreateBoundDynamicMetaObject( bindTarget, _Prototype );
            return metaObject;
        }
    }
}