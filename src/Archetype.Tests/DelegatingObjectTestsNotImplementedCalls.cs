#region Using Directives

using Microsoft.CSharp.RuntimeBinder;
using Xunit;

#endregion

namespace Archetype.Tests
{
    //[Timeout( Constants.TestTimeOutInMs )]
    public abstract class DelegatingObjectTestsNotImplementedCalls : DelegatingObjectTests
    {
        [Fact]
        public void Calling_a_function_when_there_is_no_function_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue.Foo() );
        }

        [Fact]
        public void Casting_the_object_when_no_suitable_cast_exists_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => { var value = (string) DynamicValue; } );
        }

        [Fact]
        public void Getting_a_property_when_there_is_no_property_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => { int value = DynamicValue.Foo; } );
        }

        [Fact]
        public void Invoking_the_object_when_there_is_no_backing_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => DynamicValue( 1, 2 ) );
        }

        [Fact]
        public void Setting_a_property_when_there_is_no_property_defined_throws()
        {
            Assert.Throws<RuntimeBinderException>( () => { DynamicValue.Foo = "Bar"; } );
        }

        [Fact]
        public void Setting_a_property_when_there_is_no_property_defined_throwss()
        {
            Assert.Throws<RuntimeBinderException>( () => { dynamic value = DynamicValue + DynamicValue; } );
        }
    }
}