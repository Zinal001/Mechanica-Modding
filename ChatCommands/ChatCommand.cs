using System;
using System.Collections.Generic;
using System.Text;

namespace ChatCommands
{
    public abstract class ChatCommand
    {
        public abstract String Name { get; }
        public abstract String Command { get; }
        public virtual String Description { get; }
        public virtual String Syntax { get; }

        public abstract void OnHandle(ChatCommandEventArgs e);
    }
}
