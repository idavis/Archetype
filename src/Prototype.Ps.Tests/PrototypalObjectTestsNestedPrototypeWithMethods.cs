using NUnit.Framework;

namespace Prototype.Ps.Tests
{
    [TestFixture]
    public class zPrototypalObjectTestsNestedPrototypeWithMethods : PrototypalObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new MethodHolder(new PrototypalObject());
        }
    }


    [TestFixture]
    public class PrototypalObjectTestsNestedPrototypeWithMethods : PrototypalObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject(new MethodHolder());
        }
    }

    [TestFixture]
    public class PrototypalObjectTestsDoubleNestedPrototypeWithMethods : PrototypalObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject(new PrototypalObject(new MethodHolder()));
        }
    }
}