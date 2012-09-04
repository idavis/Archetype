using NUnit.Framework;

namespace Prototype.Ps.Tests
{
    [TestFixture]
    public class PrototypalObjectTestsDefinedMethods:PrototypalObjectTests
    {
        [SetUp]
        public override void Setup()
        {
            Value = new MethodHolder();
        }

        [Test]
        public void Calling_a_void_function_with_no_params_defined_in_prototype_is_called()
        {
            DynamicValue.MethodWithNoReturnValueOrParameters();
            Assert.True(DynamicValue.MethodWithNoReturnValueOrParametersWasCalled);
        }

        [Test]
        public void Calling_a_void_function_with_a_param_defined_in_prototype_is_called()
        {
            DynamicValue.MethodWithNoReturnValueSingleParameter(42);
            Assert.True(DynamicValue.MethodWithNoReturnValueSingleParameterWasCalled);
        }

        [Test]
        public void Calling_a_void_function_with_a_param_defined_in_prototype_passes_the_parameter()
        {
            DynamicValue.MethodWithNoReturnValueSingleParameter(42);
            Assert.AreEqual(42, DynamicValue.MethodWithNoReturnValueSingleParameterValue);
        }

        [Test]
        public void Calling_a_function_with_a_retval_with_no_param_defined_is_called()
        {
            DynamicValue.MethodWithReturnValueNoParameters();
            Assert.True(DynamicValue.MethodWithReturnValueNoParametersWasCalled);
        }

        [Test]
        public void Calling_a_function_with_a_retval_with_no_param_defined_returns_value()
        {
            var actual = DynamicValue.MethodWithReturnValueNoParameters();
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void Calling_a_function_with_a_retval_with_a_param_defined_in_prototype_is_called()
        {
            DynamicValue.MethodWithReturnValueSingleParameter(42);
            Assert.True(DynamicValue.MethodWithReturnValueSingleParameterWasCalled);
        }

        [Test]
        public void Calling_a_function_with_a_retval_with_a_param_defined_in_prototype_passes_the_parameter()
        {
            var actual = DynamicValue.MethodWithReturnValueSingleParameter(42);
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void Invoking_a_function_with_an_out_param_sets_the_value()
        {
            int actual;
            DynamicValue.MethodWithOutParameter(out actual);
            Assert.AreEqual(42, actual);
        }

        [Test]
        public void Invoking_a_function_with_an_ref_param_sets_the_value()
        {
            int actual = -1;
            DynamicValue.MethodWithRefParameter(ref actual);
            Assert.AreEqual(42, actual);
        }
    }
}