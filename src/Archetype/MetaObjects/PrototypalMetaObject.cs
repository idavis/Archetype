#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace Archetype.MetaObjects
{
    public class PrototypalMetaObject : DelegatingMetaObject
    {
        private readonly object _Prototype;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PrototypalMetaObject" /> class.
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
        /// <param name="prototype">
        ///     The prototype object which will be the target of delegation for dynamic binding operations.
        /// </param>
        public PrototypalMetaObject( Expression expression,
                                     object value,
                                     DynamicMetaObject baseMetaObject,
                                     object prototype )
                : base( expression, value, baseMetaObject )
        {
            _Prototype = prototype;
        }

        /// <summary>
        ///     Implemented by inheritors, this function should determine the receiver for the binding of the dynamic operation and
        ///     use the <paramref name="bindTarget" /> to perform the binding and return the result of that binding operation.
        /// </summary>
        /// <param name="bindTarget">
        ///     Performs the binding of the dynamic operation if the target dynamic object cannot bind.
        ///     <param>The target of the dynamic operation.</param>
        ///     <returns>
        ///         The <see cref="DynamicMetaObject" /> representing the result of the binding.
        ///     </returns>
        /// </param>
        /// <returns>
        ///     The new <see cref="T:System.Dynamic.DynamicMetaObject" /> representing the result of the binding.
        /// </returns>
        /// <remarks>
        ///     The <paramref name="bindTarget" /> can be used multiple times to attempt potential binding operations.
        /// </remarks>
        protected override DynamicMetaObject Resolve( Func<DynamicMetaObject, DynamicMetaObject> bindTarget )
        {
            DynamicMetaObject metaObject = CreateBoundDynamicMetaObject( bindTarget, _Prototype );
            return metaObject;
        }
    }
}