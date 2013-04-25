using System;
using System.Dynamic;
using System.Linq.Expressions;
using Archetype.MetaObjects;
using NUnit.Framework;

namespace Archetype.Tests
{
    public class DisposableObject : IDisposable
    {
        public bool IsDisposed { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            IsDisposed = true;
        }

        #endregion
    }

    public class DisposableDelegatingObject : DelegatingObject, IDisposable
    {
        public DisposableDelegatingObject( params object[] modules )
                : base( modules )
        {
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            DynamicMetaObject baseMetaObject = GetBaseMetaObject(parameter);

            if (Modules == null ||
                 Modules.Count == 0)
            {
                return baseMetaObject;
            }

            return new NestedCastingMetaObject(parameter, this, baseMetaObject, Modules);
        }

        public bool IsDisposed { get; private set; }

        #region IDisposable Members

        public void Dispose()
        {
            IsDisposed = true;
        }

        #endregion
    }


    [TestFixture]
    [Ignore]
    public class CastingTests
    {
        [Test]
        public void CanCastToImplementedInterface()
        {
            dynamic value = new DisposableDelegatingObject();
            IDisposable instance = value;
            Assert.IsNotNull( instance );
        }

        [Test]
        public void CanCastToModuleWhenOnlyASingleModuleIsSupplied()
        {
            dynamic value = new DelegatingObject( new DisposableObject() );
            IDisposable instance = value;
            Assert.IsNotNull( instance );
        }

        [Test]
        public void CastingToABaseClassWithADelegatingObjectInTheModuleChainCastsSuccessfully()
        {
            var outer = new Cat();
            dynamic value = new DelegatingObject( outer, new DelegatingObject() );
            Animal animal = value;
            Assert.AreSame( outer, animal );
        }

        [Test]
        public void CastingToAModulesBaseClassWillResolveAndCastThatModule()
        {
            var outer = new Cat();
            dynamic value = new DelegatingObject( DateTime.Now, outer, 10 );
            Animal animal = value;
            Assert.AreSame( outer, animal );
        }

        [Test]
        public void CastingToAnInterfaceWhenThereIsAConflictBetweenModulesPrefersTheModuleThatWasLoadedLast()
        {
            var first = new DisposableObject();
            var second = new DisposableObject();
            dynamic value = new DelegatingObject( first, second );
            IDisposable instance = value;
            Assert.AreSame( second, instance );
        }

        [Test]
        public void CastingToAnInterfaceWhenThereIsAConflictPrefersTheRootObjectOverModules()
        {
            dynamic value = new DisposableDelegatingObject( new DisposableObject() );
            IDisposable instance = value;
            Assert.AreSame( value, instance );
        }

        [Test]
        public void CastingWillSkipModulesWhichAreNotValidTargetsForTheCast()
        {
            var inner = new DisposableObject();
            dynamic value = new DelegatingObject( inner, new DelegatingObject() );
            IDisposable instance = value;
            Assert.AreSame( instance, inner );
        }
    }
}