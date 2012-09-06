#region Using Directives

using NUnit.Framework;
using Prototype.Ps.Tests.TestObjects;

#endregion

namespace Prototype.Ps.Tests.StaticTests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsDefinedStaticMethods : PrototypalObjectTests
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new ProtoTypalObjectWithMethods();
        }

        #endregion

        [Test]
        public void Calling_a_static_void_function_with_no_params_defined_in_prototype_is_called()
        {
            DynamicValue.StaticMethodWithNoReturnValueOrParameters();
            Assert.True( DynamicValue.StaticMethodWithNoReturnValueOrParametersWasCalled );
        }
    }
}