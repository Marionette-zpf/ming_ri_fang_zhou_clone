using System;
using System.Collections.Generic;

namespace EasyWork.Utilities
{
    public class EBinder<TKey,TValue> : IEBinder<TKey, TValue>
    {
        private Dictionary<TKey, TValue> binder = new Dictionary<TKey, TValue>();

        public event Action<TKey, TValue> OnBind;
        public event Action<TKey, TValue> OnRebind;

        public void Binding(TKey key, TValue value)
        {
            if (binder.ContainsKey(key))
            {
                throw new System.Exception($"binded key:{key} value:{value}");
            }
            else
            {
                binder.Add(key, value);
                OnBind?.Invoke(key, value);
            }
        }

        public void Rebind(TKey key, TValue value)
        {
            if (binder.ContainsKey(key))
            {
                binder[key] = value;
                OnRebind?.Invoke(key, value);
            }
            else
            {
                throw new System.Exception($"unbind key:{key} value:{typeof(TValue)}");
            }
        }

        public TValue GetValue(TKey key)
        {
            if (binder.ContainsKey(key))
            {
                return binder[key];
            }
            else
            {
                throw new System.Exception($"unbind key:{key} value:{typeof(TValue)}");
            }
        }
    }

}

