#region Using Directives

using System.Dynamic;
using System.Linq.Expressions;

#endregion

namespace Archetype
{
    public class DelegatingPrototype : DynamicObject
    {
        public DelegatingPrototype()
                : this( null )
        {
        }

        public DelegatingPrototype( object prototype )
        {
            Prototype = prototype;
        }

        public virtual object Prototype { get; set; }

        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            if ( Prototype == null )
            {
                return GetBaseMetaObject( parameter );
            }
            return new PrototypalMetaObject( parameter, this, Prototype );
        }

        public virtual DynamicMetaObject GetBaseMetaObject( Expression parameter )
        {
            return base.GetMetaObject( parameter );
        }
    }
}