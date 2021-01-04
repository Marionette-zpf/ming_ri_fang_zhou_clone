using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyWork.Utilities
{
    public class ERuler<T>
    {
        private Dictionary<T, ERulderInfo> m_rulerMap = new Dictionary<T, ERulderInfo>();

        public void AddRuler(T ruler)
        {
            m_rulerMap.Add(ruler, new ERulderInfo());
        }

        public void Clear()
        {
            var rulerEnumerator = m_rulerMap.GetEnumerator();
            while (rulerEnumerator.MoveNext())
            {
                var pair = rulerEnumerator.Current;
                m_rulerMap[pair.Key].RulerForce = 1;
                m_rulerMap[pair.Key].Weight = 1;
            }
        }

        public void AddWeight(T ruler, float value)
        {
            m_rulerMap[ruler].RulerForce += value;
        }

        public void DisableRuler(T ruler)
        {
            m_rulerMap[ruler].Weight = 0;
        }

        public void LogRulers()
        {
            foreach (var item in m_rulerMap)
            {
                Debug.LogError($"ruler:{item.Key}, force:{item.Value.RulerForce}, weight:{item.Value.Weight}, value:{item.Value.GetValue()}");
            }
        }

        public T GetRandom(T defaultDir)
        {
            float total = 0.000000001f;
            float curValue = 0.000000001f;

            var rulerEnumerator = m_rulerMap.GetEnumerator();
            while (rulerEnumerator.MoveNext())
            {
                var pair = rulerEnumerator.Current;
                total += m_rulerMap[pair.Key].GetValue();
            }


            float randomValue = (Random.value + 0.000000001f) * total;

            rulerEnumerator = m_rulerMap.GetEnumerator();
            while (rulerEnumerator.MoveNext())
            {
                var pair = rulerEnumerator.Current;
                curValue += m_rulerMap[pair.Key].GetValue();

                if (curValue >= randomValue)
                {
                    return pair.Key;
                }
            }

            return defaultDir;
        }
    }
}

