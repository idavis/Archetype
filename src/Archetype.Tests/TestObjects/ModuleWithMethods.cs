#region Using Directives

using System.Dynamic;

#endregion

namespace Archetype.Tests.TestObjects
{
    public class ModuleWithMethods : DelegatingObject
    {
        public static bool StaticMethodWithNoReturnValueOrParametersWasCalled;
        public bool MethodWithNoReturnValueOrParametersWasCalled;
        public int MethodWithNoReturnValueSingleParameterValue;
        public bool MethodWithNoReturnValueSingleParameterWasCalled;
        public bool MethodWithReturnValueNoParametersWasCalled;
        public bool MethodWithReturnValueSingleParameterWasCalled;

        public ModuleWithMethods()
                : this( null )
        {
        }

        public ModuleWithMethods( IDynamicMetaObjectProvider module )
                : base( module )
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

        public void MethodWithNoReturnValueSingleParameter( int value )
        {
            MethodWithNoReturnValueSingleParameterWasCalled = true;
            MethodWithNoReturnValueSingleParameterValue = value;
        }

        public int MethodWithReturnValueNoParameters()
        {
            MethodWithReturnValueNoParametersWasCalled = true;
            return 42;
        }

        public int MethodWithReturnValueSingleParameter( int value )
        {
            MethodWithReturnValueSingleParameterWasCalled = true;
            return value;
        }

        public void MethodWithOutParameter( out int foo )
        {
            foo = 42;
        }

        public void MethodWithRefParameter( ref int foo )
        {
            foo = 42;
        }
    }
}