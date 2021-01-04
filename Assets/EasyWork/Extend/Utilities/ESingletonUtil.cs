using EasyWork.Utilities;

namespace EasyWork.Extend.Utilities
{
    public static class ESingletonUtil
    {
        public static T Get<T>() where T :  new()
        {
            return ESingleton<T>.Get();
        }
    }
}