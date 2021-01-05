using System.Collections.Generic;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Date    2020/12/27 13:44:24
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class ExcelHelper
    {
        public static List<ExcelRowInfo> ToTable(string context)
        {
            var result = new List<ExcelRowInfo>();
            var tempCache = new List<string>();
            var length = context.Length;
            var sb = new StringBuilder();

            int row = 0;

            for (int i = 0; i < length; i++)
            {
                var curChar = context[i];
                if (curChar == ',')
                {
                    ToString(tempCache, sb);
                }
                else if (curChar == '\r' && context[i + 1] == '\n')
                {
                    i++;
                    ToString(tempCache, sb);

                    result.Add(new ExcelRowInfo() { Row = row++, Contexts = tempCache.ToArray() });
                    tempCache.Clear();
                }
                else
                {
                    sb.Append(curChar);
                }
            }

            ToString(tempCache, sb);

            result.Add(new ExcelRowInfo() { Row = row++, Contexts = tempCache.ToArray() });
            tempCache.Clear();

            return result;
        }


        static void ToString(List<string> cache, StringBuilder sb)
        {
            cache.Add(sb.ToString());
            sb.Clear();
        }
    }

    public class ExcelRowInfo
    {
        public int Row;
        public string[] Contexts;
    }


    public class BaseConfigDao<TKey, TCfg, TDao> where TDao : new()
    {
        public static TDao g_instance;
        public static TDao Inst
        {
            get
            {
                if (g_instance == null)
                {
                    g_instance = new TDao();
                }

                return g_instance;
            }
        }

        protected Dictionary<TKey, TCfg> m_configMap;

        public TCfg GetCfg(TKey key)
        {
            return m_configMap[key];
        }
    }

    public class ConfigTableReader
    {
        public ExcelRowInfo RowValue;

        public void ReadUint(int index, out uint value)
        {
            value = uint.Parse(RowValue.Contexts[index]);
        }

        public void ReadInt(int index, out int value)
        {
            value = int.Parse(RowValue.Contexts[index]);
        }

        public void ReadString(int index, out string value)
        {
            value = RowValue.Contexts[index];
        }

        public void ReadIntArray(int index, out int[] value, char split = '|')
        {
            var intArrayStr = RowValue.Contexts[index].Split(split);
            value = new int[intArrayStr.Length];

            for (int i = 0; i < value.Length; i++)
            {
                value[i] = int.Parse(intArrayStr[index]);
            }
        }

        public void ReadUintArray(int index, out uint[] value, char split = '|')
        {
            var intArrayStr = RowValue.Contexts[index].Split(split);
            value = new uint[intArrayStr.Length];

            for (int i = 0; i < value.Length; i++)
            {
                value[i] = uint.Parse(intArrayStr[index]);
            }
        }
    }


}

