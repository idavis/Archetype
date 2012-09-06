#region Using Directives

using NUnit.Framework;
using Prototype.Ps.Tests.TestObjects;

#endregion

namespace Prototype.Ps.Tests.MethodTests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsNestedDynamicObjectWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject( new DynamicObjectWithMethods() );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsNestedPrototypeWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject( new ProtoTypalObjectWithMethods() );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsNestedPrototypeWithMethodsWithEmptyPrototype : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject( new ProtoTypalObjectWithMethods( new PrototypalObject() ) );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsDoubleNestedPrototypeWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject( new PrototypalObject( new ProtoTypalObjectWithMethods() ) );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsDoubleNestedDynamicObjectWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject( new PrototypalObject( new DynamicObjectWithMethods() ) );
        }

        #endregion
    }
}