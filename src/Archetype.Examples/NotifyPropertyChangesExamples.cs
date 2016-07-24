#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System;
using System.ComponentModel;
using NUnit.Framework;

#endregion

namespace Archetype.Examples
{
    public interface INotifyPropertyChanges : INotifyPropertyChanged, INotifyPropertyChanging
    {
        void OnPropertyChanged( string propertyName = "" );
        void OnPropertyChanging( string propertyName = "" );
    }

    public class NotifyPropertyChangesModule : INotifyPropertyChanges
    {
        #region INotifyPropertyChanges Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event PropertyChangingEventHandler PropertyChanging = delegate { };

        public virtual void OnPropertyChanged( string propertyName = "" )
        {
            PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public virtual void OnPropertyChanging( string propertyName = "" )
        {
            PropertyChanging( this, new PropertyChangingEventArgs( propertyName ) );
        }

        #endregion
    }

    public class Person : DelegatingObject
    {
        private int _Age;
        private string _Name;

        public Person()
                : this( new NotifyPropertyChangesModule() )
        {
        }

        public Person( params object[] modules )
                : base( modules )
        {
        }

        public string Name
        {
            get { return _Name; }
            set
            {
                _this.OnPropertyChanging( "Name" );
                if ( _Name != value )
                {
                    _Name = value;
                    _this.OnPropertyChanged( "Name" );
                }
            }
        }

        public int Age
        {
            get { return _Age; }
            set
            {
                Inpc.OnPropertyChanging( "Age" );
                if ( _Age != value )
                {
                    _Age = value;
                    Inpc.OnPropertyChanged( "Age" );
                }
            }
        }

        internal INotifyPropertyChanges Inpc
        {
            get { return _this; }
        }
    }

    [Ignore("I don't remember")]
    [TestFixture]
    public class PersonExamples
    {
        [Test]
        public void SharedModulesToShareBehavior()
        {
            var module = new NotifyPropertyChangesModule();
            module.PropertyChanged +=
                    ( sender, args ) => Console.WriteLine( "The field {0} has changed.", args.PropertyName );
            module.PropertyChanging +=
                    ( sender, args ) => Console.WriteLine( "The field {0} is changing.", args.PropertyName );
            var inigo = new Person( module ) { Name = "Inigo" };
            var ian = new Person( module ) { Name = "Ian" };

            inigo.Age = 45;
            ian.Age = 30;
        }

        [Test]
        public void UsingModelObjectAsDynamic()
        {
            dynamic person = new Person();
            // The cast to the interface will work
            INotifyPropertyChanges inpc = person;
            inpc.PropertyChanged +=
                    ( sender, args ) => Console.WriteLine( "The field {0} has changed.", args.PropertyName );
            inpc.PropertyChanging +=
                    ( sender, args ) => Console.WriteLine( "The field {0} is changing.", args.PropertyName );
            // We have full IntelliSense when working with person.
            // but now accessing .Name looses IntelliSense
            person.Name = "Inigo Montoya";
        }

        [Test]
        public void UsingModelObjectAsStronglyTyped()
        {
            var person = new Person();
            // Casting first to dynamic triggers the DelegatingObjects casting system
            INotifyPropertyChanges inpc = (dynamic) person;
            inpc.PropertyChanged +=
                    ( sender, args ) => Console.WriteLine( "The field {0} has changed.", args.PropertyName );
            inpc.PropertyChanging +=
                    ( sender, args ) => Console.WriteLine( "The field {0} is changing.", args.PropertyName );
            // We have full IntelliSense when working with person.
            // We have full IntelliSense when working with person.
            person.Name = "Inigo Montoya";
        }

        [Test]
        public void UsingModelWithProxyCastingProperty()
        {
            var person = new Person();
            // The cast to the interface will work
            person.Inpc.PropertyChanged +=
                    ( sender, args ) => Console.WriteLine( "The field {0} has changed.", args.PropertyName );
            person.Inpc.PropertyChanging +=
                    ( sender, args ) => Console.WriteLine( "The field {0} is changing.", args.PropertyName );
            // We have full IntelliSense when working with person.
            // but now accessing .Name looses IntelliSense
            person.Name = "Inigo Montoya";
        }
    }
}