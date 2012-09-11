#region Using Directives

using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;
using Prototype.Ps.Tests.TestObjects;

#endregion

namespace Prototype.Ps.Tests.StaticTests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsDefinedStaticMethodsOnObjectPrototype : PrototypalObjectTests
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new PrototypalObject( new ObjectWithMethods() );
        }

        #endregion

        [Test]
        public void Calling_a_static_void_function_with_no_params_defined_in_non_dynamic_prototype_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.StaticMethodWithNoReturnValueOrParameters() );
        }

        [Test]
        public void Getting_a_static_property_defined_in_non_dynamic_prototype_throws()
        {
            TestDelegate action =
                    () => { dynamic value = DynamicValue.StaticMethodWithNoReturnValueOrParametersWasCalled; };
            Assert.Throws<RuntimeBinderException>( action );
        }

        [Test]
        public void Setting_a_static_property_defined_in_non_dynamic_prototype_throws()
        {
            TestDelegate action =
                    () => { DynamicValue.StaticMethodWithNoReturnValueOrParametersWasCalled = false; };
            Assert.Throws<RuntimeBinderException>( action );
        }
    }
}