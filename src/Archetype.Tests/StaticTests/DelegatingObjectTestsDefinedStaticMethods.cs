#region Using Directives

using Archetype.Tests.TestObjects;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

#endregion

namespace Archetype.Tests.StaticTests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDefinedStaticMethods : DelegatingObjectTests
    {
        [SetUp]
        public override void Setup()
        {
            Value = new ModuleWithMethods();
        }

        [Test]
        public void Calling_a_static_void_function_with_no_params_defined_in_module_is_called_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.StaticMethodWithNoReturnValueOrParameters() );
        }
    }
}