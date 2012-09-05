#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

using System.Dynamic;
using System.Linq.Expressions;

namespace Prototype.Ps
{
    public class PrototypalMetaObject : DynamicMetaObject
    {
        private readonly DynamicMetaObject _metaObject;
        private readonly IDynamicMetaObjectProvider _prototype;

        public PrototypalMetaObject(Expression expression, object value, IDynamicMetaObjectProvider prototype)
            : base(expression, BindingRestrictions.Empty, value)
        {
            _prototype = prototype;
            _metaObject = CreatePrototypeMetaObject();
        }

        protected virtual DynamicMetaObject AddTypeRestrictions(DynamicMetaObject result)
        {
            BindingRestrictions typeRestrictions =
                BindingRestrictions.GetTypeRestriction(Expression, Value.GetType()).Merge(result.Restrictions);
            return new DynamicMetaObject(result.Expression, typeRestrictions, _metaObject.Value);
        }

        protected virtual DynamicMetaObject CreatePrototypeMetaObject()
        {
            UnaryExpression castExpression = Expression.Convert(Expression, Value.GetType());
            MemberExpression memberExpression = Expression.Property(castExpression, "Prototype");
            DynamicMetaObject prototypeMetaObject = _prototype.GetMetaObject(memberExpression);
            return prototypeMetaObject;
        }

        public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            return AddTypeRestrictions(_metaObject.BindBinaryOperation(binder, arg));
        }

        public override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            return AddTypeRestrictions(_metaObject.BindConvert(binder));
        }

        public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            return AddTypeRestrictions(_metaObject.BindCreateInstance(binder, args));
        }

        public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
        {
            return AddTypeRestrictions(_metaObject.BindDeleteIndex(binder, indexes));
        }

        public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
        {
            return AddTypeRestrictions(_metaObject.BindDeleteMember(binder));
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            return AddTypeRestrictions(_metaObject.BindGetMember(binder));
        }

        public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
        {
            return AddTypeRestrictions(_metaObject.BindGetIndex(binder, indexes));
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            return AddTypeRestrictions(_metaObject.BindInvokeMember(binder, args));
        }

        public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            return AddTypeRestrictions(_metaObject.BindInvoke(binder, args));
        }

        public override DynamicMetaObject BindSetIndex(SetIndexBinder binder,
                                                       DynamicMetaObject[] indexes,
                                                       DynamicMetaObject value)
        {
            return AddTypeRestrictions(_metaObject.BindSetIndex(binder, indexes, value));
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            return AddTypeRestrictions(_metaObject.BindSetMember(binder, value));
        }

        public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
        {
            return AddTypeRestrictions(_metaObject.BindUnaryOperation(binder));
        }
    }
}