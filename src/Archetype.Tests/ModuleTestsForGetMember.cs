using System;
using NUnit.Framework;

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

    public class CheshireCat : DelegatingPrototype
    {
        public CheshireCat(Cat cat = null) : base(cat)
        {
        }

        public string Action
        {
            get { return "Grin"; }
        }
    }

    [TestFixture]
    public class ModuleTestsForGetMember
    {
        [Test]
        public void GettingAPropertyThatIsDefinedInTheRootObjectReturnsItsValue()
        {
            dynamic value = new CheshireCat(new Cat());
            Assert.AreEqual("Grin", value.Action);
        }

        [Test]
        public void GettingAPropertyThatIsDefinedInTheRootObjectReturnsItsValueWhenThereIsNoPrototype()
        {
            dynamic value = new CheshireCat();
            Assert.AreEqual("Grin", value.Action);
        }

        [Test]
        public void GettingAPropertyThatIsNotDefinedInTheRootObjectReturnsItsValueAndIsReturnedBottomUp()
        {
            dynamic value = new CheshireCat(new Cat());
            value.Modules.Add(new Animal());
            Assert.AreEqual("Animal", value.Name);
        }

        [Test]
        public void GettingAPropertyThatIsNotDefinedInTheRootObjectReturnsItsValueIfItIsNotOnTheLastModule()
        {
            dynamic value = new CheshireCat(new Cat());
            Assert.AreEqual("Meow", value.Noise);
        }

        [Test]
        public void GettingAPropertyThatIsNotDefinedInTheRootObjectReturnsItsValueIfItIsOnTheLastModule()
        {
            dynamic value = new CheshireCat(new Cat());
            Assert.AreEqual("Cat", value.Name);
        }

        [Test]
        public void TestingChainOfModules()
        {
            dynamic value = new DelegatingPrototype(5, "cat", 10, new Uri("http://www.foo.com"));
            Assert.AreEqual(3, value.Length);
        }

    }
}