using EasyWork.Utilities;
using System;

namespace EasyWork.Extend.Utilities
{
    public static class EEventUtil
    {
        public static void Subscribe<T>(Action<T> eventHandler)
        {
            ESingletonUtil.Get<EEvent<T>>().Subscribe(eventHandler);
        }

        public static void UnSubscribe<T>(Action<T> eventHandler)
        {
            ESingletonUtil.Get<EEvent<T>>().UnSubscribe(eventHandler);
        }

        public static void Subscribe<T>(Action eventHandler)
        {
            ESingletonUtil.Get<EEvent<T>>().Subscribe(eventHandler);
        }

        public static void UnSubscribe<T>(Action eventHandler)
        {
            ESingletonUtil.Get<EEvent<T>>().UnSubscribe(eventHandler);
        }

        public static void Dispatch<T>(T param)
        {
            ESingletonUtil.Get<EEvent<T>>().Dispatch(param);
        }

        public static void Dispatch<T>()
        {
            ESingletonUtil.Get<EEvent<T>>().Dispatch();
        }
    }
}
