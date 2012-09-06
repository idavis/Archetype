#region Using Directives

using System.Dynamic;
using NUnit.Framework;

#endregion

namespace Prototype.Ps.Tests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsExpandoObjectPrototypes : PrototypalObjectTests
    {
        protected override PrototypalObject Create()
        {
            return new PrototypalObject( new ExpandoObject() );
        }

        [Test]
        public void Indexing_should_create_the_member()
        {
            // This currently creates an infinite loop.
            DynamicValue.foo = 5;
            Assert.AreEqual( 5, DynamicValue.foo );
        }
    }
}