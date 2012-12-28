using System.Dynamic;
using System.Linq.Expressions;

namespace Archetype
{
    public abstract class DynamicMetaObjectProviderBase : DynamicObject
    {
        protected virtual dynamic _this
        {
            get { return this; }
        }

        public override DynamicMetaObject GetMetaObject( Expression parameter )
        {
            DynamicMetaObject baseMetaObject = GetBaseMetaObject( parameter );
            return baseMetaObject;
        }

        protected virtual DynamicMetaObject GetBaseMetaObject( Expression parameter )
        {
            return base.GetMetaObject( parameter );
        }
    }
}