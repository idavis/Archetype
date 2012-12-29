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
        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new DynamicObjectWithMethods() );
        }
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedModuleWithMethods : DelegatingObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new ModuleWithMethods() );
        }
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedModuleWithMethodsWithEmptyModule : DelegatingObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new ModuleWithMethods( new DelegatingObject() ) );
        }
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDoubleNestedModuleWithMethods : DelegatingObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new DelegatingObject( new ModuleWithMethods() ) );
        }
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDoubleNestedDynamicObjectWithMethods : DelegatingObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new DelegatingObject( new DynamicObjectWithMethods() ) );
        }
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedObjectWithMethods : DelegatingObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new ObjectWithMethods() );
        }
    }

    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDoubleNestedObjectWithMethods : DelegatingObjectTestsDefinedMethods
    {
        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new DelegatingObject( new ObjectWithMethods() ) );
        }
    }
}