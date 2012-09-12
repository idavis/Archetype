#region Using Directives

using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

#endregion

namespace Archetype.Tests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public abstract class PrototypalObjectTestsNotImplementedCalls : PrototypalObjectTests
    {
        [Test]
        public void Calling_a_function_when_there_is_no_function_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.Foo() );
        }

        [Test]
        public void Casting_the_object_when_no_suitable_cast_exists_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => { var value = (string) DynamicValue; } );
        }

        [Test]
        public void Getting_a_property_when_there_is_no_property_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => { int value = DynamicValue.Foo; } );
        }

        [Test]
        public void Invoking_the_object_when_there_is_no_backing_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue( 1, 2 ) );
        }

        [Test]
        public void Setting_a_property_when_there_is_no_property_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => { DynamicValue.Foo = "Bar"; } );
        }

        [Test]
        public void Setting_a_property_when_there_is_no_property_defined_throwss()
        {
            Assert.Throws<RuntimeBinderException>( () => { dynamic value = DynamicValue + DynamicValue; } );
        }
    }
}