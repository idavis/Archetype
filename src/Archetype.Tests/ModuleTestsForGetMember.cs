using System;
using Xunit;

namespace Archetype.Tests
{
    public class Animal
    {
        public virtual string Name
        {
            get { return GetType().Name; }
        }
    }

    public class Cat : Animal
    {
        public string Noise
        {
            get { return "Meow"; }
        }
    }

    public class CheshireCat : DelegatingObject
    {
        public CheshireCat( Cat cat = null )
                : base( cat )
        {
        }

        public string Action
        {
            get { return "Grin"; }
        }
    }

    public class ModuleTestsForGetMember
    {
        [Fact]
        public void GettingAPropertyThatIsDefinedInTheRootObjectReturnsItsValue()
        {
            dynamic value = new CheshireCat( new Cat() );
            Assert.Equal( "Grin", value.Action );
        }

        [Fact]
        public void GettingAPropertyThatIsDefinedInTheRootObjectReturnsItsValueWhenThereIsNoBaseModule()
        {
            dynamic value = new CheshireCat();
            Assert.Equal( "Grin", value.Action );
        }

        [Fact]
        public void GettingAPropertyThatIsNotDefinedInTheRootObjectReturnsItsValueAndIsReturnedBottomUp()
        {
            dynamic value = new CheshireCat( new Cat() );
            value.Modules.Add( new Animal() );
            Assert.Equal( "Animal", value.Name );
        }

        [Fact]
        public void GettingAPropertyThatIsNotDefinedInTheRootObjectReturnsItsValueIfItIsNotOnTheLastModule()
        {
            dynamic value = new CheshireCat( new Cat() );
            Assert.Equal( "Meow", value.Noise );
        }

        [Fact]
        public void GettingAPropertyThatIsNotDefinedInTheRootObjectReturnsItsValueIfItIsOnTheLastModule()
        {
            dynamic value = new CheshireCat( new Cat() );
            Assert.Equal( "Cat", value.Name );
        }

        [Fact]
        public void TestingChainOfModules()
        {
            dynamic value = new DelegatingObject( 5, "cat", 10, new Uri( "http://www.foo.com" ) );
            Assert.Equal( 3, value.Length );
        }
    }
}