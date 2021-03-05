using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Helper;

namespace Config
{
    public class UnitPropertiesDao : BaseConfigDao<uint, UnitPropertiesConfig, UnitPropertiesDao>
    {
        public UnitPropertiesDao()
        {
            m_configMap = new Dictionary<uint, UnitPropertiesConfig>();
            List<ExcelRowInfo> excelRowInfos = ExcelHelper.ToTable(File.ReadAllText(Application.dataPath + "/Config/UnitProperties.csv"));
            ConfigTableReader configTableReader = new ConfigTableReader();
            for (int i = 3; i < excelRowInfos.Count; i++)
            {
                configTableReader.RowValue = excelRowInfos[i];
                UnitPropertiesConfig tempCfg = new UnitPropertiesConfig();
                UnitPropertiesDecode.Decode(tempCfg, configTableReader);

                m_configMap.Add(tempCfg.Id, tempCfg);
            }
        }
    }
}