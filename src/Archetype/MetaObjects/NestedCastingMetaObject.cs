#region License

// 
// Copyright (c) 2012-2013, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace Archetype.MetaObjects
{
    public class NestedCastingMetaObject : ModuleMetaObject
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NestedCastingMetaObject" /> class.
        /// </summary>
        /// <param name="expression">
        ///     The expression representing this <see cref="DynamicMetaObject" /> during the dynamic binding process.
        /// </param>
        /// <param name="value">
        ///     The runtime value represented by the <see cref="DynamicMetaObject" />.
        /// </param>
        /// <param name="baseMetaObject">
        ///     The <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding against the primary delegating object.
        /// </param>
        /// <param name="modules">
        ///     The modules current collection of modules which will be used as potential delegation targets for dynamic binding operations.
        /// </param>
        public NestedCastingMetaObject( Expression expression,
                                        object value,
                                        DynamicMetaObject baseMetaObject,
                                        IList<object> modules )
                : base( expression, value, baseMetaObject, modules )
        {
        }

        /// <summary>
        ///     Performs the binding of the dynamic conversion operation.
        /// </summary>
        /// <param name="binder">
        ///     An instance of the <see cref="T:System.Dynamic.ConvertBinder" /> that represents the details of the dynamic operation.
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        public override DynamicMetaObject BindConvert( ConvertBinder binder )
        {
            return ApplyBinding( meta => Convert( binder, meta ), binder.FallbackConvert );
        }

        protected static bool TryConvert( ConvertBinder binder, DynamicMetaObject instance, out DynamicMetaObject result )
        {
            if ( instance.HasValue &&
                 instance.RuntimeType.IsValueType )
            {
                result = instance.BindConvert( binder );
                return true;
            }

            if ( binder.Type.IsInterface )
            {
                Expression expression = Convert( instance.Expression, binder.Type );
                result = new DynamicMetaObject( expression, BindingRestrictions.Empty, instance.Value );
                result = result.BindConvert( binder );
                return true;
            }

            if ( typeof (IDynamicMetaObjectProvider).IsAssignableFrom( instance.RuntimeType ) )
            {
                result = instance.BindConvert( binder );
                return true;
            }

            result = null;
            return false;
        }

        protected static DynamicMetaObject Convert( ConvertBinder binder, DynamicMetaObject instance )
        {
            DynamicMetaObject result;
            return TryConvert( binder, instance, out result ) ? result : instance;
        }

        protected static Expression Convert( Expression expression, Type type )
        {
            return expression.Type == type ? expression : Expression.Convert( expression, type );
        }
    }
}