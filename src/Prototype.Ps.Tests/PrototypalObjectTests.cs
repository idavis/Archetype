using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

namespace Prototype.Ps.Tests
{
    [TestFixture]
    public class PrototypalObjectTests
    {
        #region Setup/Teardown

        [SetUp]
        public virtual void Setup()
        {
            Value = Create();
        }

        #endregion

        protected virtual PrototypalObject Create()
        {
            return new PrototypalObject();
        }

        public virtual PrototypalObject Value { get; set; }

        public virtual dynamic DynamicValue
        {
            get { return Value; }
        }

        [Test]
        public void Calling_a_function_when_there_is_no_function_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>(() => DynamicValue.Foo());
        }

        [Test]
        public void Getting_a_property_when_there_is_no_property_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>(() => { int value = DynamicValue.Foo; });
        }

        [Test]
        public void Invoking_the_object_when_there_is_no_backing_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>(() => DynamicValue(1, 2));
        }

        [Test]
        public void Setting_a_property_when_there_is_no_property_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>(() => { DynamicValue.Foo = "Bar"; });
        }
    }
}