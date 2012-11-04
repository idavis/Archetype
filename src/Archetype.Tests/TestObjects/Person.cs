using System.Dynamic;

namespace Archetype.Tests.TestObjects
{
    public class Person : DelegatingObject
    {
        public Person()
        {
            Name = "Ian";
        }

        public string Name { get; set; }
    }

    public class TheDude : DelegatingObject
    {
        public TheDude( IDynamicMetaObjectProvider module )
                : base( module )
        {
        }

        public string Name
        {
            get { return "The Dude"; }
        }
    }
}