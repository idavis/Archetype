#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace Archetype
{
    public interface IPrototypalMetaObjectProvider : IDynamicMetaObjectProvider
    {
        object Prototype { get; }
        IList<object> Prototypes { get; }
        DynamicMetaObject GetBaseMetaObject( Expression parameter );
    }
}