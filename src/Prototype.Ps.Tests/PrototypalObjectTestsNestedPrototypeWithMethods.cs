using NUnit.Framework;

namespace Prototype.Ps.Tests
{
    [TestFixture]
    public class zPrototypalObjectTestsNestedPrototypeWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new MethodHolder(new PrototypalObject());
        }

        #endregion
    }


    [TestFixture]
    public class PrototypalObjectTestsNestedPrototypeWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject(new MethodHolder());
        }

        #endregion
    }

    [TestFixture]
    public class PrototypalObjectTestsDoubleNestedPrototypeWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject(new PrototypalObject(new MethodHolder()));
        }

        #endregion
    }
}