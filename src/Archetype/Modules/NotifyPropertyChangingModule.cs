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
    public class NotifyPropertyChangingModule : INotifyPropertyChanging
    {
        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging = delegate { };

        #endregion

        public virtual void OnPropertyChanging( string propertyName )
        {
            PropertyChanging( this, new PropertyChangingEventArgs( propertyName ) );
        }
    }
}