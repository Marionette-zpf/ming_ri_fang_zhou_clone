using EasyWork.Utilities;
using System;

namespace EasyWork.Extend.Utilities
{
    public static class EBinderUtil
    {
        public static void Binding<TKey, TValue>(TKey key, TValue value)
        {
            ESingletonUtil.Get<EBinder<TKey, TValue>>().Binding(key, value);
        }

        public static void OnBindedHandler<TKey, TValue>(Action<TKey, TValue> OnBind)
        {
            ESingletonUtil.Get<EBinder<TKey, TValue>>().OnBind += OnBind;
        }

        public static void OnRebindedHandler<TKey, TValue>(Action<TKey, TValue> OnRebind)
        {
            ESingletonUtil.Get<EBinder<TKey, TValue>>().OnRebind += OnRebind;
        }

        public static void Get<TKey, TValue>(TKey key,out TValue value)
        {
            value = ESingletonUtil.Get<EBinder<TKey, TValue>>().GetValue(key);
        }
    }

}

