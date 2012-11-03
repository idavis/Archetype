#region Using Directives

using Archetype.Tests.TestObjects;
using NUnit.Framework;

#endregion

namespace Archetype.Tests
{
    [TestFixture]
    [Timeout( Constants.TestTimeOutInMs )]
    public class PrototypalObjectTestsInheritanceOrder
    {
        [Test]
        public void The_parent_target_should_be_executed()
        {
            dynamic dude = new TheDude( new Person() );
            Assert.AreEqual( "The Dude", dude.Name );
        }
    }
}