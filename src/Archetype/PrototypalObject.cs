#region License

// 
// Copyright (c) 2012, Ian Davis
// 
// Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
// See the file LICENSE.txt for details.
// 

#endregion

namespace Archetype
{
    public class PrototypalObject : PrototypalObject<dynamic>
    {
        public PrototypalObject( object prototype )
                : base( prototype )
        {
        }
    }
}