#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Prototype.Ps
{
    public delegate bool TryInvokeMemberMissing(InvokeMemberBinder binder, object[] args, out object result);

    public delegate bool TryInvokeMissing(InvokeBinder binder, object[] args, out object result);

    public delegate bool TryGetMemberMissing(GetMemberBinder binder, out object result);

    public delegate bool TrySetMemberMissing(SetMemberBinder binder, object value);

    public class PrototypalObject : DynamicObject
    {
        private const BindingFlags DefaultBindingFlags =
            BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public;

        public PrototypalObject()
            : this(null)
        {
        }

        public PrototypalObject(DynamicObject prototype)
        {
            Prototype = prototype;
        }

        public DynamicObject Prototype { get; protected set; }
        public TryInvokeMemberMissing TryInvokeMemberMissing { get; set; }
        public TryInvokeMissing TryInvokeMissing { get; set; }
        public TryGetMemberMissing TryGetMemberMissing { get; set; }
        public TrySetMemberMissing TrySetMemberMissing { get; set; }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            if (Prototype == null)
            {
                return base.GetMetaObject(parameter);
            }
            return new PrototypalMetaObject(parameter, this, Prototype);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (base.TryInvokeMember(binder, args, out result))
            {
                return true;
            }
            if (TryInvokeStaticMember(binder, args, out result))
            {
                return true;
            }
            if (TryInvokeMemberMissing == null)
            {
                return false;
            }
            return TryInvokeMemberMissing(binder, args, out result);
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            if (base.TryInvoke(binder, args, out result))
            {
                return true;
            }

            if (TryInvokeMissing == null)
            {
                return false;
            }
            return TryInvokeMissing(binder, args, out result);
        }

        protected virtual bool TryInvokeStaticMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            MethodInfo method = GetType().GetMethod(binder.Name, DefaultBindingFlags);
            if (method != null)
            {
                result = method.Invoke(null, args);
                return true;
            }
            if (Prototype != null)
            {
                if (Prototype.TryInvokeMember(binder, args, out result))
                {
                    return true;
                }
            }
            result = null;
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (base.TryGetMember(binder, out result))
            {
                return true;
            }
            if (TryGetStaticMember(binder, out result))
            {
                return true;
            }
            if (TryGetMemberMissing == null)
            {
                return false;
            }
            return TryGetMemberMissing(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (base.TrySetMember(binder, value))
            {
                return true;
            }
            if (TrySetStaticMember(binder, value))
            {
                return true;
            }
            if (TrySetMemberMissing == null)
            {
                return false;
            }
            return TrySetMemberMissing(binder, value);
        }

        protected virtual bool TryGetStaticMember(GetMemberBinder binder, out object result)
        {
            PropertyInfo property = GetType().GetProperty(binder.Name, DefaultBindingFlags);
            if (property != null)
            {
                result = property.GetValue(null, null);
                return true;
            }
            FieldInfo field = GetType().GetField(binder.Name, DefaultBindingFlags);
            if (field != null)
            {
                result = field.GetValue(null);
                return true;
            }
            if (Prototype != null)
            {
                if (Prototype.TryGetMember(binder, out result))
                {
                    return true;
                }
            }
            result = null;
            return false;
        }

        protected virtual bool TrySetStaticMember(SetMemberBinder binder, object value)
        {
            PropertyInfo property = GetType().GetProperty(binder.Name, DefaultBindingFlags);
            if (property != null)
            {
                property.SetValue(null, value, null);
                return true;
            }
            FieldInfo field = GetType().GetField(binder.Name, DefaultBindingFlags);
            if (field != null)
            {
                field.SetValue(null, value);
                return true;
            }
            if (Prototype != null)
            {
                if (Prototype.TrySetMember(binder, value))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool RespondsTo(string name)
        {
            return RespondsTo(name, this);
        }

        public virtual bool RespondsTo(string name, object target)
        {
            var provider = target as IDynamicMetaObjectProvider;
            IEnumerable<string> dynamicMembers = new string[0];
            if (provider != null)
            {
                DynamicMetaObject meta = provider.GetMetaObject(Expression.Constant(target));
                dynamicMembers = meta.GetDynamicMemberNames();
            }

            Type type = target.GetType();
            IEnumerable<string> members = type.GetMembers().Select(member => member.Name);
            members = members.Union(dynamicMembers);

            bool respondsTo = members.Any(item => String.Equals(item, name, StringComparison.OrdinalIgnoreCase));
            if (respondsTo)
            {
                return true;
            }
            var prototypalObject = target as PrototypalObject;
            if (prototypalObject != null && prototypalObject.Prototype != null)
            {
                return RespondsTo(name, prototypalObject.Prototype);
            }
            return false;
        }
    }
}