using System.ComponentModel;
using Archetype.Modules;
using NUnit.Framework;

namespace Archetype.Tests
{
    public class SampleModel : DelegatingObject
    {
        private string _Name;

        public SampleModel()
                : this( new NotifyPropertyChangedModule() )
        {
        }

        public SampleModel( params object[] prototypes )
                : base( prototypes )
        {
            // Or you can add it to the prototypes list after the fact
            //Modules.Add(new NotifyPropertyChangedModule());
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                if ( _Name == value )
                {
                    return;
                }
                _Name = value;
                This.OnPropertyChanged( "Name" );
            }
        }

        protected dynamic This
        {
            get { return this; }
        }
    }

    [TestFixture]
    public class NotifyPropertyChangesModuleTests
    {
        private bool called;
        private string propertyName = string.Empty;
        private dynamic instance;
        private INotifyPropertyChanged module;

        public void Initialize( params object[] prototypes )
        {
            instance = prototypes.Length == 0 ? new SampleModel() : new SampleModel( prototypes );
            module = instance;
            called = false;
            propertyName = null;
            module.PropertyChanged += ( sender, args ) =>
                                      {
                                          called = true;
                                          propertyName = args.PropertyName;
                                      };
        }

        private void AssertBehavior()
        {
            instance.Name = null;
            Assert.IsFalse( called );
            instance.Name = "Ian";
            Assert.IsTrue( called );
            Assert.AreEqual( "Ian", instance.Name );
            Assert.AreEqual( "Name", propertyName );
        }

        [Test]
        public void OnPropertyChangedCanBeCalledFromTheImportingClass()
        {
            Initialize();
            AssertBehavior();
        }

        [Test]
        public void OnPropertyChangedCanBeCalledFromTheImportingClassWhenThereIsAModuleChainEndingInTheTarget()
        {
            Initialize( 5, new NotifyPropertyChangedModule() );
            AssertBehavior();
        }

        [Test]
        public void OnPropertyChangedCanBeCalledFromTheImportingClassWhenThereIsAModuleChainWithTheTargetInTheBeginning()
        {
            Initialize( new NotifyPropertyChangedModule(), 5 );
            AssertBehavior();
        }

        [Test]
        public void OnPropertyChangedCanBeCalledFromTheImportingClassWhenThereIsAModuleChainWithTheTargetInTheMiddle()
        {
            Initialize( 10, new NotifyPropertyChangedModule(), 5 );
            AssertBehavior();
        }
    }
}