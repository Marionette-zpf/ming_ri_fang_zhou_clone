
namespace EasyWork.Utilities
{
    public class ESingleton<T> where T : new()
    {
        private static T g_instance;

        public static bool Empty => g_instance == null;

        public static T Get()
        {
            if(g_instance == null)
            {
                g_instance = new T();
            }

            return g_instance;
        }
    }
}

