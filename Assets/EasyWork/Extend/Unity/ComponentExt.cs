using System.Collections.Generic;
using UnityEngine;

namespace Extend.Unity
{
    /// <summary>
    /// Date    2021/1/6 14:45:28
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public static class ComponentExt
    {
        private static List<Component> g_coms = new List<Component>();

        public static T GetComponentNoAllot<T>(this Component @this) where T : Component
        {
            @this.GetComponents(typeof(T), g_coms);
            if(g_coms.Count > 0)
            {
                return g_coms[0] as T;
            }

            return default;
        }
    }
}