#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System.Dynamic;
using System.Linq.Expressions;

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
    }
}