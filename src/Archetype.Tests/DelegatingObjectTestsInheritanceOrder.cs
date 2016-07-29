#region Using Directives

using Archetype.Tests.TestObjects;
using Xunit;

#endregion

namespace Archetype.Tests
{




    [Timeout( Constants.TestTimeOutInMs )]
    public class DelegatingObjectTestsInheritanceOrder
    {
        [Fact]
        public void The_parent_target_should_be_executed()
        {
            dynamic dude = new TheDude( new Person() );
            Assert.Equal( "The Dude", dude.Name );
        }
    }
}