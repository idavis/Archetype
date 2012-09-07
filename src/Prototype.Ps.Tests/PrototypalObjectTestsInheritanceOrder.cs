#region Using Directives

using NUnit.Framework;
using Prototype.Ps.Tests.TestObjects;

#endregion

namespace Prototype.Ps.Tests
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