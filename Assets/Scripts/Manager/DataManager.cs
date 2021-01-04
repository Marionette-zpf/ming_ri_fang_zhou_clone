using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Binder;

namespace Manager
{
    /// <summary>
    /// Date    2021/1/2 21:47:44
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public static partial class DataManager
    {
        private static Dictionary<string, DataBinder> g_dataBinderMap = new Dictionary<string, DataBinder>();

        public static T Get<T>(string key)
        {
            if(g_dataBinderMap.TryGetValue(key, out var dataBinder))
            {
                return (dataBinder as DataBinder<T>).Get();
            }

            return default;
        }

        public static void Register<T>(string key, Func<T> dataProvier)
        {
            if (g_dataBinderMap.ContainsKey(key))
            {
                return;
            }

            g_dataBinderMap.Add(key, new DataBinder<T>(dataProvier));
        }

        public static void Register<T>(string key, DataBinder<T> dataBinder)
        {
            if (g_dataBinderMap.ContainsKey(key))
            {
                return;
            }

            g_dataBinderMap.Add(key, dataBinder);
        }

    }

    public static partial class DataManager
    {
        public static T GetFromPlayerPrefs<T>(string key)
        {
            var strData = PlayerPrefs.GetString(key, string.Empty);
            return JsonMapper.ToObject<T>(strData);
        }
        public static void Save2PlayerPrefs<T>(string key, T data)
        {
            PlayerPrefs.SetString(key, JsonMapper.ToJson(data));
        }
    }
}