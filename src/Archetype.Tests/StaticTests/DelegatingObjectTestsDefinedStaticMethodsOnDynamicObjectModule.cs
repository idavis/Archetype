#region Using Directives

using Archetype.Tests.TestObjects;
using Microsoft.CSharp.RuntimeBinder;
using Xunit;

#endregion

namespace Archetype.Tests.StaticTests
{
    //[TestFixture]
    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDefinedStaticMethodsOnDynamicObjectModule : DelegatingObjectTests
    {
        public DelegatingObjectTestsDefinedStaticMethodsOnDynamicObjectModule()
        {
            Value = new DelegatingObject( new DynamicObjectWithMethods() );
        }

        [Fact]
        public void Calling_a_static_void_function_when_a_prototypal_object_is_not_the_last_module_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.StaticMethodWithNoReturnValueOrParameters() );
        }
    }
}