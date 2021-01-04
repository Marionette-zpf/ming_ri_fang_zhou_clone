using Manager;
using System;
using System.Collections.Generic;
using Utilities.Binder;

namespace Module
{
    /// <summary>
    /// Date    2020/12/29 17:03:52
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public abstract partial class BaseModule : IDisposable
    {
        public virtual bool InitOnGameEntrance => true;

        public void Init()
        {
            OnInit();
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnInit()
        {

        }

        protected virtual void OnDispose()
        {

        }
    }

    public abstract partial class BaseModule
    {
        public virtual void Injiect(string method, Action action)
        {

        }

        public virtual void Injiect<T1>(string method, Action<T1> action)
        {

        }

        public virtual void Injiect<T1, T2>(string method, Action<T1, T2> action)
        {

        }

        public virtual void Injiect<T1, T2, T3>(string method, Action<T1, T2, T3> action)
        {

        }

        public virtual void Extract(string method, out Action action)
        {
            action = default;
        }

        public virtual void Extract<T1>(string method, out Action<T1> action)
        {
            action = default;
        }

        public virtual void Extract<T1, T2>(string method, out Action<T1, T2> action)
        {
            action = default;
        }

        public virtual void Extract<T1, T2, T3>(string method, out Action<T1, T2, T3> action)
        {
            action = default;
        }

        public virtual void Injiect<T1>(string method, Func<T1> action)
        {

        }

        public virtual void Injiect<T1, T2>(string method, Func<T1, T2> action)
        {

        }

        public virtual void Injiect<T1, T2, T3>(string method, Func<T1, T2, T3> action)
        {

        }

        public virtual void Injiect<T1, T2, T3, T4>(string method, Func<T1, T2, T3, T4> action)
        {

        }

        public virtual void Extract<T1>(string method, out Func<T1> action)
        {
            action = default;
        }

        public virtual void Extract<T1, T2>(string method, out Func<T1, T2> action)
        {
            action = default;
        }

        public virtual void Extract<T1, T2, T3>(string method, out Func<T1, T2, T3> action)
        {
            action = default;
        }

        public virtual void Extract<T1, T2, T3, T4>(string method, out Func<T1, T2, T3, T4> action)
        {
            action = default;
        }
    }

    public abstract partial class BaseModule
    {
        private Dictionary<string, DataBinder> m_binderMap = new Dictionary<string, DataBinder>();

        public void GetData<T>(string key, out T data)
        {
            if (m_binderMap.TryGetValue(key, out var dataBinder))
            {
                data = (dataBinder as DataBinder<T>).Get();
            }
            else
            {
                data = default;
            }
        }

        protected void Register<T>(string key, Func<T> dataProvider, bool goal = false)
        {
            if (m_binderMap.ContainsKey(key))
            {
                return;
            }

            var dataBinder = new DataBinder<T>(dataProvider);

            if (goal)
            {
                DataManager.Register(key, dataBinder);
            }

            m_binderMap.Add(key, dataBinder);
        }
    }

    public interface IModuleBinder<T> where T : BaseModule
    {

    }

    public static class IModuleBinderExt
    {
        public static T Module<T>(this IModuleBinder<T> @this) where T : BaseModule
        {
            return ModuleManager.GetModule<T>();
        }

        public static void GetData<T, TData>(this IModuleBinder<T> @this, string key, out TData data) where T : BaseModule
        {
            ModuleManager.GetModule<T>().GetData(key, out data);
        }
    }
}