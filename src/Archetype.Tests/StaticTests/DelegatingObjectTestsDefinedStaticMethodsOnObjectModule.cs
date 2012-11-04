#region Using Directives

using Archetype.Tests.TestObjects;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

#endregion

namespace Archetype.Tests.StaticTests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDefinedStaticMethodsOnObjectModule : DelegatingObjectTests
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new ObjectWithMethods() );
        }

        #endregion

        [Test]
        public void Calling_a_static_void_function_with_no_params_defined_in_non_dynamic_module_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.StaticMethodWithNoReturnValueOrParameters() );
        }

        [Test]
        public void Getting_a_static_property_defined_in_non_dynamic_module_throws()
        {
            TestDelegate action =
                    () => { dynamic value = DynamicValue.StaticMethodWithNoReturnValueOrParametersWasCalled; };
            Assert.Throws<RuntimeBinderException>( action );
        }

        [Test]
        public void Setting_a_static_property_defined_in_non_dynamic_module_throws()
        {
            TestDelegate action =
                    () => { DynamicValue.StaticMethodWithNoReturnValueOrParametersWasCalled = false; };
            Assert.Throws<RuntimeBinderException>( action );
        }
    }
}