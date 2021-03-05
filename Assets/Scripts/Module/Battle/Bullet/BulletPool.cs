using EasyWork.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle.Bullet
{
    /// <summary>
    /// Date    2021/2/24 10:49:21
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public static class BulletPool
    {
        private static Dictionary<GameObject, EPool<BaseBullet>> g_pool = new Dictionary<GameObject, EPool<BaseBullet>>();

        public static bool ExistPool(GameObject prefab)
        {
            return g_pool.ContainsKey(prefab);
        }

        public static EPool<BaseBullet> RegisterPool(GameObject prefab)
        {
            if (ExistPool(prefab))
            {
                return g_pool[prefab];
            }

            g_pool[prefab] = new EPool<BaseBullet>();
            g_pool[prefab].SetCreator(() => GameObject.Instantiate(prefab).GetComponent<BaseBullet>());
            return g_pool[prefab];
        }

        public static EPool<BaseBullet> GetPool(GameObject prefab)
        {
            return g_pool[prefab];
        }

        public static BaseBullet Get(GameObject prefab)
        {
            if (!g_pool.ContainsKey(prefab))
            {
                RegisterPool(prefab);
            }

            return g_pool[prefab].Get();
        }

        public static BaseBullet Recycle(GameObject prefab)
        {
            return g_pool[prefab].Get();
        }
    }
}