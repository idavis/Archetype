using System.ComponentModel;
using NUnit.Framework;

namespace Archetype.Tests
{
    public class NotifyPropertyChangedModule : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SampleModel : DelegatingPrototype
    {
        private string _Name;

        public SampleModel()
            : base(new NotifyPropertyChangedModule())
        {
            // Or you can add it to the prototypes list after the fact
            //Prototypes.Add(new NotifyPropertyChangedModule());
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                This.OnPropertyChanged("Name");
            }
        }

        protected dynamic This { get { return this; } }
    }

    [TestFixture]
    public class NotifyPropertyChangesModuleTests
    {
        [Test]
        public void OnPropertyChangedCanBeCalledFromTheImportingClass()
        {
            dynamic instance = new SampleModel();
            INotifyPropertyChanged module = instance; // fancy casting of our object will return the module
            bool called = false;
            string propertyName = null;
            module.PropertyChanged += (sender, args) =>
                                          {
                                              called = true;
                                              propertyName = args.PropertyName;
                                          };
            instance.Name = null;
            Assert.IsFalse(called);
            instance.Name = "Ian";
            Assert.IsTrue(called);
            Assert.AreEqual("Ian", instance.Name);
            Assert.AreEqual("Name", propertyName);
        }
    }
}