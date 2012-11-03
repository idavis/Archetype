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
    public class DelegatingObject : DynamicObject
    {
        public IList<object> Prototypes { get; protected set; }

        public DelegatingObject(params object[] modules)
        {
            Prototypes = new List<object>(modules.Length);
            foreach (var module in modules)
            {
                Prototypes.Add(module);
            }
        }  
    }

    public class DelegatingPrototype : DelegatingObject
    {
        public DelegatingPrototype(params object[] modules) : base(modules)
        {
        }

        #region IPrototypalMetaObjectProvider Members

        public virtual object Prototype
        {
            get { return Prototypes[0]; }
            set { Prototypes[0] = value; }
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            if (Prototypes.Count == 0 || Prototype == null)
            {
                return GetBaseMetaObject(parameter);
            }
            var baseMetaObject = GetBaseMetaObject(parameter);
            return new PrototypalMetaObject(parameter, this, baseMetaObject, Prototypes);
        }

        public virtual DynamicMetaObject GetBaseMetaObject(Expression parameter)
        {
            return base.GetMetaObject(parameter);
        }

        #endregion
    }
}