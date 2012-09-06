#region Using Directives

using NUnit.Framework;

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
            //Value = new PrototypalObject(new ObjectWithMethods());
            Value = null;
        }

        #endregion

        [Test]
        [Ignore]
        public void Calling_a_static_void_function_with_no_params_defined_in_prototype_is_called()
        {
            DynamicValue.StaticMethodWithNoReturnValueOrParameters();
            Assert.True( DynamicValue.StaticMethodWithNoReturnValueOrParametersWasCalled );
        }
    }
}