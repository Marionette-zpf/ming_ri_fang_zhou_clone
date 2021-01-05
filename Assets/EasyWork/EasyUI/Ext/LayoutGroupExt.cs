using Config;
using Manager.Res;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUI.Ext
{
    /// <summary>
    /// Date    2020/12/23 23:03:54
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class LayoutGroupExt<TCom, TData> where TCom : BaseUIElement
    {
        protected LayoutGroup m_layoutGroup;
        protected Action<int, TCom, TData> m_itemRenderHandle;
        protected Action<int, TCom, TData> m_itemClickHandle;

        protected IList<TData> m_data;
        protected List<TCom> m_coms = new List<TCom>();

        public LayoutGroupExt(LayoutGroup layoutGroup, Action<int, TCom, TData> itemRender, Action<int, TCom, TData> itemClickHandle = null)
        {
            m_layoutGroup = layoutGroup;
            m_itemRenderHandle = itemRender;
            m_itemClickHandle = itemClickHandle;

            var initComs = m_layoutGroup.transform.GetComponentsInChildren<TCom>();
            if (initComs.Length != 0)
            {
                m_coms.AddRange(initComs);
            }
        }

        public IList<TData> Data
        {
            set
            {
                if (value == null)
                {
                    for (int i = 0; i < m_coms.Count; i++)
                    {
                        m_coms[i].gameObject.SetActive(false);
                    }

                    return;
                }

                m_data = value;

                for (int i = 0; i < m_coms.Count; i++)
                {
                    m_coms[i].gameObject.SetActive(i < m_data.Count);
                }

                if (m_data.Count > m_coms.Count)
                {
                    var offset = m_data.Count - m_coms.Count;
                    for (int i = 0; i < offset; i++)
                    {
                        var cfg = ResBinderDao.Inst.GetCfg(typeof(TCom).Name.ToString());
                        var tempCom = GameObject.Instantiate(ResManager.LoadAsset<GameObject>(cfg.Url)).GetComponent<TCom>();
                        if (tempCom == null)
                        {
                            Debug.LogError($"un find com:{typeof(TCom)} from factory go");
                            continue;
                        }
                        tempCom.transform.SetParent(m_layoutGroup.transform);

                        m_coms.Add(tempCom);
                    }
                }

                for (int i = 0; i < m_coms.Count; i++)
                {
                    if (m_coms[i].gameObject.activeSelf)
                    {
                        m_itemRenderHandle.Invoke(i, m_coms[i], m_data[i]);
                    }
                }
            }
        }
    }

}