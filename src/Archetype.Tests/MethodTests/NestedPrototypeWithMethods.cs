#region Using Directives

using Archetype.Tests.TestObjects;
using NUnit.Framework;

#endregion

namespace Archetype.Tests.MethodTests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedDynamicObjectWithMethods : DelegatingObjectTestsDefinedMethods
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
    public class DelegatingObjectTestsNestedModuleWithMethods : DelegatingObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new ModuleWithMethods() );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedModuleWithMethodsWithEmptyModule : DelegatingObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new ModuleWithMethods( new DelegatingObject() ) );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDoubleNestedModuleWithMethods : DelegatingObjectTestsDefinedMethods
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new DelegatingObject( new ModuleWithMethods() ) );
        }

        #endregion
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDoubleNestedDynamicObjectWithMethods : DelegatingObjectTestsDefinedMethods
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
    public class DelegatingObjectTestsNestedObjectWithMethods : DelegatingObjectTestsDefinedMethods
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
    public class DelegatingObjectTestsDoubleNestedObjectWithMethods : DelegatingObjectTestsDefinedMethods
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