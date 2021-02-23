using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Helper;

namespace Config
{
    public class EnemyDao : BaseConfigDao<int, EnemyConfig, EnemyDao>
    {
        public EnemyDao()
        {
            m_configMap = new Dictionary<int, EnemyConfig>();
            List<ExcelRowInfo> excelRowInfos = ExcelHelper.ToTable(File.ReadAllText(Application.dataPath + "/Config/Enemy.csv"));
            ConfigTableReader configTableReader = new ConfigTableReader();
            for (int i = 3; i < excelRowInfos.Count; i++)
            {
                configTableReader.RowValue = excelRowInfos[i];
                EnemyConfig tempCfg = new EnemyConfig();
                EnemyDecode.Decode(tempCfg, configTableReader);

                m_configMap.Add(tempCfg.Key, tempCfg);
            }
        }
    }
}