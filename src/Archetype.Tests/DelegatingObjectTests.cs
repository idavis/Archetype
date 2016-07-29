#region Using Directives

using Xunit;

#endregion

namespace Archetype.Tests
{
    public abstract class DelegatingObjectTests
    {
        public DelegatingObjectTests()
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