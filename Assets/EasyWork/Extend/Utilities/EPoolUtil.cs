using EasyWork.Utilities;
using System;


namespace EasyWork.Extend.Utilities
{
    public static class EPoolUtil
    {
        public static IEPool<T> CreatePool<T>(Func<T> factory, uint initCount = 0)
        {
            if (ESingleton<EPool<T>>.Empty)
            {
                var pool = ESingletonUtil.Get<EPool<T>>();
                pool.SetCreator(factory);

                for (int i = 0; i < initCount; i++)
                {
                    pool.Recycle(factory.Invoke());
                }

                return pool;
            }
            else
            {
                ELogUtil.LogError("已经创建该Pool");
                return ESingletonUtil.Get<EPool<T>>();
            }
        }

        public static IEPool<T> GetPool<T>()
        {
            if (ESingleton<EPool<T>>.Empty)
            {
                ELogUtil.LogError("pool has not yet create");
                return null;
            }
            else
            {
                return ESingletonUtil.Get<EPool<T>>();
            }
        }

        public static IEPool<T> OnCreatorHandler<T>(this IEPool<T> @this, Action<T> onCreate)
        {
            EPool<T> pool = @this is EPool<T> ? @this as EPool<T> : GetPool<T>() as EPool<T>;
            pool.OnCreate += onCreate;
            return pool;
        }

        public static IEPool<T> OnGetHandler<T>(this IEPool<T> @this, Action<T> onGet)
        {
            EPool<T> pool = @this is EPool<T> ? @this as EPool<T> : GetPool<T>() as EPool<T>;
            pool.OnGet += onGet;
            return pool;
        }

        public static IEPool<T> OnRecycleHandler<T>(this IEPool<T> @this, Action<T> recycle)
        {
            EPool<T> pool = @this is EPool<T> ? @this as EPool<T> : GetPool<T>() as EPool<T>;
            pool.OnRecycle += recycle;
            return pool;
        }

        public static IEPool<T> OnClearHandler<T>(this IEPool<T> @this, Action<T> onClear)
        {
            EPool<T> pool = @this is EPool<T> ? @this as EPool<T> : GetPool<T>() as EPool<T>;
            pool.OnClear += onClear;
            return pool;
        }

        public static void Recycle<T>(this T @this)
        {
            var pool = GetPool<T>();
            if (pool != null)
            {
                pool.Recycle(@this);
            }
            else
            {
                ELogUtil.LogError($"should create pool<{typeof(T)}> by EPoolUtil.CreatePool");
            }
        }

        public static T Get<T>()
        {
            var pool = GetPool<T>();
            if (pool != null)
            {
                return pool.Get();
            }
            else
            {
                ELogUtil.LogError($"should create pool<{typeof(T)}> by EPoolUtil.CreatePool");
                return default;
            }
        }
    }
}


