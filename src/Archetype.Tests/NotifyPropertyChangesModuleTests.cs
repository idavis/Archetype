using System.ComponentModel;
using Archetype.Modules;
using Xunit;

namespace Archetype.Tests
{
    public class SampleModel : DelegatingObject
    {
        private string _Name;

        public SampleModel()
                : this( new NotifyPropertyChangedModule() )
        {
        }

        public SampleModel( params object[] modules )
                : base( modules )
        {
            // Or you can add it to the modules list after the fact
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
                _this.OnPropertyChanged( "Name" );
            }
        }
    }
    /*
    public class NotifyPropertyChangesModuleTests
    {
        private bool _Called;
        private string _PropertyName = string.Empty;
        private dynamic _Instance;
        private INotifyPropertyChanged _Module;

        public void Initialize( params object[] modules )
        {
            _Instance = modules.Length == 0 ? new SampleModel() : new SampleModel( modules );
            _Module = _Instance;
            _Called = false;
            _PropertyName = null;
            _Module.PropertyChanged += ( sender, args ) =>
                                      {
                                          _Called = true;
                                          _PropertyName = args.PropertyName;
                                      };
        }

        private void AssertBehavior()
        {
            _Instance.Name = null;
            Assert.False( _Called );
            _Instance.Name = "Ian";
            Assert.True( _Called );
            Assert.Equal( "Ian", _Instance.Name );
            Assert.Equal( "Name", _PropertyName );
        }

        [Fact]
        public void OnPropertyChangedCanBeCalledFromTheImportingClass()
        {
            Initialize();
            AssertBehavior();
        }

        [Fact]
        public void OnPropertyChangedCanBeCalledFromTheImportingClassWhenThereIsAModuleChainEndingInTheTarget()
        {
            Initialize( 5, new NotifyPropertyChangedModule() );
            AssertBehavior();
        }

        [Fact]
        public void OnPropertyChangedCanBeCalledFromTheImportingClassWhenThereIsAModuleChainWithTheTargetInTheBeginning()
        {
            Initialize( new NotifyPropertyChangedModule(), 5 );
            AssertBehavior();
        }

        [Fact]
        public void OnPropertyChangedCanBeCalledFromTheImportingClassWhenThereIsAModuleChainWithTheTargetInTheMiddle()
        {
            Initialize( 10, new NotifyPropertyChangedModule(), 5 );
            AssertBehavior();
        }
    }
    */
}