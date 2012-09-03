namespace Prototype.Ps
{
    using System;
    using System.Dynamic;
    
    public delegate bool TryInvokeMissingMemberCallback(InvokeMemberBinder binder, object[] args, out object result);
    public delegate bool TryInvokeMissingGetMemberCallback(GetMemberBinder binder, out object result);
    public delegate bool TryInvokeMissingSetMemberCallback(SetMemberBinder binder, object value);
    
    public class Prototype : DynamicObject
    {
        public Prototype()
          :this(new DynamicObject())
        {
        }

        public Prototype(DynamicObject prototype)
        {
            Prototype = prototype;
        }

        public DynamicObject Prototype { get; set; }
        public TryInvokeMissingMemberCallback TryInvokeMissingMember { get; set; }
        public TryInvokeMissingGetMemberCallback TryInvokeMissingGetMember { get; set; }
        public TryInvokeMissingSetMemberCallback TryInvokeMissingSetMember { get; set; }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if(base.TryInvokeMember(binder, args, out result))
            {
                return true;
            }
            if(!ReferenceEquals(Prototype, null))
            {
                if(Prototype.TryInvokeMember(binder, args, out result))
                {
                    return true;
                }
            }
                if (TryInvokeMissingMember == null)
                { 
                    return false;
                }
                return TryInvokeMissingMember(binder, args, out result);
            }
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!base.TryGetMember(binder, out result))
            {
                if (TryInvokeMissingGetMember == null)
                {
                    return false;
                }
                return TryInvokeMissingGetMember(binder, out result);
            }
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!base.TrySetMember(binder, value))
            {
                if (TryInvokeMissingMember == null)
                {
                    return false;
                }
                return TryInvokeMissingSetMember( binder, value );
            }
            return true;
        }
    }
}