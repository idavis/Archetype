#region Using Directives

using Archetype.Tests.TestObjects;
using Xunit;

#endregion

namespace Archetype.Tests.MethodTests
{
    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedDynamicObjectWithMethods : DelegatingObjectTestsDefinedMethods
    {
        public DelegatingObjectTestsNestedDynamicObjectWithMethods()
        {
            Value = new DelegatingObject( new DynamicObjectWithMethods() );
        }
    }

    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedModuleWithMethods : DelegatingObjectTestsDefinedMethods
    {
        public DelegatingObjectTestsNestedModuleWithMethods()
        {
            Value = new DelegatingObject( new ModuleWithMethods() );
        }
    }

    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedModuleWithMethodsWithEmptyModule : DelegatingObjectTestsDefinedMethods
    {
        public DelegatingObjectTestsNestedModuleWithMethodsWithEmptyModule()
        {
            Value = new DelegatingObject( new ModuleWithMethods( new DelegatingObject() ) );
        }
    }

    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDoubleNestedModuleWithMethods : DelegatingObjectTestsDefinedMethods
    {
        public DelegatingObjectTestsDoubleNestedModuleWithMethods()
        {
            Value = new DelegatingObject( new DelegatingObject( new ModuleWithMethods() ) );
        }
    }

    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDoubleNestedDynamicObjectWithMethods : DelegatingObjectTestsDefinedMethods
    {
        public DelegatingObjectTestsDoubleNestedDynamicObjectWithMethods()
        {
            Value = new DelegatingObject( new DelegatingObject( new DynamicObjectWithMethods() ) );
        }
    }

    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsNestedObjectWithMethods : DelegatingObjectTestsDefinedMethods
    {
        public DelegatingObjectTestsNestedObjectWithMethods()
        {
            Value = new DelegatingObject( new ObjectWithMethods() );
        }
    }

    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDoubleNestedObjectWithMethods : DelegatingObjectTestsDefinedMethods
    {
        public DelegatingObjectTestsDoubleNestedObjectWithMethods()
        {
            Value = new DelegatingObject( new DelegatingObject( new ObjectWithMethods() ) );
        }
    }
}