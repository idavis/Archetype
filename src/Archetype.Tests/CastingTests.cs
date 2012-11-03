using System;
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

    public class DisposableDelegatingObject : DelegatingPrototype, IDisposable
    {
        public DisposableDelegatingObject(params object[] prototypes) : base(prototypes)
        {
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
    public class CastingTests
    {
        [Test]
        public void CanCastToImplementedInterface()
        {
            dynamic value = new DisposableDelegatingObject();
            IDisposable instance = value;
            Assert.IsNotNull(instance);
        }

        [Test]
        public void CanCastToModuleWhenOnlyASingleModuleIsSupplied()
        {
            dynamic value = new DelegatingPrototype(new DisposableObject());
            IDisposable instance = value;
            Assert.IsNotNull(instance);
        }

        [Test]
        public void CastingToAnInterfaceWhenThereIsAConflictBetweenModulesPrefersTheModuleThatWasLoadedLast()
        {
            var first = new DisposableObject();
            var second = new DisposableObject();
            dynamic value = new DelegatingPrototype(first, second);
            IDisposable instance = value;
            Assert.AreSame(second, instance);
        }

        [Test]
        public void CastingToAnInterfaceWhenThereIsAConflictPrefersTheRootObjectOverModules()
        {
            dynamic value = new DisposableDelegatingObject(new DisposableObject());
            IDisposable instance = value;
            Assert.AreSame(value, instance);
        }

        [Test]
        public void CastingWillSkipPrototypesWhichAreNotValidTargetsForTheCast()
        {
            var inner = new DisposableObject();
            dynamic value = new DelegatingPrototype(inner, DateTime.Now);
            IDisposable instance = value;
            Assert.AreSame(instance, inner);
        }
    }
}