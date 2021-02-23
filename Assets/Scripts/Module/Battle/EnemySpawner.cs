using Config;
using Manager;
using Manager.Res;
using Module.Battle.Com;
using System;
using UnityEngine;

namespace Module.Battle
{
    /// <summary>
    /// Date    2021/2/14 15:51:13
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public LevelDataManager DataManager;
        public SpawneInfo SpawneInfo;

        private GameObject go;

        void Start()
        {
            ResManager.LoadAssetAsync(EnemyDao.Inst.GetCfg(1).ResUrl, loader =>
            {
                go = loader.Get<GameObject>();
                var enemy = Instantiate(go);
                enemy.GetComponent<BaseEnemyUnit>().SetPath(DataManager.UnitPathExts[0]);
                enemy.transform.position = DataManager.UnitPathExts[0].GetStartPos();
            });
        }

        void Update()
        {

        }
    }

    [Serializable]
    public class SpawneInfo
    {
        public int EnemyId;
    }
}