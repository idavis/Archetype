using System.Dynamic;

namespace Prototype.Ps.Tests
{
    public class MethodHolder : PrototypalObject
    {
        public MethodHolder():this(null){}
        public MethodHolder(DynamicObject prototype)
            : base(prototype)
        {
        }

        public bool MethodWithNoReturnValueOrParametersWasCalled;
        public bool MethodWithNoReturnValueSingleParameterWasCalled;
        public int MethodWithNoReturnValueSingleParameterValue;
        public bool MethodWithReturnValueNoParametersWasCalled;
        public bool MethodWithReturnValueSingleParameterWasCalled;

        public void MethodWithNoReturnValueOrParameters()
        {
            MethodWithNoReturnValueOrParametersWasCalled = true;
        }

        public void MethodWithNoReturnValueSingleParameter(int value)
        {
            MethodWithNoReturnValueSingleParameterWasCalled = true;
            MethodWithNoReturnValueSingleParameterValue = value;
        }

        public int MethodWithReturnValueNoParameters()
        {
            MethodWithReturnValueNoParametersWasCalled = true;
            return 42;
        }

        public int MethodWithReturnValueSingleParameter(int value)
        {
            MethodWithReturnValueSingleParameterWasCalled = true;
            return value;
        }

        public void MethodWithOutParameter(out int foo)
        {
            foo = 42;
        }

        public void MethodWithRefParameter(ref int foo)
        {
            foo = 42;
        }
    }
}