using EasyUI;
using EasyUI.Ext;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ext
{
    /// <summary>
    /// Date    2020/12/24 0:27:39
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class ResLayoutGroupExt<TCom, TData> : LayoutGroupExt<TCom, TData>, IDisposable where TCom : EUiCom
    {
        public ResLayoutGroupExt(LayoutGroup layoutGroup, Action<int, TCom, TData> itemRender, Action<int, TCom, TData> itemClickHandle = null) : base(layoutGroup, itemRender, itemClickHandle)
        {

        }

        public void Dispose()
        {
            foreach (var com in m_coms)
            {
                UnityEngine.Object.Destroy(com.gameObject);
            }
        }
    }
}