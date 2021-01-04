using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Date    2021/1/1 18:54:20
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public static class CommandManager
    {
        private static Dictionary<string, BaseCommand> g_commandMap = new Dictionary<string, BaseCommand>();

        public static void RegisterCommand(BaseCommand command)
        {
            var commandTypeStr = command.GetType().ToString();
            if (g_commandMap.ContainsKey(commandTypeStr))
            {
                Debug.LogError($"command:{commandTypeStr} has registered");
                return;
            }

            g_commandMap.Add(commandTypeStr, command);
        }

        public static void ExcuteCommand(string commandName, params object[] param)
        {
            if (!g_commandMap.TryGetValue(commandName, out var command))
            {
                Debug.LogError($"command:{commandName} nerver registered");
                return;
            }

            command.Excute(param);
        }
    }

    public abstract class BaseCommand
    {
        public abstract void Excute(params object[] param);
    }



}