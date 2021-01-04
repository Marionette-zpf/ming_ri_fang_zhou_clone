using EasyWork.Extend.Utilities;
using EasyWork.Utilities;
using System;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Date    2020/12/20 22:46:53
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public static partial class StrHelper
    {
        /// <summary>
        /// str 转二维数组
        /// </summary>
        public static T[][] GetTwoDimensionalArray<T>(Func<string, T> convert, string str)
        {
            var str1Array = str.Split(SPLIT_CHAR_1);
            T[][] result = new T[str1Array.Length][];

            for (int i = 0; i < str1Array.Length; i++)
            {
                var str2Array = str1Array[i].Split(SPLIT_CHAR_2);
                result[i] = new T[str2Array.Length];

                for (int j = 0; j < str2Array.Length; j++)
                {
                    result[i][j] = convert.Invoke(str2Array[j]);
                }
            }

            return result;
        }
        public static StringBuilder GetSb()
        {
            return GetSbPool().Get();
        }

        public static string ToStrAndRecycle(this StringBuilder @this)
        {
            var str = @this.ToString();
            @this.Recycle();

            return str;
        }
    }

    public static partial class StrHelper
    {
        public const char SPLIT_CHAR_1 = '|';
        public const char SPLIT_CHAR_2 = '*';

        private static IEPool<StringBuilder> g_sbPool;

        private static IEPool<StringBuilder> GetSbPool()
        {
            if(g_sbPool == null)
            {
                g_sbPool = EPoolUtil.CreatePool(() => new StringBuilder())
                                    .OnRecycleHandler(sb => sb.Clear());
            }

            return g_sbPool;
        }

    }
}