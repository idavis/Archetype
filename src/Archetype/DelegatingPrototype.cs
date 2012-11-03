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
        public DelegatingObject(params object[] modules)
        {
            Modules = new List<object>(modules.Length);
            foreach (object module in modules)
            {
                Modules.Add(module);
            }
        }

        public IList<object> Modules { get; protected set; }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            if (Modules == null || Modules.Count == 0)
            {
                return GetBaseMetaObject(parameter);
            }
            DynamicMetaObject baseMetaObject = GetBaseMetaObject(parameter);
            return new DynamicModuleMetaObject(parameter, this, baseMetaObject, Modules);
        }

        public virtual DynamicMetaObject GetBaseMetaObject(Expression parameter)
        {
            return base.GetMetaObject(parameter);
        }
    }

    public class DelegatingPrototype : DelegatingObject
    {
        public DelegatingPrototype(params object[] modules) : base(modules)
        {
        }

        public virtual object Prototype
        {
            get { return Modules[0]; }
            set { Modules[0] = value; }
        }
    }
}