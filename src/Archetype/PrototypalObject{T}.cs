using System.Dynamic;
using System.Linq.Expressions;

namespace Archetype
{
    public class PrototypalObject<T> : DynamicMetaObjectProviderBase
    {
        public PrototypalObject( T prototype )
        {
            Prototype = prototype;
        }

        public T Prototype { get; set; }

        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            DynamicMetaObject baseMetaObject = base.GetMetaObject( parameter );

            if ( ReferenceEquals( Prototype, null ) )
            {
                return baseMetaObject;
            }

            return new ModuleMetaObject( parameter, this, baseMetaObject, new object[] { Prototype } );
        }
    }
}