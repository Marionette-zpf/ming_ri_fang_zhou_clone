using Helper;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ConfigGenerator : EditorWindow
{
    public TextAsset m_config;


    [MenuItem("EasyWork/配置")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(ConfigGenerator));
        window.Show();
    }

    private void OnGUI()
    {
        m_config = (TextAsset)EditorGUILayout.ObjectField("Config", m_config, typeof(TextAsset), false);

        if (GUILayout.Button("生成配置"))
        {
            Generator();
        }
    }


    private const string g_fieldTemplate = @"
        /// <summary>
        /// #描述#
        /// </summary>
        public #字段类型# #字段名#;
";

    private const string g_configTemplate = @"namespace Config
{
    public class #类名#Config
    {
        #字段#
    }
}
";
    private const string g_configDaoTemplate = @"using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Helper;

namespace Config
{
    public class #类名#Dao : BaseConfigDao<#主键#, #类名#Config, #类名#Dao>
    {
        public #类名#Dao()
        {
            m_configMap = new Dictionary<#主键#, #类名#Config>();
            List<ExcelRowInfo> excelRowInfos = ExcelHelper.ToTable(File.ReadAllText(Application.dataPath + ""/Config/#类名#.csv""));
            ConfigTableReader configTableReader = new ConfigTableReader();
            for (int i = 3; i < excelRowInfos.Count; i++)
            {
                configTableReader.RowValue = excelRowInfos[i];
                #类名#Config tempCfg = new #类名#Config();
                #类名#Decode.Decode(tempCfg, configTableReader);

                m_configMap.Add(tempCfg.#主键名#, tempCfg);
            }
        }
    }
}";

    private const string g_cfgDecodeReadTemplate = @"
            tableReader.Read#类型#(#下标#, out cfg.#字段名#);";

    private const string g_configDecodeTemplate = @"using Helper;

namespace Config
{
    public static class #类名#Decode
    {
        public static void Decode(#类名#Config cfg, ConfigTableReader tableReader)
        {
            #读取#
        }
    }
}
";


    private void Generator()
    {
        var excelRowInfos = ExcelHelper.ToTable(m_config.text);
        ExcelRowInfo rowDescript = excelRowInfos[0];
        ExcelRowInfo rowType = excelRowInfos[1];
        ExcelRowInfo rowName = excelRowInfos[2];

        var className = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(m_config));

        var configPath = Application.dataPath + "/Scripts/Config/" + className + "/";

        if (!Directory.Exists(configPath))
        {
            Directory.CreateDirectory(configPath);
        }

        //创建 config 类
        int fieldCount = rowType.Contexts.Length;

        //长度必须相同
        for (int i = 0; i < excelRowInfos.Count; i++)
        {
            if(fieldCount != excelRowInfos[i].Contexts.Length)
            {
                Debug.LogError($"配置数据不一致行数为:{i}");
                return;
            }
        }

        var sb = new StringBuilder();
        for (int i = 0; i < fieldCount; i++)
        {
            sb.Append(g_fieldTemplate.Replace("#字段类型#", rowType.Contexts[i]).Replace("#字段名#", rowName.Contexts[i]).Replace("#描述#", rowDescript.Contexts[i]));
        }

        string configCode = g_configTemplate.Replace("#类名#", className).Replace("#字段#", sb.ToString());
        File.WriteAllText(configPath + className + "Config.cs", configCode);

        //创建 configDao 类
        string configDaoCode = g_configDaoTemplate;
        configDaoCode = configDaoCode.Replace("#类名#", className);
        configDaoCode = configDaoCode.Replace("#主键#", rowType.Contexts[0]);
        configDaoCode = configDaoCode.Replace("#主键名#", rowName.Contexts[0]);
        File.WriteAllText(configPath + className + "Dao.cs", configDaoCode);

        //创建 configDecode 类
        sb.Clear();
        for (int i = 0; i < fieldCount; i++)
        {
            sb.Append(g_cfgDecodeReadTemplate.Replace("#类型#", g_fieldTypeMap[rowType.Contexts[i]]).Replace("#下标#", i.ToString()).Replace("#字段名#", rowName.Contexts[i]));
        }

        string cfgDecodeCode = g_configDecodeTemplate.Replace("#类名#", className).Replace("#读取#", sb.ToString());
        File.WriteAllText(configPath + className + "Decode.cs", cfgDecodeCode);

        AssetDatabase.Refresh();
    }

    private static Dictionary<string, string> g_fieldTypeMap = new Dictionary<string, string>()
    {
        { "int", "Int" }, {"uint", "Uint"}, {"string", "String"}, {"int[]", "IntArray"},
        { "uint[]", "UintArray" }, { "string[]", "StringArray"}, {"int[][]", "IntArray2"},
        { "uint[][]", "UintArray2"}
    };

}

public class BaseConfigDao<TKey, TCfg, TDao> where TDao : new()
{
    public static TDao g_instance;
    public static TDao Inst
    {
        get
        {
            if(g_instance == null)
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

    public void ReadStringArray(int index, out string[] value, char split = '|')
    {
        var strArray = RowValue.Contexts[index].Split(split);
        value = new string[strArray.Length];

        for (int i = 0; i < value.Length; i++)
        {
            value[i] = strArray[index];
        }
    }

    public void ReadIntArray2(int index, out int[][] value, char split = '|', char split2 = '*')
    {
        var strArray = RowValue.Contexts[index].Split(split);

        value = new int[strArray.Length][];

        for (int i = 0; i < strArray.Length; i++)
        {
            var strArray2 = strArray[i].Split(split2);
            value[i] = new int[strArray2.Length];

            for (int j = 0; j < strArray2.Length; j++)
            {
                value[i][j] = int.Parse(strArray2[j]);
            }
        }
    }

    public void ReadUintArray2(int index, out uint[][] value, char split = '|', char split2 = '*')
    {
        var strArray = RowValue.Contexts[index].Split(split);

        value = new uint[strArray.Length][];

        for (int i = 0; i < strArray.Length; i++)
        {
            var strArray2 = strArray[i].Split(split2);
            value[i] = new uint[strArray2.Length];

            for (int j = 0; j < strArray2.Length; j++)
            {
                value[i][j] = uint.Parse(strArray2[j]);
            }
        }
    }
}

