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

namespace Archetype
{
    public class NotifyPropertyChangingModule : INotifyPropertyChanging
    {
        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        public virtual void OnPropertyChanging( string propertyName )
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if ( handler != null )
            {
                handler( this, new PropertyChangingEventArgs( propertyName ) );
            }
        }
    }
}