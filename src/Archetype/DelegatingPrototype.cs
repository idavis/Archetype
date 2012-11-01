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
    public class DelegatingPrototype : DynamicObject, IPrototypalMetaObjectProvider
    {
        private readonly List<object> _Prototypes = new List<object>();
 
        public DelegatingPrototype()
                : this( null )
        {
        }

        public DelegatingPrototype( object prototype )
        {
            _Prototypes.Add(prototype);
        }

        #region IPrototypalMetaObjectProvider Members

        public virtual object Prototype
        {
            get { return _Prototypes[0]; }
            set { _Prototypes[0] = value; }
        }

        public virtual IList<object> Prototypes { get { return _Prototypes; } }

        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            if (Prototypes.Count == 0 || Prototype == null)
            {
                return GetBaseMetaObject(parameter);
            }
            return _Prototypes.Count == 1 ? new PrototypalMetaObject( parameter, this, Prototype ) : new PrototypalMetaObject(parameter, this, _Prototypes);
        }

        public virtual DynamicMetaObject GetBaseMetaObject( Expression parameter )
        {
            return base.GetMetaObject( parameter );
        }

        #endregion
    }
}