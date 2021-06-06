using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatCommands
{
    public static class ChatSystem
    {
        private static Dictionary<String, ChatCommand> _RegisteredCommands = new Dictionary<String, ChatCommand>();


        static ChatSystem()
        {
            Register(new Commands.HelpCommand());
        }

        public static bool Register(ChatCommand command)
        {
            if (_RegisteredCommands.ContainsKey(command.Name))
                return false;

            _RegisteredCommands[command.Name] = command;
            return true;
        }

        public static int Register(IEnumerable<ChatCommand> commands)
        {
            int numRegistered = 0;
            foreach(ChatCommand cmd in commands)
            {
                if (Register(cmd))
                    numRegistered++;
            }

            return numRegistered;
        }

        public static int RegisterAll(System.Reflection.Assembly assembly)
        {
            Type[] chatCommandTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ChatCommand)) && t.IsPublic && !t.IsAbstract && !t.IsInterface).ToArray();

            List<ChatCommand> commands = new List<ChatCommand>();
            foreach(Type cmdType in chatCommandTypes)
            {
                try
                {
                    ChatCommand cc = (ChatCommand)Activator.CreateInstance(cmdType);
                    if (cc != null)
                        commands.Add(cc);
                }
                catch { }
            }

            if (commands.Any())
                return Register(commands);

            return 0;
        }

        public static void Overwrite(ChatCommand command)
        {
            _RegisteredCommands[command.Name] = command;
        }

        public static void Overwrite(IEnumerable<ChatCommand> commands)
        {
            foreach (ChatCommand cmd in commands)
                Overwrite(cmd);
        }


        public static void OverwriteAll(System.Reflection.Assembly assembly)
        {
            Type[] chatCommandTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ChatCommand)) && t.IsPublic && !t.IsAbstract && !t.IsInterface).ToArray();

            List<ChatCommand> commands = new List<ChatCommand>();
            foreach (Type cmdType in chatCommandTypes)
            {
                try
                {
                    ChatCommand cc = (ChatCommand)Activator.CreateInstance(cmdType);
                    if (cc != null)
                        commands.Add(cc);
                }
                catch { }
            }

            if (commands.Any())
                Overwrite(commands);
        }

        internal static ChatCommand[] GetCommands()
        {
            return _RegisteredCommands.Values.ToArray();
        }

        internal static ChatCommandEventArgs HandleCommand(String command, String[] arguments)
        {
            ChatCommandEventArgs e = new ChatCommandEventArgs(command, arguments);

            foreach(ChatCommand cmd in _RegisteredCommands.Values)
            {
                try
                {
                    cmd.OnHandle(e);
                    if (e.Handled)
                        break;
                }
                catch(Exception ex)
                {
                    UnityEngine.Debug.LogWarning($"Failed to process command '{command}' with handler '{cmd.Name}': {ex.Message}\n{ex.StackTrace}");
                }
            }

            return e;
        }
    }
}
