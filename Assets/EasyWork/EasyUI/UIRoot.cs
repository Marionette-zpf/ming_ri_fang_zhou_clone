using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUI
{
    public enum UIHierarchyEnum
    {
        Top,
        Common,
        Base
    }

    /// <summary>
    /// Date    2020/12/22 15:02:38
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class UIRoot : BaseUIElement
    {
        private Dictionary<UIHierarchyEnum, UIHierarchy> m_uiIHierarchyMap = new Dictionary<UIHierarchyEnum, UIHierarchy>();

        private void Awake()
        {
            PanelManager.G_Root = this;

            for (UIHierarchyEnum i = UIHierarchyEnum.Top; i <= UIHierarchyEnum.Base; i++)
            {
                var uIHierarchy = UIHierarchy.Get(i.ToString());
                uIHierarchy.RectTransform.SetParent(RectTransform);
                uIHierarchy.RectTransform.pivot = new Vector2(0.5f, 0.5f);
                uIHierarchy.RectTransform.anchorMin = Vector2.zero;
                uIHierarchy.RectTransform.anchorMax = Vector2.one;
                uIHierarchy.RectTransform.sizeDelta = Vector2.zero;
                uIHierarchy.RectTransform.anchoredPosition = Vector2.zero;
                uIHierarchy.RectTransform.localScale = Vector3.one;
                m_uiIHierarchyMap.Add(i, uIHierarchy);
            }
        }

        public void AddChild(BaseUIElement uIElement, UIHierarchyEnum uIHierarchyEnum = UIHierarchyEnum.Common)
        {
            if (!m_uiIHierarchyMap.TryGetValue(uIHierarchyEnum, out var uIHierarchy))
            {
                Debug.LogError($"un find uIHierarchy:{uIHierarchyEnum}");
                return;
            }

            uIElement.RectTransform.SetParent(uIHierarchy.RectTransform, false);
        }
    }

    public class UIHierarchy : BaseUIElement
    {
        public static UIHierarchy Get(string name)
        {
            return new GameObject(name, typeof(RectTransform), typeof(UIHierarchy)).GetComponent<UIHierarchy>();
        }
    }
}