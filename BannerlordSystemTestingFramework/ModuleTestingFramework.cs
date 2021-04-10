using HarmonyLib;
using SimpleTCP;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TaleWorlds.MountAndBlade;

namespace ModTestingFramework
{
    public class TestingFramework
    {
        public TestingFramework()
        {
            Harmony harmony = new Harmony("com.TaleWorlds.MountAndBlade.Bannerlord.TestEnv");

            if (communicator.Connected)
            {
                communicator.OnMessageReceived += Communicator_OnMessageReceived;
                communicator.SendData($"PID {Process.GetCurrentProcess().Id}");
                
                CommandRegistry.RegisterAllCommands();

                StateEvents.OnStateActivate += (state) => { communicator.SendData($"GAME_STATE {state}"); };

                // Remove patch after main menu becomes selectable
                StateEvents.OnMainMenuReady += () => { 
                    harmony.Unpatch(typeof(MBInitialScreenBase)
                        .GetMethod("OnFrameTick", BindingFlags.NonPublic | BindingFlags.Instance), 
                        HarmonyPatchType.Postfix); };
                StateEvents.OnMainMenuReady += () => { communicator.SendData($"GAME_STATE MainMenuReady"); };
            }
            
            harmony.PatchAll();
        }

        public void SendMessage(string message)
        {
            if (communicator.Connected)
            {
                communicator.SendData(message);
            }
        }

        private void Communicator_OnMessageReceived(object sender, Message msg)
        {
            if(msg.MessageString.StartsWith("COMMAND "))
            {
                ParseCommandAndActivate(msg);
            }
        }

        private void ParseCommandAndActivate(Message message)
        {
            string msg = message.MessageString;
            // COMMAND StartGame [args]
            string formattedMsg = msg.Remove(0, "COMMAND ".Length);
            string[] splitArray = formattedMsg.Split(new string[] { "%\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] args = splitArray.Skip(1).ToArray();
            string command = splitArray.First();

            if (!CommandRegistry.commands.ContainsKey(command)) 
            {
                throw new Exception("Command does not exist in command registry.");
            }

            MethodInfo methodInfo = CommandRegistry.commands[command];

            object returnParam;

            if (methodInfo.GetParameters().Length == 0)
            {
                returnParam = methodInfo.Invoke(null, null);
            }
            else if(methodInfo.GetParameters().Length == 1 &&
                methodInfo.GetParameters()[0].ParameterType == typeof(string[]))
            {
                returnParam = methodInfo.Invoke(null, new object[] { args });
            }
            else
            {
                throw new Exception($"{methodInfo.Name} does not meet generic requirements.");
            }

            if (returnParam != null &&
                returnParam?.GetType() == typeof(string))
            {
                message.ReplyLine($"COMMAND_RETURN {returnParam}");
            }
        }

        #region Private
        private static RunnerCommunicator communicator = RunnerCommunicator.Instance;
        #endregion

        #region Singleton
        public static TestingFramework Instance { get; } = new TestingFramework();
        #endregion
    }
}
