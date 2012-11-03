#region Using Directives

using Archetype.Tests.TestObjects;
using NUnit.Framework;

#endregion

namespace Archetype.Tests.MethodTests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsNestedDynamicObjectWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new DynamicObjectWithMethods() );
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
            Value = new DelegatingObject( new ProtoTypalObjectWithMethods() );
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
            Value = new DelegatingObject( new ProtoTypalObjectWithMethods( new DelegatingObject() ) );
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
            Value = new DelegatingObject( new DelegatingObject( new ProtoTypalObjectWithMethods() ) );
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
            Value = new DelegatingObject( new DelegatingObject( new DynamicObjectWithMethods() ) );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsNestedObjectWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new ObjectWithMethods() );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsDoubleNestedObjectWithMethods : PrototypalObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new DelegatingObject( new ObjectWithMethods() ) );
        }

        #endregion
    }
}