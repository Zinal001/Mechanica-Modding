using System;
using System.Collections.Generic;
using System.Text;

namespace ChatCommands
{
    public class ChatCommandEventArgs
    {
        public String Command { get; private set; }
        public String[] Arguments { get; private set; }

        public List<String> ResponseLines { get; private set; } = new List<string>();

        public bool Handled { get; set; } = false;



        internal ChatCommandEventArgs(String command, String[] arguments)
        {
            Command = command;
            Arguments = arguments;
        }

        public void AddResponse(String response)
        {
            ResponseLines.Add(response);
        }
    }
}
