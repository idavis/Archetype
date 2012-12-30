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
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace Archetype.MetaObjects
{
    public class ModuleMetaObject : DelegatingMetaObject
    {
        private readonly IList<object> _Modules;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModuleMetaObject" /> class.
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
        public ModuleMetaObject( Expression expression,
                                 object value,
                                 DynamicMetaObject baseMetaObject,
                                 IList<object> modules )
                : base( expression, value, baseMetaObject )
        {
            _Modules = modules;
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
            // The modules are processed in reverse to preserve a LIFO ordering. This first module to successfully bind wins. 
            for ( int index = _Modules.Count - 1; index >= 0; index-- )
            {
                object module = _Modules[index];
                DynamicMetaObject metaObject = CreateBoundDynamicMetaObject( bindTarget, module );

                if ( BindingHasFailed( metaObject ) )
                {
                    continue;
                }

                return metaObject;
            }
            return null;
        }
    }
}