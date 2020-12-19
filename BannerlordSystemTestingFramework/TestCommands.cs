using System;
using System.Collections.Generic;
using System.Reflection;

namespace ModTestingFramework
{
    class CommandRegisterException : Exception
    {
        public CommandRegisterException(string message) : base(message)
        {
        }
    }
    public abstract class TestCommands
    {
        protected static Dictionary<string, MethodInfo> commands = new Dictionary<string, MethodInfo>();
    }
}
