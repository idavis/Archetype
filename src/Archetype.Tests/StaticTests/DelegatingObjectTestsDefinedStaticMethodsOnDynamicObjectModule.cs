#region Using Directives

using Archetype.Tests.TestObjects;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

#endregion

namespace Archetype.Tests.StaticTests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsDefinedStaticMethodsOnDynamicObjectModule : DelegatingObjectTests
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            Value = new DelegatingObject( new DynamicObjectWithMethods() );
        }

        #endregion

        [Test]
        public void Calling_a_static_void_function_when_a_prototypal_object_is_not_the_last_prototype_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.StaticMethodWithNoReturnValueOrParameters() );
        }
    }
}