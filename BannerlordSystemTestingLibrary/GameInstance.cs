using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BannerlordSystemTestingLibrary
{
    public class GameInstance : IDisposable
    {
        GameProcess process;

        public event Action<string> OnGameStateChanged;
        public event Action<string> OnCommandResponse;
        public bool Running => process.Running;
        public int PID => process.GetPID();
        
        public Message PIDMsg
        {
            get { return _PIDMsg; } 
            set
            {
                if (_PIDMsg == null)
                {
                    _PIDMsg = value;
                }
                else
                {
                    throw new Exception("Client already assigned");
                }
            } 
        }

        public GameInstance(string args)
        {
            process = new GameProcess(args);
        }

        public GameInstance(Process p)
        {
            process = new GameProcess(p);
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            process.Start();

            // Wait for menu to be ready
        }

        public void Stop()
        {
            process.Kill();
        }

        public void SendCommand(string command)
        {
            if (attached)
            {
                PIDMsg.ReplyLine($"COMMAND {command}");
            }
        }

        public void SendCommand(string command, string[] args)
        {
            if (attached)
            {
                PIDMsg.ReplyLine($"COMMAND {command}%\"{string.Join("%\"", args)}%\"");
            }
        }

        public void ProcessMessage(Message e)
        {
            if (e.MessageString.StartsWith("GAME_STATE "))
            {
                ParseGameState(e);
            }
            else if (e.MessageString.StartsWith("COMMAND_RETURN "))
            {
                ParseCommandReturn(e);
            }
        }

        #region private
        private void ParseGameState(Message e)
        {
            string gameState = e.MessageString.Remove(0, "GAME_STATE ".Length);

            OnGameStateChanged?.Invoke(gameState);
        }

        private void ParseCommandReturn(Message e)
        {
            string returnParam = e.MessageString.Remove(0, "COMMAND_RETURN ".Length);

            OnCommandResponse?.Invoke(returnParam);
        }

        private Message _PIDMsg;
        private bool attached => _PIDMsg != null;
        #endregion
    }
}
