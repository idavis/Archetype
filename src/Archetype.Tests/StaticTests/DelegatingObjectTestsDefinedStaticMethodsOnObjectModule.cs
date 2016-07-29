#region Using Directives

using System;
using Archetype.Tests.TestObjects;
using Microsoft.CSharp.RuntimeBinder;
using Xunit;

#endregion

namespace Archetype.Tests.StaticTests
{
    //[TestFixture]
    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDefinedStaticMethodsOnObjectModule : DelegatingObjectTests
    {
      
        public DelegatingObjectTestsDefinedStaticMethodsOnObjectModule()
        {
            Value = new DelegatingObject( new ObjectWithMethods() );
        }

        [Fact]
        public void Calling_a_static_void_function_with_no_params_defined_in_non_dynamic_module_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.StaticMethodWithNoReturnValueOrParameters() );
        }

        [Fact]
        public void Getting_a_static_property_defined_in_non_dynamic_module_throws()
        {
            var action =
                    (Action)(() => { dynamic value = DynamicValue.StaticMethodWithNoReturnValueOrParametersWasCalled; });
            Assert.Throws<RuntimeBinderException>( action );
        }

        [Fact]
        public void Setting_a_static_property_defined_in_non_dynamic_module_throws()
        {
            var action =
                    (Action)(() => { DynamicValue.StaticMethodWithNoReturnValueOrParametersWasCalled = false; });
            Assert.Throws<RuntimeBinderException>( action );
        }
    }
}