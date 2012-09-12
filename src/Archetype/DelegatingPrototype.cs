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
    public class DelegatingPrototype : DynamicObject, IPrototypalMetaObjectProvider
    {
        public DelegatingPrototype()
                : this( null )
        {
        }

        public DelegatingPrototype( object prototype )
        {
            Prototype = prototype;
        }

        #region IPrototypalMetaObjectProvider Members

        public virtual object Prototype { get; set; }

        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            if ( Prototype == null )
            {
                return GetBaseMetaObject( parameter );
            }
            return new PrototypalMetaObject( parameter, this, Prototype );
        }

        public virtual DynamicMetaObject GetBaseMetaObject( Expression parameter )
        {
            return base.GetMetaObject( parameter );
        }

        #endregion
    }
}