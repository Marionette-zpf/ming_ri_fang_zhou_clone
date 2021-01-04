using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Helper;

namespace Config
{
    public class ResBinderDao : BaseConfigDao<string, ResBinderConfig, ResBinderDao>
    {
        public ResBinderDao()
        {
            m_configMap = new Dictionary<string, ResBinderConfig>();
            List<ExcelRowInfo> excelRowInfos = ExcelHelper.ToTable(File.ReadAllText(Application.dataPath + "/Config/ResBinder.csv"));
            ConfigTableReader configTableReader = new ConfigTableReader();
            for (int i = 3; i < excelRowInfos.Count; i++)
            {
                configTableReader.RowValue = excelRowInfos[i];
                ResBinderConfig tempCfg = new ResBinderConfig();
                ResBinderDecode.Decode(tempCfg, configTableReader);

                m_configMap.Add(tempCfg.Key, tempCfg);
            }
        }
    }
}