using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Helper;

namespace Config
{
    public class UnitDao : BaseConfigDao<int, UnitConfig, UnitDao>
    {
        public UnitDao()
        {
            m_configMap = new Dictionary<int, UnitConfig>();
            List<ExcelRowInfo> excelRowInfos = ExcelHelper.ToTable(File.ReadAllText(Application.dataPath + "/Config/Unit.csv"));
            ConfigTableReader configTableReader = new ConfigTableReader();
            for (int i = 3; i < excelRowInfos.Count; i++)
            {
                configTableReader.RowValue = excelRowInfos[i];
                UnitConfig tempCfg = new UnitConfig();
                UnitDecode.Decode(tempCfg, configTableReader);

                m_configMap.Add(tempCfg.Key, tempCfg);
            }
        }
    }
}