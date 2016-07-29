#region Using Directives

using Archetype.Tests.TestObjects;
using Microsoft.CSharp.RuntimeBinder;
using Xunit;

#endregion

namespace Archetype.Tests.StaticTests
{
    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDefinedStaticMethods : DelegatingObjectTests
    {
        public DelegatingObjectTestsDefinedStaticMethods()
        {
            Value = new ModuleWithMethods();
        }

        [Fact]
        public void Calling_a_static_void_function_with_no_params_defined_in_module_is_called_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.StaticMethodWithNoReturnValueOrParameters() );
        }
    }
}