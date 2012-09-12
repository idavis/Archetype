#region Using Directives

using NUnit.Framework;

#endregion

namespace Archetype.Tests
{
    [TestFixture]
    public abstract class PrototypalObjectTests
    {
        #region Setup/Teardown

        [SetUp]
        public virtual void Setup()
        {
            Value = Create();
        }

        #endregion

        public virtual PrototypalObject Value { get; set; }

        public virtual dynamic DynamicValue
        {
            get { return Value; }
        }

        protected virtual PrototypalObject Create()
        {
            return new PrototypalObject();
        }
    }
}