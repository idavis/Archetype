#region Using Directives

using System;
using System.Threading.Tasks;
using System.Reflection;
using Xunit;

#endregion

namespace Archetype.Tests
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TimeoutAttribute : Attribute
    {
        public TimeSpan Timeout { get; private set; }

        public TimeoutAttribute(int milliseconds)
        {
            Timeout = TimeSpan.FromMilliseconds(milliseconds);
        }
    }

    public static class TimeoutRunner
    {
        public static void Execute(object owner, Func<Task> action)
        {
            var timeout = owner.GetType().GetTypeInfo().GetCustomAttribute<TimeoutAttribute>().Timeout;
            var result = Task.WaitAny(new[] { action() }, (int)timeout.TotalMilliseconds);
            Assert.True(result == 0, $"The test failed due to a timeout after {timeout}.");
        }
    }
}
