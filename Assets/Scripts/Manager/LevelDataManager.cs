using Module.Battle;
using Module.Battle.Com;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Date    2021/2/14 15:45:55
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class LevelDataManager : MonoBehaviour
    {
        public static LevelDataManager DataMgr;
        public LevelMapInfo MapInfo;

        public LevelMapInfoExt MapInfoExt;
        public UnitPathExt[] UnitPathExts;

        private void Awake()
        {
            DataMgr = this;

            MapInfoExt = new LevelMapInfoExt(MapInfo);

            UnitPathExts = new UnitPathExt[MapInfo.EnemyPathArray.Length];
            for (int i = 0; i < UnitPathExts.Length; i++)
            {
                UnitPathExts[i] = new UnitPathExt(MapInfo.EnemyPathArray[i], MapInfo);
            }

            ModuleManager.GetModule<BattleModule>().InitUnitMap(MapInfo);
        }
    }
}