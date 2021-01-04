using EasyWork.Utilities;

namespace EasyWork.Extend.Utilities
{
    public static class ELogUtil 
    {
        private readonly static IELogger g_logger = new EUnityLogger();
        private readonly static IEAssert g_assert = new EUnityAssert();

        public static void Assert(bool condition, string context)
        {
            g_assert.Assert(condition, context);
        }

        public static void Assert(bool condition, object context)
        {
            Assert(condition, context.ToString());
        }

        public static void Log(string context)
        {
            g_logger.Log(context);
        }

        public static void Log(object context)
        {
            Log(context.ToString());
        }

        public static void LogError(string context)
        {
            g_logger.LogError(context);
        }

        public static void LogError(object context)
        {
            LogError(context.ToString());
        }

        public static void LogWarning(string context)
        {
            g_logger.LogWarning(context);
        }

        public static void LogWarning(object context)
        {
            LogWarning(context.ToString());
        }
    }

}
