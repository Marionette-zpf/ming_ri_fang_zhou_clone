using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Helper;

namespace Config
{
    public class CharacterDao : BaseConfigDao<uint, CharacterConfig, CharacterDao>
    {
        public CharacterDao()
        {
            m_configMap = new Dictionary<uint, CharacterConfig>();
            List<ExcelRowInfo> excelRowInfos = ExcelHelper.ToTable(File.ReadAllText(Application.dataPath + "/Config/Character.csv"));
            ConfigTableReader configTableReader = new ConfigTableReader();
            for (int i = 3; i < excelRowInfos.Count; i++)
            {
                configTableReader.RowValue = excelRowInfos[i];
                CharacterConfig tempCfg = new CharacterConfig();
                CharacterDecode.Decode(tempCfg, configTableReader);

                m_configMap.Add(tempCfg.Id, tempCfg);
            }
        }
    }
}