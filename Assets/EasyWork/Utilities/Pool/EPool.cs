using EasyWork.Extend.Utilities;
using System;
using System.Collections.Generic;

namespace EasyWork.Utilities
{
    public class EPool<T> : IEPool<T>
    {
        private Stack<T> m_cache = new Stack<T>();

        public int Count => m_cache.Count;

        public event Func<T> Creator;

        public event Action<T> OnGet;
        public event Action<T> OnCreate;
        public event Action<T> OnRecycle;
        public event Action<T> OnClear;

        public void SetCreator(Func<T> creator)
        {
            Creator = creator;
        }

        public T Get()
        {
            T result;

            if(Count == 0)
            {
                if(Creator == null)
                {
                    throw new Exception("EControllablePool Creator must be not null");
                }
                else
                {
                    result = Creator.Invoke();
                    OnCreate?.Invoke(result);
                }
            }
            else
            {
                result = m_cache.Pop();
            }

            OnGet?.Invoke(result);

            return result;
        }

        public void Recycle(T obj)
        {
            m_cache.Push(obj);
            OnRecycle?.Invoke(obj);
        }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                OnClear?.Invoke(m_cache.Pop());
            }
            
            if(Count != 0)
            {
                throw new Exception("not cleaned up");
            }
        }
    }

}

