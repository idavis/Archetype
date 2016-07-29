#region Using Directives

using Archetype.Tests.TestObjects;
using Xunit;

#endregion

namespace Archetype.Tests.MethodTests
{
    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDefinedMethods : DelegatingObjectTests
    {
        [Fact]
        public void Calling_a_function_with_a_retval_with_a_param_defined_in_module_is_called()
        {
            Value = new ModuleWithMethods();
            dynamic result = DynamicValue.MethodWithReturnValueSingleParameter( 42 );
            Assert.True( DynamicValue.MethodWithReturnValueSingleParameterWasCalled );
        }

        [Fact]
        public void Calling_a_function_with_a_retval_with_a_param_defined_in_module_passes_the_parameter()
        {
            Value = new ModuleWithMethods();
            dynamic actual = DynamicValue.MethodWithReturnValueSingleParameter( 42 );
            Assert.Equal( 42, actual );
        }

        [Fact]
        public void Calling_a_function_with_a_retval_with_no_param_defined_is_called()
        {
            Value = new ModuleWithMethods();
            DynamicValue.MethodWithReturnValueNoParameters();
            Assert.True( DynamicValue.MethodWithReturnValueNoParametersWasCalled );
        }

        [Fact]
        public void Calling_a_function_with_a_retval_with_no_param_defined_returns_value()
        {
            Value = new ModuleWithMethods();
            dynamic actual = DynamicValue.MethodWithReturnValueNoParameters();
            Assert.Equal( 42, actual );
        }

        [Fact]
        public void Calling_a_void_function_with_a_param_defined_in_module_is_called()
        {
            Value = new ModuleWithMethods();
            DynamicValue.MethodWithNoReturnValueSingleParameter( 42 );
            Assert.True( DynamicValue.MethodWithNoReturnValueSingleParameterWasCalled );
        }

        [Fact]
        public void Calling_a_void_function_with_a_param_defined_in_module_passes_the_parameter()
        {
            Value = new ModuleWithMethods();
            DynamicValue.MethodWithNoReturnValueSingleParameter( 42 );
            Assert.Equal( 42, DynamicValue.MethodWithNoReturnValueSingleParameterValue );
        }

        [Fact]
        public void Calling_a_void_function_with_no_params_defined_in_module_is_called()
        {
            Value = new ModuleWithMethods();
            DynamicValue.MethodWithNoReturnValueOrParameters();
            Assert.True( DynamicValue.MethodWithNoReturnValueOrParametersWasCalled );
        }

        [Fact]
        public void Invoking_a_function_with_an_out_param_sets_the_value()
        {
            Value = new ModuleWithMethods();
            int actual;
            DynamicValue.MethodWithOutParameter( out actual );
            Assert.Equal( 42, actual );
        }

        [Fact]
        public void Invoking_a_function_with_an_ref_param_sets_the_value()
        {
            Value = new ModuleWithMethods();
            int actual = -1;
            DynamicValue.MethodWithRefParameter( ref actual );
            Assert.Equal( 42, actual );
        }
    }
}