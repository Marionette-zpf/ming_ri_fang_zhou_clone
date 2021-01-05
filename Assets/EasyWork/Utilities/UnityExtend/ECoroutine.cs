using System.Collections;
using UnityEngine;


namespace EasyWork.Utilities
{
    public static class ECoroutine 
    {
        private static MonoBehaviour m_corourineMono;

        public static Coroutine StartCoroutine(IEnumerator enumerator)
        {
            if(m_corourineMono == null)
            {
                var go = new GameObject();
                go.hideFlags = HideFlags.HideInHierarchy;
                m_corourineMono = go.AddComponent<ECoroutineCom>();
                Object.DontDestroyOnLoad(go);
            }
            return m_corourineMono.StartCoroutine(enumerator);
        }

        public static void StopCoroutine(IEnumerator enumerator)
        {
            if(m_corourineMono == null)
            {
                return;
            }

            m_corourineMono.StopCoroutine(enumerator);
        }
    }

    class ECoroutineCom : MonoBehaviour
    {

    }
}

