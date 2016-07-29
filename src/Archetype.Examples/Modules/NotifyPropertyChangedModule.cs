#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

#region Using Directives

using System.ComponentModel;

#endregion

namespace Archetype.Modules
{
    public class NotifyPropertyChangedModule : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        public virtual void OnPropertyChanged( string propertyName )
        {
            PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
        }
    }
}