#region Using Directives

using System.Dynamic;
using NUnit.Framework;

#endregion

namespace Archetype.Tests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsExpandoObjectModules : DelegatingObjectTests
    {
        private dynamic _expandoObject;

        protected override DelegatingObject Create()
        {
            _expandoObject = new ExpandoObject();
            return new DelegatingObject( _expandoObject );
        }

        [Test]
        public void Indexing_should_create_the_member()
        {
            DynamicValue.foo = 5;
            Assert.AreEqual( 5, _expandoObject.foo );
        }

        [Test]
        public void Indexing_should_get_the_member()
        {
            _expandoObject.foo = 5;
            Assert.AreEqual( 5, DynamicValue.foo );
        }
    }
}