#region Using Directives

using NUnit.Framework;

#endregion

namespace Archetype.Tests
{
    [TestFixture]
    public abstract class DelegatingObjectTests
    {
        [SetUp]
        public virtual void Setup()
        {
            Value = Create();
        }

        public virtual DelegatingObject Value { get; set; }

        public virtual dynamic DynamicValue
        {
            get { return Value; }
        }

        protected virtual DelegatingObject Create()
        {
            return new DelegatingObject();
        }
    }
}