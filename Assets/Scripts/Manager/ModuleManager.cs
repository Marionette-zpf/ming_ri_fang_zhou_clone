using Module;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Date    2021/1/1 18:52:57
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>

    public static class ModuleManager
    {
        private static Dictionary<Type, BaseModule> g_moduleMap = new Dictionary<Type, BaseModule>();

        public static T GetModule<T>() where T : BaseModule
        {
            var moduleType = typeof(T);

            if (!g_moduleMap.TryGetValue(typeof(T), out var module))
            {
                Debug.LogError($"module:{moduleType} never registered");
            }

            return module as T;
        }

        public static void RegisterModule(Type moduleType)
        {
            var baseModuleType = typeof(BaseModule);
            if (baseModuleType.IsAssignableFrom(moduleType))
            {
                RegisterModule(Activator.CreateInstance(moduleType) as BaseModule);
            }
        }

        public static void RegisterModule(BaseModule module)
        {
            var moduleType = module?.GetType();

            if (g_moduleMap.ContainsKey(moduleType))
            {
                Debug.LogError($"module:{moduleType} has been registered");
                return;
            }

            module.Init();
            g_moduleMap.Add(moduleType, module);
        }

        public static void UnRegisterModule(BaseModule module)
        {
            UnRegisterModule(module?.GetType());
        }

        public static void UnRegisterModule(Type moduleType)
        {

            if (!g_moduleMap.TryGetValue(moduleType, out var module))
            {
                Debug.LogError($"module:{moduleType} never registered");
                return;
            }

            module.Dispose();
            g_moduleMap.Remove(moduleType);
        }
    }
}