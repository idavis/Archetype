#region Using Directives

using System.Dynamic;
using Xunit;

#endregion

namespace Archetype.Tests
{
    //[Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsExpandoObjectModules : DelegatingObjectTests
    {
        private dynamic _expandoObject;

        protected override DelegatingObject Create()
        {
            _expandoObject = new ExpandoObject();
            return new DelegatingObject( _expandoObject );
        }

        [Fact]
        public void Indexing_should_create_the_member()
        {
            DynamicValue.foo = 5;
            Assert.Equal( 5, _expandoObject.foo );
        }

        [Fact]
        public void Indexing_should_get_the_member()
        {
            _expandoObject.foo = 5;
            Assert.Equal( 5, DynamicValue.foo );
        }
    }
}