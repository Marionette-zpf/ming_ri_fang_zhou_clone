using Config;
using Manager;
using Manager.Res;
using Module.Battle.Com;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public List<Spawner> Spawners;

        private GameObject go;

        void Start()
        {
            ResManager.LoadAssetAsync(UnitDao.Inst.GetCfg(1).ResUrl, loader =>
            {
                go = loader.Get<GameObject>();

                StartCoroutine(StartBattle());
            });
        }

        private IEnumerator StartBattle()
        {
            for (int i = 0; i < Spawners.Count; i++)
            {
                var spawner = Spawners[i];

                yield return new WaitForSeconds(spawner.Delay);

                for (int j = 0; j < spawner.Infos.Count; j++)
                {
                    var info = spawner.Infos[j];
                    yield return new WaitForSeconds(info.Delay);

                    var unitCfg = UnitDao.Inst.GetCfg(info.EnemyId);
                    var properties = UnitPropertiesDao.Inst.GetCfg(unitCfg.PropertiesKey);

                    var enemy = Instantiate(go).GetComponent<BaseEnemyUnit>();
                    enemy.Initialize(properties, DataManager.UnitPathExts[spawner.Path]);
                    //enemy.transform.position = DataManager.UnitPathExts[0].GetStartPos();
                    //enemy.SetProperties(properties);
                }
            }
        }

    }

    [Serializable]
    public class Spawner
    {
        public List<SpawneInfo> Infos;
        public float Delay;
        public int Path;
    }

    [Serializable]
    public class SpawneInfo
    {
        public int EnemyId;
        public float Delay;
        public float Level;
    }
}