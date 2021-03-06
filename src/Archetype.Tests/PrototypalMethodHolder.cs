using System.Dynamic;

namespace Archetype.Tests
{
    public class ObjectMethodHolder
    {
        public static bool StaticMethodWithNoReturnValueOrParametersWasCalled;
        public bool MethodWithNoReturnValueOrParametersWasCalled;
        public int MethodWithNoReturnValueSingleParameterValue;
        public bool MethodWithNoReturnValueSingleParameterWasCalled;
        public bool MethodWithReturnValueNoParametersWasCalled;
        public bool MethodWithReturnValueSingleParameterWasCalled;

        public ObjectMethodHolder()
        {
            StaticMethodWithNoReturnValueOrParametersWasCalled = false;
        }

        public static void StaticMethodWithNoReturnValueOrParameters()
        {
            StaticMethodWithNoReturnValueOrParametersWasCalled = true;
        }

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


    public class DynamicMethodHolder : DynamicObject
    {
        public static bool StaticMethodWithNoReturnValueOrParametersWasCalled;
        public bool MethodWithNoReturnValueOrParametersWasCalled;
        public int MethodWithNoReturnValueSingleParameterValue;
        public bool MethodWithNoReturnValueSingleParameterWasCalled;
        public bool MethodWithReturnValueNoParametersWasCalled;
        public bool MethodWithReturnValueSingleParameterWasCalled;

        public DynamicMethodHolder()
        {
            StaticMethodWithNoReturnValueOrParametersWasCalled = false;
        }

        public static void StaticMethodWithNoReturnValueOrParameters()
        {
            StaticMethodWithNoReturnValueOrParametersWasCalled = true;
        }

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

    public class PrototypalMethodHolder : PrototypalObject
    {
        public static bool StaticMethodWithNoReturnValueOrParametersWasCalled;
        public bool MethodWithNoReturnValueOrParametersWasCalled;
        public int MethodWithNoReturnValueSingleParameterValue;
        public bool MethodWithNoReturnValueSingleParameterWasCalled;
        public bool MethodWithReturnValueNoParametersWasCalled;
        public bool MethodWithReturnValueSingleParameterWasCalled;

        public PrototypalMethodHolder() : this(null)
        {
        }

        public PrototypalMethodHolder(DynamicObject prototype)
            : base(prototype)
        {
            StaticMethodWithNoReturnValueOrParametersWasCalled = false;
        }

        public static void StaticMethodWithNoReturnValueOrParameters()
        {
            StaticMethodWithNoReturnValueOrParametersWasCalled = true;
        }

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