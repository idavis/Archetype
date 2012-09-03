#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Prototype.Ps
{
    public class PrototypalMetaObject : DynamicMetaObject
    {
        public PrototypalMetaObject(Expression expression, object value,
                                    Func<DynamicMetaObject> baseMetaObjectFactory)
            : base(expression, BindingRestrictions.Empty, value)
        {
            if (baseMetaObjectFactory == null)
            {
                throw new ArgumentNullException("baseMetaObjectFactory");
            }
            CreateBaseMetaObject = baseMetaObjectFactory;
        }

        protected virtual Func<DynamicMetaObject> CreateBaseMetaObject { get; set; }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            DynamicMetaObject baseMeta = CreateBaseMetaObject();
            baseMeta = baseMeta.BindInvokeMember(binder, args);
            DynamicMetaObject errorSuggestion = binder.FallbackInvokeMember(this, args);
            errorSuggestion = binder.FallbackInvokeMember(this, args, baseMeta);

            DynamicMetaObject prototypeMetaObject = CreatePrototypeMetaObject();
            if (prototypeMetaObject == null) return errorSuggestion;
            errorSuggestion = binder.FallbackInvokeMember(prototypeMetaObject, args, errorSuggestion);

            return errorSuggestion;
        }

        public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            DynamicMetaObject baseMeta = CreateBaseMetaObject();
            baseMeta = baseMeta.BindInvoke(binder, args);
            DynamicMetaObject errorSuggestion = binder.FallbackInvoke(this, args);
            errorSuggestion = binder.FallbackInvoke(this, args, baseMeta);

            DynamicMetaObject prototypeMetaObject = CreatePrototypeMetaObject();
            if (prototypeMetaObject == null) return errorSuggestion;
            errorSuggestion = binder.FallbackInvoke(prototypeMetaObject, args, errorSuggestion);

            return errorSuggestion;
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            DynamicMetaObject baseMeta = CreateBaseMetaObject();
            baseMeta = baseMeta.BindSetMember(binder, value);
            DynamicMetaObject errorSuggestion = binder.FallbackSetMember(this, value);
            errorSuggestion = binder.FallbackSetMember(this, value, baseMeta);

            DynamicMetaObject prototypeMetaObject = CreatePrototypeMetaObject();
            if (prototypeMetaObject == null) return errorSuggestion;
            errorSuggestion = binder.FallbackSetMember(prototypeMetaObject, value, errorSuggestion);

            return errorSuggestion;
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            DynamicMetaObject baseMeta = CreateBaseMetaObject();
            baseMeta = baseMeta.BindGetMember(binder);
            DynamicMetaObject errorSuggestion = binder.FallbackGetMember(this);
            errorSuggestion = binder.FallbackGetMember(this, baseMeta);

            DynamicMetaObject prototypeMetaObject = CreatePrototypeMetaObject();
            if (prototypeMetaObject == null) return errorSuggestion;
            errorSuggestion = binder.FallbackGetMember(prototypeMetaObject, errorSuggestion);

            return errorSuggestion;
        }

        protected virtual DynamicMetaObject CreatePrototypeMetaObject()
        {
            var value = Value as PrototypalObject;
            if (value != null && value.Prototype != null)
            {
                MemberExpression expression = Expression.Property(Expression.Constant(value), "Prototype");
                DynamicMetaObject prototypeMetaObject = Create(value.Prototype, expression);
                return prototypeMetaObject;
            }
            return null;
        }
    }
}