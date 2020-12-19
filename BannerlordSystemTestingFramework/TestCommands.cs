using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
